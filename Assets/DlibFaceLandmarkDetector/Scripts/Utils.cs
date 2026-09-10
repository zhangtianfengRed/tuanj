using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AOT;
using UnityEngine;
using UnityEngine.Networking;

namespace DlibFaceLandmarkDetector.UnityUtils
{
    [Obsolete("This class is deprecated. Use DlibFaceLandmarkDetector.UnityIntegration.DlibEnv or DlibDebug class instead.")]
    public static class Utils
    {
        /// <summary>
        /// Returns this "Dlib FaceLandmark Detector" version number.
        /// </summary>
        /// <returns>
        ///  this "Dlib FaceLandmark Detector" version number
        /// </returns>
        public static string getVersion()
        {
            return "2.0.1";
        }

        #region getFilePath

        /// <summary>
        /// Copies a file from the "StreamingAssets" directory to a readable location.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory. e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the Application.persistentDataPath directory.
        /// [WebGL] If the target file has not yet been copied to WebGL's virtual filesystem, it is necessary to use <see cref="CopyFileCoroutine">CopyFileCoroutine</see>, <see cref="CopyFileAsync">CopyFileAsync</see> or <see cref="CopyFileTaskAsync">CopyFileAsyncTask</see> at first.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="refresh">
        /// [Android] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <returns>
        /// Returns the copied file path in case of success and returns <c>string.Empty</c> in case of error.
        /// </returns>
        private static string CopyFile(string filepath, bool refresh, int timeout)
        {
            string srcPath = Path.Combine(Application.streamingAssetsPath, filepath);

#if UNITY_ANDROID
            string destPath = Path.Combine(Application.persistentDataPath, "dlibfacelandmarkdetector", filepath);
#elif UNITY_WEBGL
            string destPath = Path.Combine(Path.DirectorySeparatorChar.ToString(), "dlibfacelandmarkdetector", filepath);
#else
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);
#endif

            if (!refresh && File.Exists(destPath))
                return destPath;

            using (UnityWebRequest request = UnityWebRequest.Get(srcPath))
            {
                if (timeout > 0)
                    request.timeout = timeout;

                request.SendWebRequest();
                while (!request.isDone) { }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    File.WriteAllBytes(destPath, request.downloadHandler.data);
                    return destPath;
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        /// <summary>
        /// Gets the readable path of a file in the "StreamingAssets" directory.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory. e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] If the target file has not yet been copied to WebGL's virtual filesystem, it is necessary to use <see cref="getFilePathCoroutine">getFilePathCoroutine</see>, <see cref="getFilePathAsync">getFilePathAsync</see> or <see cref="getFilePathTaskAsync">getFilePathAsyncTask</see> at first.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="refresh">
        /// [Android] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <returns>
        /// Returns the readable file path in case of success and returns <c>string.Empty</c> in case of error.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        public static string getFilePath(string filepath, bool refresh = false, int timeout = 0)
        {
            if (filepath == null)
                throw new ArgumentNullException(nameof(filepath));

            filepath = filepath.TrimStart(chTrims);

            if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(Path.GetExtension(filepath)))
                return string.Empty;

#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
#if UNITY_ANDROID
            string destPath = CopyFile(filepath, refresh, timeout);
            return destPath;
#else // UNITY_WEBGL
            string destPath = Path.Combine(Path.DirectorySeparatorChar.ToString(), "dlibfacelandmarkdetector", filepath);
            if (File.Exists(destPath)) {
                return destPath;
            }
            else
            {
                Debug.LogWarning($"The file does not exist: {filepath}. If the target file has not yet been copied to WebGL's virtual filesystem, it is necessary to use getFilePathCoroutine, getFilePathAsync or getFilePathAsyncTask at first.");
                return string.Empty;
            }
#endif
#else // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);
            return File.Exists(destPath) ? destPath : string.Empty;
#endif // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
        }

        /// <summary>
        /// Gets the multiple readable paths of a file in the "StreamingAssets" directory.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory.  e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] If the target file has not yet been copied to WebGL's virtual filesystem, it is necessary to use <see cref="getFilePathCoroutine">getFilePathCoroutine</see>, <see cref="getFilePathAsync">getFilePathAsync</see> or <see cref="getFilePathTaskAsync">getFilePathAsyncTask</see> at first.
        /// </remarks>
        /// <param name="filepaths">
        /// The list of file paths relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="refresh">
        /// [Android] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <returns>
        /// Returns the list of readable file paths in case of success and returns <c>string.Empty</c> in case of error.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        public static IReadOnlyList<string> getMultipleFilePaths(IReadOnlyList<string> filepaths, bool refresh = false, int timeout = 0)
        {
            if (filepaths == null)
                throw new ArgumentNullException(nameof(filepaths));

            var results = new string[filepaths.Count];

            for (int i = 0; i < filepaths.Count; i++)
            {
                results[i] = getFilePath(filepaths[i], refresh, timeout);
            }

            return results;
        }

        #endregion

        #region getFilePathCoroutine

        /// <summary>
        /// Asynchronously copies a file from the "StreamingAssets" directory to a readable location using Coroutine.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory. e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the Application.persistentDataPath directory.
        /// [WebGL] If the target file has not yet been copied to WebGL's virtual filesystem, it is necessary to use this method before accessing the file.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="completed">
        /// A callback that is called when the process is completed. Returns the copied file path in case of success and returns <c>string.Empty</c> in case of error.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when the process is in progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when an error occurs. Returns the file path, an error string, and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <returns>
        /// Returns an IEnumerator object. Yielding the IEnumerator in a coroutine pauses execution until the UnityWebRequest completes or encounters an error.
        /// </returns>
        private static IEnumerator CopyFileCoroutine(string filepath, Action<string> completed, Action<string, float> progressChanged = null, Action<string, string, long> errorOccurred = null, bool refresh = false, int timeout = 0)
        {
            string srcPath = Path.Combine(Application.streamingAssetsPath, filepath);

#if UNITY_ANDROID
            string destPath = Path.Combine(Application.persistentDataPath, "dlibfacelandmarkdetector", filepath);
#elif UNITY_WEBGL
            string destPath = Path.Combine(Path.DirectorySeparatorChar.ToString(), "dlibfacelandmarkdetector", filepath);
#else
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);
#endif

            if (!refresh && File.Exists(destPath))
            {
                progressChanged?.Invoke(filepath, 0);
                yield return null;
                progressChanged?.Invoke(filepath, 1);
                completed?.Invoke(destPath);
                yield break;
            }


            using (UnityWebRequest request = UnityWebRequest.Get(srcPath))
            {
                if (timeout > 0)
                    request.timeout = timeout;

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    progressChanged?.Invoke(filepath, request.downloadProgress);
                    yield return null;
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    progressChanged?.Invoke(filepath, request.downloadProgress);
                    // create directory and write downlorded data
                    string dirPath = Path.GetDirectoryName(destPath);
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);
                    File.WriteAllBytes(destPath, request.downloadHandler.data);
                    completed?.Invoke(destPath);
                    yield break;
                }
                else
                {
                    //Debug.LogError($"UnityWebRequest error occurred: {filepath}, {request.error}, {request.responseCode}");
                    errorOccurred?.Invoke(filepath, request.error, request.responseCode);
                    completed?.Invoke(string.Empty);
                    yield break;
                }
            }
        }

        /// <summary>
        /// Asynchronously retrieves the readable path of a file in the "StreamingAssets" directory using Coroutine.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory.  e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] The target file in the "StreamingAssets" directory is copied to the WebGL's virtual filesystem.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="completed">
        /// A callback that is called when the process is completed. Returns a readable file path in case of success and returns <c>string.Empty</c> in case of error.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when the process is the progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when the process is error occurred. Returns the file path and an error string and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android][WebGL] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android][WebGL] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <returns>
        /// Returns an IEnumerator object. Yielding the IEnumerator in a coroutine pauses the coroutine until the UnityWebRequest completes or encounters a system error.
        /// <strong>Note:</strong> that if the IEnumerator is externally stoped, the UnityWebRequest's Abort method will not be called, meaning the download will continue in the background.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        public static IEnumerator getFilePathCoroutine(
            string filepath,
            Action<string> completed,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0)
        {
            if (filepath == null)
                throw new ArgumentNullException(nameof(filepath));

            filepath = filepath.TrimStart(chTrims);

            if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(Path.GetExtension(filepath)))
            {
                progressChanged?.Invoke(filepath, 0);
                yield return null;
                progressChanged?.Invoke(filepath, 1);
                errorOccurred?.Invoke(filepath, "Invalid file path.", -1);
                completed?.Invoke(string.Empty);

                yield break;
            }

#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            yield return CopyFileCoroutine(filepath, completed, progressChanged, errorOccurred, refresh, timeout);
#else // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);

            progressChanged?.Invoke(filepath, 0);
            yield return null;
            progressChanged?.Invoke(filepath, 1);

            if (File.Exists(destPath))
            {
                completed?.Invoke(destPath);
            }
            else
            {
                errorOccurred?.Invoke(filepath, "File does not exist.", -1);
                completed?.Invoke(string.Empty);
            }
            yield break;
#endif // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR

        }


        /// <summary>
        /// Asynchronously retrieves the multiple readable paths of files in the "StreamingAssets" directory using Coroutine.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory.  e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] The target file in the "StreamingAssets" directory is copied to the WebGL's virtual filesystem.
        /// </remarks>
        /// <param name="filepaths">
        /// The list of file paths relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="allCompleted">
        /// A callback that is called when all processes are completed. Returns a list of file paths. Returns a readable file path in case of success and returns <c>string.Empty</c> in case of error.
        /// </param>
        /// <param name="completed">
        /// An optional callback that is called when one process is completed. Returns a readable file path in case of success and returns empty in case of error.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when one process is the progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when one process is error occurred. Returns the file path and an error string and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android][WebGL] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android][WebGL] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <returns>
        /// Returns an IEnumerator object. Yielding the IEnumerator in a coroutine pauses the coroutine until the UnityWebRequest completes or encounters a system error.
        /// <strong>Note:</strong> that if the IEnumerator is externally stoped, the UnityWebRequest's Abort method will not be called, meaning the download will continue in the background.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        public static IEnumerator getMultipleFilePathsCoroutine(
            IReadOnlyList<string> filepaths,
            Action<IReadOnlyList<string>> allCompleted,
            Action<string> completed = null,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0)
        {
            if (filepaths == null)
                throw new ArgumentNullException(nameof(filepaths));

            string[] readableFilePaths = new string[filepaths.Count];

            for (int i = 0; i < filepaths.Count; i++)
            {
                yield return getFilePathCoroutine(filepaths[i],
                (path) =>
                {
                    readableFilePaths[i] = path;
                    completed?.Invoke(path);
                },
                progressChanged,
                errorOccurred,
                refresh, timeout);
            }

            allCompleted?.Invoke(readableFilePaths);
        }

        #endregion

        #region getFilePathAsync

#if UNITY_2023_1_OR_NEWER

        /// <summary>
        /// Asynchronously copies a file from the "StreamingAssets" directory to a readable location using Awaitable.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory. e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the Application.persistentDataPath directory.
        /// [WebGL] If the target file has not yet been copied to WebGL's virtual filesystem, it is necessary to use this method before accessing the file.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when the process is in progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when an error occurs. Returns the file path, an error string, and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the download operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous copy operation. The result is the copied file path if successful, or <c>string.Empty</c> if the operation fails.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the copy operation is canceled.
        /// </exception>
        private static async Awaitable<string> CopyFileAsync(string filepath, Action<string, float> progressChanged, Action<string, string, long> errorOccurred, bool refresh, int timeout, CancellationToken cancellationToken)
        {
            string srcPath = Path.Combine(Application.streamingAssetsPath, filepath);

#if UNITY_ANDROID
            string destPath = Path.Combine(Application.persistentDataPath, "dlibfacelandmarkdetector", filepath);
#elif UNITY_WEBGL
            string destPath = Path.Combine(Path.DirectorySeparatorChar.ToString(), "dlibfacelandmarkdetector", filepath);
#else
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);
#endif

            if (!refresh && File.Exists(destPath))
            {
                progressChanged?.Invoke(filepath, 0);
                await Awaitable.NextFrameAsync();
                progressChanged?.Invoke(filepath, 1);
                return destPath;
            }


            using (UnityWebRequest request = UnityWebRequest.Get(srcPath))
            {
                if (timeout > 0)
                    request.timeout = timeout;

                var operation = request.SendWebRequest();

                try
                {

                    while (!operation.isDone)
                    {
                        progressChanged?.Invoke(filepath, request.downloadProgress);
                        await Awaitable.NextFrameAsync();

                        if (cancellationToken.IsCancellationRequested)
                        {
                            request.Abort();
                            cancellationToken.ThrowIfCancellationRequested();
                        }
                    }

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                        File.WriteAllBytes(destPath, request.downloadHandler.data);
                        return destPath;
                    }
                    else
                    {
                        errorOccurred?.Invoke(filepath, request.error, request.responseCode);
                        return string.Empty;
                    }

                }
                catch (OperationCanceledException)
                {
                    //Debug.Log($"Download canceled: {filepath}");
                    throw;
                }
                catch (Exception ex)
                {
                    //Debug.LogError($"An error occurred: {filepath}, {ex.Message}");
                    errorOccurred?.Invoke(filepath, ex.Message, -1);
                    request.Abort();
                    return string.Empty;
                }
            }
        }

        private static async Task<T> AsTask<T>(this Awaitable<T> awaitable)
        {
            return await awaitable;
        }

        /// <summary>
        /// Asynchronously retrieves the readable path of a file in the "StreamingAssets" directory using Awaitable.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory.  e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] The target file in the "StreamingAssets" directory is copied to the WebGL's virtual filesystem.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when the process is the progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when the process is error occurred. Returns the file path and an error string and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android][WebGL] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android][WebGL] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the download operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous download operation. The result is a readable file path where the downloaded file was saved, or <c>string.Empty</c> if the download fails.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this method is called from a non-main thread.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the download operation is canceled.
        /// </exception>
        public static async Awaitable<string> getFilePathAsync(
            string filepath,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            if (filepath == null)
                throw new ArgumentNullException(nameof(filepath));

            if (SynchronizationContext.Current == null)
                throw new InvalidOperationException("This method must be called from the main thread.");

            cancellationToken.ThrowIfCancellationRequested();

            filepath = filepath.TrimStart(chTrims);

            if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(Path.GetExtension(filepath)))
            {
                progressChanged?.Invoke(filepath, 0);
                await Awaitable.NextFrameAsync();

                cancellationToken.ThrowIfCancellationRequested();

                progressChanged?.Invoke(filepath, 1);
                errorOccurred?.Invoke(filepath, "Invalid file path.", -1);

                return string.Empty;
            }

#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            string destPath = await CopyFileAsync(filepath, progressChanged, errorOccurred, refresh, timeout, cancellationToken);
            return destPath;
#else // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);

            progressChanged?.Invoke(filepath, 0);
            await Awaitable.NextFrameAsync();

            cancellationToken.ThrowIfCancellationRequested();

            progressChanged?.Invoke(filepath, 1);

            if (File.Exists(destPath))
            {
                return destPath;
            }
            else
            {
                errorOccurred?.Invoke(filepath, "File does not exist.", -1);
                return string.Empty;
            }
#endif // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
        }

        /// <summary>
        /// Asynchronously retrieves the multiple readable paths of files in the "StreamingAssets" directory using Awaitable.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory.  e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] The target file in the "StreamingAssets" directory is copied to the WebGL's virtual filesystem.
        /// </remarks>
        /// <param name="filepaths">
        /// The list of file paths relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="completed">
        /// An optional callback that is called when one process is completed. Returns a readable file path in case of success and returns empty in case of error.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when one process is the progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when one process is error occurred. Returns the file path and an error string and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android][WebGL] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android][WebGL] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the download operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous download operation. The result is a list of readable file paths where the downloaded file was saved, or <c>string.Empty</c> if the download fails.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this method is called from a non-main thread.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the download operation is canceled.
        /// </exception>
        public static async Awaitable<IReadOnlyList<string>> getMultipleFilePathsAsync(
            IReadOnlyList<string> filepaths,
            Action<string> completed = null,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            if (filepaths == null)
                throw new ArgumentNullException(nameof(filepaths));

            if (SynchronizationContext.Current == null)
                throw new InvalidOperationException("This method must be called from the main thread.");

            var downloadTasks = new List<Task<string>>(filepaths.Count);
            var results = new string[filepaths.Count];

            for (int i = 0; i < filepaths.Count; i++)
            {
                int index = i;

                var task = getFilePathAsync(filepaths[index], progressChanged, errorOccurred, refresh, timeout, cancellationToken).AsTask().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        var result = t.Result;
                        completed?.Invoke(result);
                        return result;
                    }
                    return string.Empty;

                }, cancellationToken, TaskContinuationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());


                downloadTasks.Add(task);
            }

            results = await Task.WhenAll(downloadTasks);

            return results;
        }

#endif

        #endregion

        #region getFilePathTaskAsync

        /// <summary>
        /// Asynchronously copies a file from the "StreamingAssets" directory to a readable location using Task.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory. e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the Application.persistentDataPath directory.
        /// [WebGL] If the target file has not yet been copied to WebGL's virtual filesystem, it is necessary to use this method before accessing the file.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when the process is in progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when an error occurs. Returns the file path, an error string, and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the copy operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous copy operation. The result is the copied file path if successful, or <c>string.Empty</c> if the operation fails.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the copy operation is canceled.
        /// </exception>
        private static async Task<string> CopyFileTaskAsync(string filepath, Action<string, float> progressChanged, Action<string, string, long> errorOccurred, bool refresh, int timeout, CancellationToken cancellationToken)
        {

            string srcPath = Path.Combine(Application.streamingAssetsPath, filepath);

#if UNITY_ANDROID
            string destPath = Path.Combine(Application.persistentDataPath, "dlibfacelandmarkdetector", filepath);
#elif UNITY_WEBGL
            string destPath = Path.Combine(Path.DirectorySeparatorChar.ToString(), "dlibfacelandmarkdetector", filepath);
#else
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);
#endif

            if (!refresh && File.Exists(destPath))
            {
                progressChanged?.Invoke(filepath, 0);
                await Task.Yield();
                progressChanged?.Invoke(filepath, 1);
                return destPath;
            }


            using (UnityWebRequest request = UnityWebRequest.Get(srcPath))
            {
                if (timeout > 0)
                    request.timeout = timeout;

                var operation = request.SendWebRequest();

                try
                {

                    while (!operation.isDone)
                    {
                        progressChanged?.Invoke(filepath, request.downloadProgress);
                        await Task.Yield();

                        if (cancellationToken.IsCancellationRequested)
                        {
                            request.Abort();
                            cancellationToken.ThrowIfCancellationRequested();
                        }
                    }

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                        File.WriteAllBytes(destPath, request.downloadHandler.data);
                        return destPath;
                    }
                    else
                    {
                        errorOccurred?.Invoke(filepath, request.error, request.responseCode);
                        request.Abort();
                        return string.Empty;
                    }

                }
                catch (OperationCanceledException)
                {
                    //Debug.Log($"Download canceled: {filepath}");
                    throw;
                }
                catch (Exception ex)
                {
                    //Debug.LogError($"An error occurred: {filepath}, {ex.Message}");
                    errorOccurred?.Invoke(filepath, ex.Message, -1);
                    return string.Empty;
                }
            }
        }


        /// <summary>
        /// @deprecated Use Utils.getFilePathTaskAsync
        /// </summary>
        [Obsolete("This method is deprecated. Use getFilePathTaskAsync method instead.")]
        public static async Task<string> getFilePathAsyncTask(
            string filepath,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            return await getFilePathTaskAsync(filepath, progressChanged, errorOccurred, refresh, timeout, cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves the readable path of a file in the "StreamingAssets" directory using Task.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory.  e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] The target file in the "StreamingAssets" directory is copied to the WebGL's virtual filesystem.
        /// </remarks>
        /// <param name="filepath">
        /// A file path relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when the process is the progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when the process is error occurred. Returns the file path and an error string and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android][WebGL] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android][WebGL] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the download operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous download operation. The result is a readable file path where the downloaded file was saved, or <c>string.Empty</c> if the download fails.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this method is called from a non-main thread.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the download operation is canceled.
        /// </exception>
        public static async Task<string> getFilePathTaskAsync(
            string filepath,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            if (filepath == null)
                throw new ArgumentNullException(nameof(filepath));

            if (SynchronizationContext.Current == null)
                throw new InvalidOperationException("This method must be called from the main thread.");

            cancellationToken.ThrowIfCancellationRequested();

            filepath = filepath.TrimStart(chTrims);

            if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(Path.GetExtension(filepath)))
            {
                progressChanged?.Invoke(filepath, 0);
                await Task.Yield();

                cancellationToken.ThrowIfCancellationRequested();

                progressChanged?.Invoke(filepath, 1);
                errorOccurred?.Invoke(filepath, "Invalid file path.", -1);

                return string.Empty;
            }

#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            string destPath = await CopyFileTaskAsync(filepath, progressChanged, errorOccurred, refresh, timeout, cancellationToken);
            return destPath;
#else // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            string destPath = Path.Combine(Application.streamingAssetsPath, filepath);

            progressChanged?.Invoke(filepath, 0);
            await Task.Yield();

            cancellationToken.ThrowIfCancellationRequested();

            progressChanged?.Invoke(filepath, 1);

            if (File.Exists(destPath))
            {
                return destPath;
            }
            else
            {
                errorOccurred?.Invoke(filepath, "File does not exist.", -1);
                return string.Empty;
            }
#endif // (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
        }


        /// <summary>
        /// @deprecated Use Utils.getMultipleFilePathsTaskAsync
        /// </summary>
        [Obsolete("This method is deprecated. Use getMultipleFilePathsTaskAsync method instead.")]
        public static async Task<IReadOnlyList<string>> getMultipleFilePathsAsyncTask(
            IReadOnlyList<string> filepaths,
            Action<string> completed = null,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            return await getMultipleFilePathsTaskAsync(filepaths, completed, progressChanged, errorOccurred, refresh, timeout, cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves the multiple readable paths of files in the "StreamingAssets" directory using Task.
        /// </summary>
        /// <remarks>
        /// Provide a relative file path based on the "StreamingAssets" directory.  e.g., "foobar.txt" or "hogehoge/foobar.txt".
        /// [Android] The target file that exists in the "StreamingAssets" directory is copied to the the Application.persistentDataPath directory.
        /// [WebGL] The target file in the "StreamingAssets" directory is copied to the WebGL's virtual filesystem.
        /// </remarks>
        /// <param name="filepaths">
        /// The list of file paths relative to the "StreamingAssets" directory.
        /// </param>
        /// <param name="completed">
        /// An optional callback that is called when one process is completed. Returns a readable file path in case of success and returns empty in case of error.
        /// </param>
        /// <param name="progressChanged">
        /// An optional callback that is called when one process is the progress. Returns the file path and a progress value (0.0 to 1.0).
        /// </param>
        /// <param name="errorOccurred">
        /// An optional callback that is called when one process is error occurred. Returns the file path and an error string and an error response code.
        /// </param>
        /// <param name="refresh">
        /// [Android][WebGL] If false, the file is not copied if it already exists. If true, the file is always copied.
        /// </param>
        /// <param name="timeout">
        /// [Android][WebGL] Sets the UnityWebRequest to abort after the specified number of seconds. If set to 0, no timeout is applied. The default is 0.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the download operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous download operation. The result is a list of readable file paths where the downloaded file was saved, or <c>string.Empty</c> if the download fails.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this method is called from a non-main thread.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the download operation is canceled.
        /// </exception>
        public static async Task<IReadOnlyList<string>> getMultipleFilePathsTaskAsync(
            IReadOnlyList<string> filepaths,
            Action<string> completed = null,
            Action<string, float> progressChanged = null,
            Action<string, string, long> errorOccurred = null,
            bool refresh = false,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            if (filepaths == null)
                throw new ArgumentNullException(nameof(filepaths));

            if (SynchronizationContext.Current == null)
                throw new InvalidOperationException("This method must be called from the main thread.");

            var downloadTasks = new List<Task<string>>(filepaths.Count);
            var results = new string[filepaths.Count];

            for (int i = 0; i < filepaths.Count; i++)
            {
                int index = i;

                var task = getFilePathTaskAsync(
                    filepaths[index],
                    progressChanged,
                    errorOccurred,
                    refresh,
                    timeout,
                    cancellationToken
                ).ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        var result = t.Result;
                        completed?.Invoke(result);
                        return result;
                    }
                    return string.Empty;

                }, cancellationToken, TaskContinuationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());

                downloadTasks.Add(task);
            }

            results = await Task.WhenAll(downloadTasks);

            return results;
        }

        #endregion

        private static char[] chTrims = {
            '.',
#if UNITY_WINRT_8_1 && !UNITY_EDITOR
            '/',
            '\\'
#else
            System.IO.Path.DirectorySeparatorChar,
            System.IO.Path.AltDirectorySeparatorChar
#endif
        };

#pragma warning disable 0414
        /// <summary>
        /// The current debug mode state.
        /// </summary>
        private static bool dlibDebugMode = false;

        /// <summary>
        /// If true, DlibException is thrown instead of calling Debug.LogError (msg).
        /// </summary>
        private static bool throwDlibException = false;

        /// <summary>
        /// Callback called when an Dlib error occurs on the Native side.
        /// </summary>
        private static Action<string> dlibSetDebugModeCallback;
#pragma warning restore 0414

        /// <summary>
        /// Gets the current debug mode state.
        /// </summary>
        /// <returns>
        /// True if debug mode is enabled, false otherwise.
        /// </returns>
        public static bool isDebugMode()
        {
            return dlibDebugMode;
        }

        /// <summary>
        /// Gets the current exception throwing state.
        /// </summary>
        /// <returns>
        /// True if exceptions are thrown instead of logging errors, false otherwise.
        /// </returns>
        public static bool isThrowException()
        {
            return throwDlibException;
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
        ///     Utils.setDebugMode(true, false);
        ///
        ///     // Code that causes an error.
        ///
        ///     Utils.setDebugMode(false);
        ///
        ///
        ///     // Throw DlibException.
        ///     Utils.setDebugMode(true, true);
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
        ///     Utils.setDebugMode(false);
        /// }
        /// </code>
        /// </example>
        /// <param name="debugMode">
        /// If true, The error log of the Native side Dlib will be displayed on the Unity Editor Console.
        /// </param>
        /// <param name="throwException">
        /// If true, DlibException is thrown instead of calling Debug.LogError (msg).
        /// </param>
        public static void setDebugMode(bool debugMode, bool throwException = false)
        {
            setDebugModeInternal(debugMode, throwException, dlibSetDebugModeCallback);
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
        ///     Utils.setDebugMode(true, false);
        ///
        ///     // Code that causes an error.
        ///
        ///     Utils.setDebugMode(false);
        ///
        ///
        ///     // Throw DlibException.
        ///     Utils.setDebugMode(true, true);
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
        ///     Utils.setDebugMode(true, true, (str) =>
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
        ///     Utils.setDebugMode(false);
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
        public static void setDebugMode(bool debugMode, bool throwException, Action<string> callback)
        {
            setDebugModeInternal(debugMode, throwException, callback);
        }

        /// <summary>
        /// Internal implementation of setDebugMode that handles the common functionality.
        /// </summary>
        private static void setDebugModeInternal(bool debugMode, bool throwException, Action<string> callback)
        {
            DlibFaceLandmarkDetector_SetDebugMode(debugMode);

            if (debugMode)
            {
                DlibFaceLandmarkDetector_SetDebugLogFunc(debugLogFunc);
                //DlibFaceLandmarkDetector_DebugLogTest ();

                throwDlibException = throwException;
            }
            else
            {
                DlibFaceLandmarkDetector_SetDebugLogFunc(null);

                throwDlibException = false;
            }

            dlibDebugMode = debugMode;
            dlibSetDebugModeCallback = callback;
        }

        private delegate void DebugLogDelegate(string str);

        [MonoPInvokeCallback(typeof(DebugLogDelegate))]
        private static void debugLogFunc(string str)
        {
            if (dlibSetDebugModeCallback != null) dlibSetDebugModeCallback.Invoke(str);

            if (throwDlibException)
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
