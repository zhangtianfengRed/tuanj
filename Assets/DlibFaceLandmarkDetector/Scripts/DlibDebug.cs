using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DlibFaceLandmarkDetector.UnityIntegration
{
    /// <summary>
    /// Dlib Debug utilities.
    /// </summary>
    public static class DlibDebug
    {

#pragma warning disable 0414
        /// <summary>
        /// If true, The error log of the Native side Dlib will be displayed on the Unity Editor Console.
        /// </summary>
        private static bool _dlibDebugMode = false;

        /// <summary>
        /// If true, DlibException is thrown instead of calling Debug.LogError (msg).
        /// </summary>
        private static bool _throwDlibException = false;

        /// <summary>
        /// Callback called when an Dlib error occurs on the Native side.
        /// </summary>
        private static Action<string> _dlibSetDebugModeCallback;
#pragma warning restore 0414

        /// <summary>
        /// Gets whether debug mode is enabled.
        /// </summary>
        /// <returns>True if debug mode is enabled, false otherwise.</returns>
        public static bool IsDebugMode()
        {
            return _dlibDebugMode;
        }

        /// <summary>
        /// Gets whether Dlib exceptions are thrown.
        /// </summary>
        /// <returns>True if exceptions are thrown, false otherwise.</returns>
        public static bool IsThrowException()
        {
            return _throwDlibException;
        }

        /// <summary>
        /// Sets the debug mode.
        /// </summary>
        /// <remarks>
        /// If debugMode is true, The error log of the Native side Dlib will be displayed on the Unity Editor Console.However, if throwException is true, DlibException is thrown instead of calling Debug.LogError (msg).
        /// </remarks>
        /// <example>
        /// Please use as follows.
        /// <code>
        /// {
        ///     // DlibException handling
        ///     // Publish DlibException to Debug.LogError.
        ///     DlibDebug.SetDebugMode(true, false);
        ///
        ///     // Code that causes an error.
        ///
        ///     DlibDebug.SetDebugMode(false);
        ///
        ///
        ///     // Throw DlibException.
        ///     DlibDebug.SetDebugMode(true, true);
        ///
        ///     try
        ///     {
        ///         // Code that causes an error.
        ///     }
        ///     catch (Exception e)
        ///     {
        ///         Debug.Log("DlibException: " + e);
        ///     }
        ///
        ///     DlibDebug.SetDebugMode(false);
        /// }
        /// </code>
        /// </example>
        /// <param name="debugMode">
        /// If true, The error log of the Native side Dlib will be displayed on the Unity Editor Console.
        /// </param>
        /// <param name="throwException">
        /// If true, DlibException is thrown instead of calling Debug.LogError (msg).
        /// </param>
        public static void SetDebugMode(bool debugMode, bool throwException = false)
        {
            SetDebugModeInternal(debugMode, throwException, _dlibSetDebugModeCallback);
        }

        /// <summary>
        /// Sets the debug mode with a callback.
        /// </summary>
        /// <remarks>
        /// If debugMode is true, The error log of the Native side Dlib will be displayed on the Unity Editor Console.However, if throwException is true, DlibException is thrown instead of calling Debug.LogError (msg).
        /// The callback is maintained even when debug mode is turned off, allowing it to be reused when debug mode is turned on again.
        /// To clear the callback, call this method with null as the callback parameter.
        /// </remarks>
        /// <example>
        /// Please use as follows.
        /// <code>
        /// {
        ///     // DlibException handling
        ///     // Publish DlibException to Debug.LogError.
        ///     DlibDebug.SetDebugMode(true, false);
        ///
        ///     // Code that causes an error.
        ///
        ///     DlibDebug.SetDebugMode(false);
        ///
        ///
        ///     // Throw DlibException.
        ///     DlibDebug.SetDebugMode(true, true);
        ///
        ///     try
        ///     {
        ///         // Code that causes an error.
        ///     }
        ///     catch (Exception e)
        ///     {
        ///         Debug.Log("DlibException: " + e);
        ///     }
        ///
        ///     // Callback string of DlibException.
        ///     DlibDebug.SetDebugMode(true, true, (str) =>
        ///     {
        ///         Debug.Log("DlibException: " + str);
        ///     });
        ///
        ///     try
        ///     {
        ///         // Code that causes an error.
        ///     }
        ///     catch (Exception e)
        ///     {
        ///         Debug.Log("DlibException: " + e);
        ///     }
        ///
        ///     DlibDebug.SetDebugMode(false);
        /// }
        /// </code>
        /// </example>
        /// <param name="debugMode">
        /// If true, The error log of the Native side Dlib will be displayed on the Unity Editor Console.
        /// </param>
        /// <param name="throwException">
        /// If true, DlibException is thrown instead of calling Debug.LogError (msg).
        /// </param>
        /// <param name="callback">
        /// Callback called when an Dlib error occurs on the Native side.
        /// The callback is maintained even when debug mode is turned off.
        /// To clear the callback, pass null as this parameter.
        /// </param>
        public static void SetDebugMode(bool debugMode, bool throwException, Action<string> callback)
        {
            SetDebugModeInternal(debugMode, throwException, callback);
        }

        /// <summary>
        /// Internal implementation of SetDebugMode that handles the common functionality.
        /// </summary>
        private static void SetDebugModeInternal(bool debugMode, bool throwException, Action<string> callback)
        {
            DlibFaceLandmarkDetector_SetDebugMode(debugMode);

            if (debugMode)
            {
                DlibFaceLandmarkDetector_SetDebugLogFunc(debugLogFunc);
                //DlibFaceLandmarkDetector_DebugLogTest ();

                _throwDlibException = throwException;
            }
            else
            {
                DlibFaceLandmarkDetector_SetDebugLogFunc(null);

                _throwDlibException = false;
            }

            _dlibDebugMode = debugMode;
            _dlibSetDebugModeCallback = callback;
        }

        private delegate void DebugLogDelegate(string str);

        [MonoPInvokeCallback(typeof(DebugLogDelegate))]
        private static void debugLogFunc(string str)
        {
            if (_dlibSetDebugModeCallback != null) _dlibSetDebugModeCallback.Invoke(str);

            if (_throwDlibException)
            {
                throw new DlibException(str);
            }
            else
            {
                Debug.LogError(str);
            }
        }

#if (UNITY_IOS || UNITY_VISIONOS || UNITY_WEBGL) && !UNITY_EDITOR
        const string LIBNAME = "__Internal";
#else
        const string LIBNAME = "dlibfacelandmarkdetector";
#endif

        [DllImport(LIBNAME)]
        private static extern void DlibFaceLandmarkDetector_SetDebugMode([MarshalAs(UnmanagedType.U1)] bool flag);

        [DllImport(LIBNAME)]
        private static extern void DlibFaceLandmarkDetector_SetDebugLogFunc(DebugLogDelegate func);

        [DllImport(LIBNAME)]
        private static extern void DlibFaceLandmarkDetector_DebugLogTest();
    }
}