using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DlibFaceLandmarkDetector;

namespace BlinkDetection
{
    /// <summary>
    /// 基于 Dlib 68 点面部关键点 + EAR（Eye Aspect Ratio）的眨眼检测。
    /// Dlib 比 MediaPipe 快得多（CPU 上 30+ FPS），EAR 算法成熟稳定。
    /// 参考方案：PyImageSearch EAR threshold=0.3 + 3 consecutive frames。
    /// </summary>
    public class WebCamBlinkDetector : MonoBehaviour
    {
        #region Nested Types

        public enum BlinkState { Open, Closed }

        #endregion

        #region Inspector Fields

        [Header("摄像头")]
        [SerializeField] private string deviceName = "";
        [SerializeField] private int requestWidth = 320;
        [SerializeField] private int requestHeight = 240;
        [SerializeField] private int requestFps = 30;

        [Header("Dlib")]
        [Tooltip("68 点关键点模型文件路径。")]
        [SerializeField] private string shapePredictorPath = "DlibFaceLandmarkDetector/StreamingAssets/DlibFaceLandmarkDetector/sp_human_face_68_for_mobile.dat";

        [Header("EAR 检测（自适应基线）")]
        [Tooltip("EAR 跌到基线的此比例以下判定为闭眼。")]
        [Range(0.4f, 0.9f)] [SerializeField] private float closeRatio = 0.65f;
        [Tooltip("EAR 回升到基线的此比例以上判定为睁眼。")]
        [Range(0.5f, 0.95f)] [SerializeField] private float openRatio = 0.85f;
        [Tooltip("基线滚动窗口大小（帧数）。")]
        [SerializeField] private int baselineWindow = 60;
        [Tooltip("连续多少帧低于阈值才算闭眼。")]
        [SerializeField] private int consecFramesForClose = 3;
        [Tooltip("连续多少帧高于阈值才算睁眼。")]
        [SerializeField] private int consecFramesForOpen = 2;

        [Header("眼镜模式")]
        [Tooltip("是否戴眼镜。眼镜反光会降低关键点精度，启用后使用更低阈值 + 更强平滑。运行时按 G 键切换。")]
        [SerializeField] private bool glassesMode = true;
        [Tooltip("眼镜模式下的 EAR 阈值。")]
        [SerializeField] private float glassesThreshold = 0.13f;
        [Tooltip("无眼镜模式下的 EAR 阈值。")]
        [SerializeField] private float noGlassesThreshold = 0.25f;
        [Tooltip("眼镜模式下的连续帧要求（更高以过滤噪声）。")]
        [SerializeField] private int glassesConsecFrames = 4;
        [Tooltip("无眼镜模式下的连续帧要求。")]
        [SerializeField] private int noGlassesConsecFrames = 3;
        [Tooltip("切换眼镜模式的按键。")]
        [SerializeField] private KeyCode glassesToggleKey = KeyCode.G;

        [Header("时序")]
        [SerializeField] private float maxBlinkDuration = 1f;
        [SerializeField] private float cooldown = 0.2f;

        [Header("UI")]
        [SerializeField] private BlinkCalibrationUI calibrationUI;

        [Header("调试")]
        [SerializeField] private bool showDebugView = true;
        [SerializeField] private Vector2 debugViewSize = new Vector2(320f, 240f);
        [SerializeField] private int logEveryNFrames = 30;

        #endregion

        #region dlib 68 点 — 眼睛关键点索引

        // dlib 68 点中左眼索引：36~41
        private static readonly int[] LeftEyeIndices = { 36, 37, 38, 39, 40, 41 };
        // 右眼索引：42~47
        private static readonly int[] RightEyeIndices = { 42, 43, 44, 45, 46, 47 };

        #endregion

        #region Runtime State

        private WebCamTexture webCamTexture;
        private Texture2D frameTexture;
        private FaceLandmarkDetector faceLandmarkDetector;
        private bool modelReady;

        private BlinkState state = BlinkState.Open;
        private float eyesClosedTime;
        private float lastBlinkTime = -999f;
        private float rawEAR;
        private float smoothEAR;
        private bool earInitialized;
        private float baselineEAR;
        private readonly List<float> earHistory = new List<float>(120);
        private int belowCounter;
        private int aboveCounter;
        private int frameCounter;

        private RawImage debugRawImage;
        private Color32[] pixelBuffer;

        #endregion

        #region Properties

        public float CurrentEAR => smoothEAR;
        public float BaselineEAR => baselineEAR;
        public BlinkState CurrentState => state;
        public bool GlassesMode => glassesMode;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            if (calibrationUI == null) calibrationUI = gameObject.GetComponent<BlinkCalibrationUI>();
            if (calibrationUI == null) calibrationUI = gameObject.AddComponent<BlinkCalibrationUI>();
            if (showDebugView) CreateDebugView();
        }

        private void OnEnable()
        {
            StartCamera();
            InitializeDlib();
        }

        private void OnDisable()
        {
            StopCamera();
            StopDlib();
        }

        private void OnDestroy()
        {
            StopCamera();
            StopDlib();
            if (frameTexture != null) { Destroy(frameTexture); frameTexture = null; }
        }

        private void Update()
        {
            if (webCamTexture == null || !webCamTexture.isPlaying || !modelReady) return;
            if (!webCamTexture.didUpdateThisFrame) return;
            if (webCamTexture.width < 2 || webCamTexture.height < 2) return;

            // 切换眼镜模式
            if (Input.GetKeyDown(glassesToggleKey))
            {
                glassesMode = !glassesMode;
                // 眼镜模式下用更宽松的比例（更容易触发）
                closeRatio = glassesMode ? 0.70f : 0.65f;
                openRatio = glassesMode ? 0.80f : 0.85f;
                consecFramesForClose = glassesMode ? 4 : 3;
                consecFramesForOpen = glassesMode ? 3 : 2;
                state = BlinkState.Open;
                belowCounter = 0;
                aboveCounter = 0;
                earHistory.Clear();
                Debug.Log($"[WebCamBlinkDetector] 眼镜模式: {(glassesMode ? "开启" : "关闭")} | closeRatio={closeRatio} openRatio={openRatio}");
            }

            frameCounter++;
            ProcessFrame();
            UpdateStateMachine();

            if (logEveryNFrames > 0 && frameCounter % logEveryNFrames == 0)
            {
                Debug.Log($"[WebCamBlinkDetector] EAR={smoothEAR:F4} base={baselineEAR:F4} close={baselineEAR*closeRatio:F4} {state} glasses={glassesMode}");
            }
        }

        #endregion

        #region Camera

        private void StartCamera()
        {
            if (webCamTexture != null && webCamTexture.isPlaying) return;
            if (WebCamTexture.devices.Length == 0)
            {
                Debug.LogError("[WebCamBlinkDetector] 未检测到摄像头设备。");
                return;
            }
            if (string.IsNullOrEmpty(deviceName)) deviceName = WebCamTexture.devices[0].name;

            webCamTexture = new WebCamTexture(deviceName, requestWidth, requestHeight, requestFps);
            webCamTexture.Play();
            Debug.Log($"[WebCamBlinkDetector] 摄像头已启动: {deviceName} ({webCamTexture.width}x{webCamTexture.height})");
        }

        private void StopCamera()
        {
            if (webCamTexture != null)
            {
                if (webCamTexture.isPlaying) webCamTexture.Stop();
                webCamTexture = null;
            }
        }

        #endregion

        #region Dlib

        private void InitializeDlib()
        {
            if (modelReady) return;

            string fullPath = System.IO.Path.Combine(Application.dataPath, shapePredictorPath);
            Debug.Log($"[WebCamBlinkDetector] 模型路径: {fullPath} exists={System.IO.File.Exists(fullPath)}");

            faceLandmarkDetector = new FaceLandmarkDetector(fullPath);
            modelReady = true;

            // 根据眼镜模式设定初始参数
            closeRatio = glassesMode ? 0.70f : 0.65f;
            openRatio = glassesMode ? 0.80f : 0.85f;
            consecFramesForClose = glassesMode ? 4 : 3;
            consecFramesForOpen = glassesMode ? 3 : 2;

            Debug.Log($"[WebCamBlinkDetector] Dlib 初始化完成。眼镜: {(glassesMode ? "开" : "关")} | 自适应基线模式 | 按 G 切换");

            if (calibrationUI != null)
            {
                calibrationUI.Show();
                calibrationUI.SetStatus($"检测就绪！请眨眼\n眼镜模式: {(glassesMode ? "开" : "关")} (G键切换)");
                StartCoroutine(HideCalibrationUIDelayed(2f));
            }
        }

        private void StopDlib()
        {
            if (faceLandmarkDetector != null)
            {
                faceLandmarkDetector.Dispose();
                faceLandmarkDetector = null;
            }
            modelReady = false;
        }

        #endregion

        #region Frame Processing

        private void ProcessFrame()
        {
            int camW = webCamTexture.width;
            int camH = webCamTexture.height;

            if (frameTexture == null || frameTexture.width != camW || frameTexture.height != camH)
            {
                frameTexture = new Texture2D(camW, camH, TextureFormat.RGBA32, false);
                pixelBuffer = new Color32[camW * camH];
            }

            webCamTexture.GetPixels32(pixelBuffer);
            frameTexture.SetPixels32(pixelBuffer);
            frameTexture.Apply();

            // Dlib 检测
            faceLandmarkDetector.SetImage(pixelBuffer, camW, camH, 4, true);
            List<Rect> faces = faceLandmarkDetector.Detect();

            if (faces == null || faces.Count == 0)
            {
                smoothEAR = 0.3f; // 默认睁眼值
                return;
            }

            // 取第一张脸
            Rect face = faces[0];
            List<Vector2> landmarks = faceLandmarkDetector.DetectLandmark(face);

            if (landmarks == null || landmarks.Count < 68)
            {
                rawEAR = 0.2f;
                return;
            }

            // 计算 EAR
            float leftEAR = CalculateEAR(landmarks, LeftEyeIndices);
            float rightEAR = CalculateEAR(landmarks, RightEyeIndices);
            rawEAR = (leftEAR + rightEAR) * 0.5f;

            // EMA 平滑（减少抖动）
            if (!earInitialized)
            {
                smoothEAR = rawEAR;
                earInitialized = true;
            }
            else
            {
                smoothEAR = Mathf.Lerp(smoothEAR, rawEAR, 0.4f);
            }

            // 更新滚动基线（只用高于当前基线的值，避免眨眼拉低基线）
            earHistory.Add(smoothEAR);
            if (earHistory.Count > baselineWindow) earHistory.RemoveAt(0);
            if (earHistory.Count >= 10)
            {
                // 取 75 百分位作为基线（排除眨眼帧的影响）
                var sorted = new List<float>(earHistory);
                sorted.Sort();
                baselineEAR = sorted[sorted.Count * 3 / 4];
            }

            if (showDebugView) DrawDebugOverlay();
        }

        /// <summary>
        /// EAR = (|p2-p6| + |p3-p5|) / (2 * |p1-p4|)
        /// </summary>
        private static float CalculateEAR(List<Vector2> landmarks, int[] indices)
        {
            Vector2 p1 = landmarks[indices[0]];
            Vector2 p2 = landmarks[indices[1]];
            Vector2 p3 = landmarks[indices[2]];
            Vector2 p4 = landmarks[indices[3]];
            Vector2 p5 = landmarks[indices[4]];
            Vector2 p6 = landmarks[indices[5]];

            float v1 = Vector2.Distance(p2, p6);
            float v2 = Vector2.Distance(p3, p5);
            float h = Vector2.Distance(p1, p4);

            if (h < 0.0001f) return 0f;
            return (v1 + v2) / (2f * h);
        }

        #endregion

        #region State Machine

        private void UpdateStateMachine()
        {
            if (baselineEAR < 0.01f) return;

            float closeThr = baselineEAR * closeRatio;
            float openThr = baselineEAR * openRatio;

            switch (state)
            {
                case BlinkState.Open:
                    if (smoothEAR < closeThr)
                    {
                        belowCounter++;
                        aboveCounter = 0;
                        if (belowCounter >= consecFramesForClose)
                        {
                            state = BlinkState.Closed;
                            eyesClosedTime = Time.time;
                        }
                    }
                    else
                    {
                        belowCounter = 0;
                    }
                    break;

                case BlinkState.Closed:
                    if (smoothEAR > openThr)
                    {
                        aboveCounter++;
                        belowCounter = 0;
                        if (aboveCounter >= consecFramesForOpen)
                        {
                            float duration = Time.time - eyesClosedTime;
                            TryRaiseBlink(duration);
                            state = BlinkState.Open;
                            aboveCounter = 0;
                        }
                    }
                    else
                    {
                        aboveCounter = 0;
                        if (Time.time - eyesClosedTime > maxBlinkDuration)
                        {
                            state = BlinkState.Open;
                            belowCounter = 0;
                        }
                    }
                    break;
            }
        }

        private void TryRaiseBlink(float duration)
        {
            if (Time.time - lastBlinkTime < cooldown) return;
            lastBlinkTime = Time.time;

            float typicalCenter = 0.15f;
            float typicalRange = 0.25f;
            float confidence = 1f - Mathf.Clamp01(Mathf.Abs(duration - typicalCenter) / typicalRange);
            confidence = Mathf.Max(0f, confidence);

            var data = new BlinkEventData(Time.time, duration, confidence);
            Debug.Log($"[WebCamBlinkDetector] 眨眼! 持续={duration:F3}s EAR={smoothEAR:F4} base={baselineEAR:F4}");
            BlinkEventSystem.RaiseBlink(data);
        }

        #endregion

        #region Debug View

        private void CreateDebugView()
        {
            var canvasGo = new GameObject("BlinkDebugCanvas");
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5000;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            canvasGo.AddComponent<GraphicRaycaster>();

            var rawGo = new GameObject("BlinkDebugRawImage");
            rawGo.transform.SetParent(canvasGo.transform, false);
            debugRawImage = rawGo.AddComponent<RawImage>();
            debugRawImage.color = Color.white;

            var rect = debugRawImage.rectTransform;
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = new Vector2(-10f, 10f);
            rect.sizeDelta = debugViewSize;

            Vector3 scale = rect.localScale;
            scale.x = -Mathf.Abs(scale.x);
            rect.localScale = scale;
        }

        private void DrawDebugOverlay()
        {
            if (debugRawImage == null || frameTexture == null) return;
            debugRawImage.texture = frameTexture;
        }

        private System.Collections.IEnumerator HideCalibrationUIDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (calibrationUI != null) calibrationUI.Hide();
        }

        #endregion
    }
}
