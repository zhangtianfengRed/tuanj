using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BlinkDetection
{
    /// <summary>
    /// 眨眼校准 UI：黑屏 + 圆点 + 按钮。
    /// </summary>
    public class BlinkCalibrationUI : MonoBehaviour
    {
        [Header("UI 参数")]
        [SerializeField] private int dotCount = 3;
        [SerializeField] private float dotSize = 40f;
        [SerializeField] private Color dotColor = new Color(1f, 1f, 1f, 0.9f);
        [SerializeField] private Color dotDoneColor = new Color(0.3f, 1f, 0.5f, 0.5f);
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private int fontSize = 28;

        private Canvas canvas;
        private Image blackOverlay;
        private Text statusText;
        private readonly List<Image> dots = new List<Image>();
        private Button yesButton;
        private Button noButton;
        private GameObject buttonsContainer;

        public event Action OnYesClicked;
        public event Action OnNoClicked;

        public bool IsVisible => canvas != null && canvas.gameObject.activeSelf;

        private void Awake()
        {
            CreateUI();
            Hide();
        }

        private void CreateUI()
        {
            // Root Canvas
            var canvasGo = new GameObject("BlinkCalibrationUICanvas");
            canvasGo.transform.SetParent(transform, false);
            canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 6000;

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            canvasGo.AddComponent<GraphicRaycaster>();

            // Black overlay
            var overlayGo = new GameObject("BlackOverlay");
            overlayGo.transform.SetParent(canvasGo.transform, false);
            blackOverlay = overlayGo.AddComponent<Image>();
            blackOverlay.color = new Color(0, 0, 0, 0.92f);
            var overlayRect = blackOverlay.rectTransform;
            overlayRect.anchorMin = Vector2.zero;
            overlayRect.anchorMax = Vector2.one;
            overlayRect.offsetMin = Vector2.zero;
            overlayRect.offsetMax = Vector2.zero;

            // Status text
            var textGo = new GameObject("StatusText");
            textGo.transform.SetParent(canvasGo.transform, false);
            statusText = textGo.AddComponent<Text>();
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            statusText.fontSize = fontSize;
            statusText.color = textColor;
            statusText.alignment = TextAnchor.MiddleCenter;
            statusText.raycastTarget = false;
            var textRect = statusText.rectTransform;
            textRect.anchorMin = new Vector2(0.5f, 0.55f);
            textRect.anchorMax = new Vector2(0.5f, 0.55f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(1000, 200);

            // Dots container
            var dotsGo = new GameObject("Dots");
            dotsGo.transform.SetParent(canvasGo.transform, false);
            var dotsRect = dotsGo.AddComponent<RectTransform>();
            dotsRect.anchorMin = new Vector2(0.5f, 0.4f);
            dotsRect.anchorMax = new Vector2(0.5f, 0.4f);
            dotsRect.pivot = new Vector2(0.5f, 0.5f);
            float spacing = dotSize + 30f;
            dotsRect.sizeDelta = new Vector2(spacing * dotCount, dotSize);
            dotsRect.anchoredPosition = Vector2.zero;

            // Create dots
            for (int i = 0; i < dotCount; i++)
            {
                var dotGo = new GameObject($"Dot{i}");
                dotGo.transform.SetParent(dotsGo.transform, false);
                var dot = dotGo.AddComponent<Image>();
                dot.color = dotColor;
                dot.raycastTarget = false;
                var dotRect = dot.rectTransform;
                dotRect.anchorMin = new Vector2(0.5f, 0.5f);
                dotRect.anchorMax = new Vector2(0.5f, 0.5f);
                dotRect.pivot = new Vector2(0.5f, 0.5f);
                float x = (i - (dotCount - 1) * 0.5f) * spacing;
                dotRect.anchoredPosition = new Vector2(x, 0);
                dotRect.sizeDelta = new Vector2(dotSize, dotSize);
                dots.Add(dot);
            }

            // Y/N buttons (hidden by default)
            buttonsContainer = new GameObject("Buttons");
            buttonsContainer.transform.SetParent(canvasGo.transform, false);
            var btnRect = buttonsContainer.AddComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.2f);
            btnRect.anchorMax = new Vector2(0.5f, 0.2f);
            btnRect.pivot = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(400, 60);
            btnRect.anchoredPosition = Vector2.zero;

            yesButton = CreateButton("YesBtn", "准确 (Y)", new Color(0.2f, 0.8f, 0.3f), -110f);
            noButton = CreateButton("NoBtn", "不准 (N)", new Color(0.8f, 0.2f, 0.2f), 110f);

            buttonsContainer.SetActive(false);
        }

        private Button CreateButton(string name, string label, Color color, float xPos)
        {
            var go = new GameObject(name);
            go.transform.SetParent(buttonsContainer.transform, false);
            var img = go.AddComponent<Image>();
            img.color = color;
            var btn = go.AddComponent<Button>();
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(180, 50);
            rect.anchoredPosition = new Vector2(xPos, 0);

            var labelGo = new GameObject("Label");
            labelGo.transform.SetParent(go.transform, false);
            var labelTxt = labelGo.AddComponent<Text>();
            labelTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelTxt.fontSize = 22;
            labelTxt.color = Color.white;
            labelTxt.alignment = TextAnchor.MiddleCenter;
            labelTxt.raycastTarget = false;
            var labelRect = labelTxt.rectTransform;
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            return btn;
        }

        public void ShowVerifyButtons()
        {
            if (buttonsContainer != null) buttonsContainer.SetActive(true);
            if (yesButton != null) yesButton.onClick.RemoveAllListeners();
            if (noButton != null) noButton.onClick.RemoveAllListeners();
            if (yesButton != null) yesButton.onClick.AddListener(() => OnYesClicked?.Invoke());
            if (noButton != null) noButton.onClick.AddListener(() => OnNoClicked?.Invoke());
        }

        public void HideVerifyButtons()
        {
            if (buttonsContainer != null) buttonsContainer.SetActive(false);
        }

        public void Show()
        {
            if (canvas != null) canvas.gameObject.SetActive(true);
            foreach (var dot in dots)
            {
                dot.color = dotColor;
                dot.gameObject.SetActive(true);
            }
            HideVerifyButtons();
        }

        public void Hide()
        {
            if (canvas != null) canvas.gameObject.SetActive(false);
            HideVerifyButtons();
        }

        public void SetStatus(string text)
        {
            if (statusText != null) statusText.text = text;
        }

        /// <summary>
        /// 标记第 index 个眨眼完成（隐藏对应圆点）。
        /// </summary>
        public void SetDotCompleted(int index)
        {
            if (index >= 0 && index < dots.Count)
            {
                dots[index].color = dotDoneColor;
            }
        }

        public int DotCount => dotCount;
    }
}
