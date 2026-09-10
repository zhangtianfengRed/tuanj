#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;

namespace DlibFaceLandmarkDetector.Editor
{
    [InitializeOnLoad]
    public static class DlibFaceLandmarkDetectorCheckHasOpenCV
    {
        // Constants
        private const string SYMBOL_HAS_OPENCVFORUNITY = "HAS_OPENCVFORUNITY";

        // Private Methods
        static DlibFaceLandmarkDetectorCheckHasOpenCV()
        {
            CheckAndApplyDefines();
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            CheckAndApplyDefines();
        }

        private static void CheckAndApplyDefines()
        {
            // Try to find a known OpenCVForUnity class
            var openCVType = Type.GetType("OpenCVForUnity.CoreModule.Core, EnoxSoftware.OpenCVForUnity");
            bool isInstalled = openCVType != null;

            //UnityEngine.Debug.Log("CheckAndApplyDefines: isInstalled: " + isInstalled);

            ApplySymbol(NamedBuildTarget.Standalone, isInstalled);
            ApplySymbol(NamedBuildTarget.Android, isInstalled);
            ApplySymbol(NamedBuildTarget.iOS, isInstalled);

#if (UNITY_2022_3_OR_NEWER && !(UNITY_2023_1_OR_NEWER) && !(UNITY_2022_3_0 || UNITY_2022_3_1 || UNITY_2022_3_2 || UNITY_2022_3_3 || UNITY_2022_3_4 || UNITY_2022_3_5 || UNITY_2022_3_6 || UNITY_2022_3_7 || UNITY_2022_3_8 || UNITY_2022_3_9 || UNITY_2022_3_10 || UNITY_2022_3_11 || UNITY_2022_3_12 || UNITY_2022_3_13 || UNITY_2022_3_14 || UNITY_2022_3_15 || UNITY_2022_3_16 || UNITY_2022_3_17)) || UNITY_6000_0_OR_NEWER
            ApplySymbol(NamedBuildTarget.VisionOS, isInstalled);
#endif

            ApplySymbol(NamedBuildTarget.WebGL, isInstalled);
            ApplySymbol(NamedBuildTarget.WindowsStoreApps, isInstalled);
        }

        private static void ApplySymbol(NamedBuildTarget target, bool shouldAdd)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbols(target).Split(';', StringSplitOptions.RemoveEmptyEntries);
            var defines = new System.Collections.Generic.List<string>(symbols);

            if (shouldAdd && !defines.Contains(SYMBOL_HAS_OPENCVFORUNITY))
            {
                defines.Add(SYMBOL_HAS_OPENCVFORUNITY);
            }
            else if (!shouldAdd && defines.Contains(SYMBOL_HAS_OPENCVFORUNITY))
            {
                defines.Remove(SYMBOL_HAS_OPENCVFORUNITY);
            }

            PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
        }
    }
}
#endif
