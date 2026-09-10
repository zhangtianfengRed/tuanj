#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DlibFaceLandmarkDetector.Editor
{
    public class DlibFaceLandmarkDetectorBuildPreprocessor : IPreprocessBuildWithReport
    {
        // Public Properties
        public int callbackOrder { get { return 0; } }

        // Public Methods
        public void OnPreprocessBuild(BuildReport report)
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("DlibFaceLandmarkDetectorBuildPreprocessor");
            if (guids.Length == 0)
            {
                Debug.LogWarning("SetPluginImportSettings Failed : DlibFaceLandmarkDetectorBuildPreprocessor.cs is missing.");
                return;
            }
            string dlibFaceLandmarkDetectorFolderPath = AssetDatabase.GUIDToAssetPath(guids[0]).Substring(0, AssetDatabase.GUIDToAssetPath(guids[0]).LastIndexOf("/Editor/DlibFaceLandmarkDetectorBuildPreprocessor.cs"));

            string pluginsFolderPath = dlibFaceLandmarkDetectorFolderPath + "/Plugins";
            //Debug.Log("pluginsFolderPath " + pluginsFolderPath);

            Debug.Log("DlibFaceLandmarkDetectorBuildPreprocessor " + report.summary.platform);

            switch (report.summary.platform)
            {
                case BuildTarget.StandaloneOSX:
                    DlibFaceLandmarkDetectorMenuItem.SetOSXPluginImportSettings(pluginsFolderPath);
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:

                    DlibFaceLandmarkDetectorMenuItem.SetWindowsPluginImportSettings(pluginsFolderPath);
                    break;
                case BuildTarget.iOS:
                    bool incrementalBuild = (report.summary.options & BuildOptions.AcceptExternalModificationsToPlayer) == BuildOptions.AcceptExternalModificationsToPlayer;
                    DlibFaceLandmarkDetectorMenuItem.SetIOSPluginImportSettings(pluginsFolderPath, false, incrementalBuild);
                    break;
#if (UNITY_2022_3_OR_NEWER && !(UNITY_2023_1_OR_NEWER) && !(UNITY_2022_3_0 || UNITY_2022_3_1 || UNITY_2022_3_2 || UNITY_2022_3_3 || UNITY_2022_3_4 || UNITY_2022_3_5 || UNITY_2022_3_6 || UNITY_2022_3_7 || UNITY_2022_3_8 || UNITY_2022_3_9 || UNITY_2022_3_10 || UNITY_2022_3_11 || UNITY_2022_3_12 || UNITY_2022_3_13 || UNITY_2022_3_14 || UNITY_2022_3_15 || UNITY_2022_3_16 || UNITY_2022_3_17)) || UNITY_6000_0_OR_NEWER
                case BuildTarget.VisionOS:

                    DlibFaceLandmarkDetectorMenuItem.SetVisionOSPluginImportSettings(pluginsFolderPath);
                    break;
#endif
                case BuildTarget.Android:

                    DlibFaceLandmarkDetectorMenuItem.SetAndroidPluginImportSettings(pluginsFolderPath);
                    break;
                case BuildTarget.StandaloneLinux64:

                    DlibFaceLandmarkDetectorMenuItem.SetLinuxPluginImportSettings(pluginsFolderPath);
                    break;
                case BuildTarget.WebGL:

                    DlibFaceLandmarkDetectorMenuItem.SetWebGLPluginImportSettings(pluginsFolderPath);
                    break;
                case BuildTarget.WSAPlayer:

                    DlibFaceLandmarkDetectorMenuItem.SetUWPPluginImportSettings(pluginsFolderPath);
                    break;
                case BuildTarget.NoTarget:

                    break;
                default:

                    break;
            }
        }
    }
}
#endif
