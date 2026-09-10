#if UNITY_EDITOR
using DlibFaceLandmarkDetector.UnityIntegration;
using UnityEditor;
using UnityEngine;

namespace DlibFaceLandmarkDetector.Editor
{
    public class DlibFaceLandmarkDetectorResetDebugMode : MonoBehaviour
    {
        // Private Methods
        [InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            DlibDebug.SetDebugMode(false);
        }
    }
}
#endif
