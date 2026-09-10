#if UNITY_EDITOR
using UnityEditor;

namespace DlibFaceLandmarkDetector.Editor
{
    [InitializeOnLoad]
    public class DlibFaceLandmarkDetectorSetupToolsWindowStartup
    {
        // Private Methods
        static DlibFaceLandmarkDetectorSetupToolsWindowStartup()
        {
            EditorApplication.update -= ShowSetupToolsWindow;
            EditorApplication.update += ShowSetupToolsWindow;

            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;
        }

        private static void ShowSetupToolsWindow()
        {
            //Debug.Log("DlibFaceLandmarkDetectorProjectSettings.Instance.ShowSetupToolsWindowFlag: " + DlibFaceLandmarkDetectorProjectSettings.Instance.ShowSetupToolsWindowFlag);

            var showAtStartup = DlibFaceLandmarkDetectorProjectSettings.Instance.ShowSetupToolsWindowFlag;

            if (showAtStartup)
            {
                DlibFaceLandmarkDetectorSetupToolsWindow.OpenSetupToolsWindow();

                DlibFaceLandmarkDetectorProjectSettings.Instance.ShowSetupToolsWindowFlag = false;
                EditorUtility.SetDirty(DlibFaceLandmarkDetectorProjectSettings.Instance);

                DlibFaceLandmarkDetectorMenuItem.SetPluginImportSettings();
            }

            EditorApplication.update -= ShowSetupToolsWindow;
        }

        private static void PlayModeChanged(PlayModeStateChange playMode)
        {
            EditorApplication.update -= ShowSetupToolsWindow;
        }
    }
}
#endif
