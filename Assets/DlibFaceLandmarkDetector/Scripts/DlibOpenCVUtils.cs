#if HAS_OPENCVFORUNITY

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityIntegration;
using UnityEngine;

namespace DlibFaceLandmarkDetector.UnityIntegration
{
    /// <summary>
    /// Utility class for the integration of DlibFaceLandmarkDetector and OpenCVForUnity.
    /// </summary>
    public static class DlibOpenCVUtils
    {
        /// <summary>
        /// White color constant for text rendering.
        /// </summary>
        private static readonly Scalar WHITE_COLOR = new Scalar(255, 255, 255, 255);

        /// <summary>
        /// White color constant for text rendering (Vec4d version).
        /// </summary>
        private static readonly Vec4d WHITE_COLOR_VEC4D = new Vec4d(255, 255, 255, 255);

        /// <summary>
        /// White color constant for text rendering (ValueTuple version).
        /// </summary>
        private static readonly (double v0, double v1, double v2, double v3) WHITE_COLOR_TUPLE = (255, 255, 255, 255);

        /// <summary>
        /// Sets the image for the specified <see cref="FaceLandmarkDetector"/> using a given <see cref="Mat"/> object.
        /// </summary>
        /// <param name="faceLandmarkDetector">
        /// An instance of <see cref="FaceLandmarkDetector"/> that processes the specified image.
        /// </param>
        /// <param name="imgMat">
        /// A <see cref="Mat"/> object representing the image to set. The matrix must be continuous and valid for processing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="faceLandmarkDetector"/> or <paramref name="imgMat"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="imgMat"/> is not continuous. Ensure <c>imgMat.isContinuous() == true</c>.
        /// </exception>
        /// <remarks>
        /// This method directly assigns the <paramref name="imgMat"/> data pointer, width, height, and element size to the
        /// specified <see cref="FaceLandmarkDetector"/>. It avoids additional memory allocations by reusing the existing <paramref name="imgMat"/> data.
        /// </remarks>
        public static void SetImage(FaceLandmarkDetector faceLandmarkDetector, Mat imgMat)
        {
            if (faceLandmarkDetector == null)
                throw new ArgumentNullException(nameof(faceLandmarkDetector));
            if (faceLandmarkDetector != null)
                faceLandmarkDetector.ThrowIfDisposed();

            if (imgMat == null)
                throw new ArgumentNullException(nameof(imgMat));
            if (imgMat != null)
                imgMat.ThrowIfDisposed();
            if (!imgMat.isContinuous())
                throw new ArgumentException("imgMat.isContinuous() must be true.");

            faceLandmarkDetector.SetImage((IntPtr)imgMat.dataAddr(), imgMat.width(), imgMat.height(), (int)imgMat.elemSize());
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="UnityEngine.Rect"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="Scalar"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, UnityEngine.Rect rect, Scalar color, int thickness)
        {
            Imgproc.rectangle(imgMat, new Point(rect.xMin, rect.yMin), new Point(rect.xMax, rect.yMax), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="UnityEngine.Rect"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="Vec4d"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, UnityEngine.Rect rect, in Vec4d color, int thickness)
        {
            Imgproc.rectangle(imgMat, new Vec2d(rect.xMin, rect.yMin), new Vec2d(rect.xMax, rect.yMax), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="UnityEngine.Rect"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, UnityEngine.Rect rect, in (double v0, double v1, double v2, double v3) color, int thickness)
        {
            Imgproc.rectangle(imgMat, (rect.xMin, rect.yMin), (rect.xMax, rect.yMax), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="OpenCVForUnity.CoreModule.Rect"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="Scalar"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, OpenCVForUnity.CoreModule.Rect rect, Scalar color, int thickness)
        {
            Imgproc.rectangle(imgMat, rect, color, thickness);
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="Vec4i"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="Vec4d"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, in Vec4i rect, in Vec4d color, int thickness)
        {
            Imgproc.rectangle(imgMat, rect, color, thickness);
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="Vec4d"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="Vec4d"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, in Vec4d rect, in Vec4d color, int thickness)
        {
            Imgproc.rectangle(imgMat, rect.ToVec4i(), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="ValueTuple{Int32, Int32, Int32, Int32}"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, in (int x, int y, int width, int height) rect, in (double v0, double v1, double v2, double v3) color, int thickness)
        {
            Imgproc.rectangle(imgMat, rect, color, thickness);
        }

        /// <summary>
        /// Draws a rectangle on the specified image to indicate a detected face region.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle.</param>
        /// <param name="rect">The <see cref="ValueTuple{Double, Double, Double, Double}"/> defining the area to highlight.</param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        public static void DrawFaceRect(Mat imgMat, in (double x, double y, double width, double height) rect, in (double v0, double v1, double v2, double v3) color, int thickness)
        {
            Imgproc.rectangle(imgMat, ((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the given <see cref="RectDetection"/> data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">The <see cref="DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection"/> containing the rectangle and detection details.</param>
        /// <param name="color">The <see cref="Scalar"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection rect, Scalar color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));

            UnityEngine.Rect _rect = rect.rect;
            Imgproc.putText(imgMat, "detection_confidence : " + rect.detection_confidence, new Point(_rect.xMin, _rect.yMin - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR, 1, Imgproc.LINE_AA, false);
            Imgproc.putText(imgMat, "weight_index : " + rect.weight_index, new Point(_rect.xMin, _rect.yMin - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, new Point(_rect.xMin, _rect.yMin), new Point(_rect.xMax, _rect.yMax), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the given <see cref="DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection"/> data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">The <see cref="DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection"/> containing the rectangle and detection details.</param>
        /// <param name="color">The <see cref="Vec4d"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection rect, in Vec4d color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));

            UnityEngine.Rect _rect = rect.rect;
            Imgproc.putText(imgMat, "detection_confidence : " + rect.detection_confidence, new Vec2d(_rect.xMin, _rect.yMin - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
            Imgproc.putText(imgMat, "weight_index : " + rect.weight_index, new Vec2d(_rect.xMin, _rect.yMin - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, new Vec2d(_rect.xMin, _rect.yMin), new Vec2d(_rect.xMax, _rect.yMax), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the given <see cref="DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection"/> data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">The <see cref="DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection"/> containing the rectangle and detection details.</param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, DlibFaceLandmarkDetector.FaceLandmarkDetector.RectDetection rect, in (double v0, double v1, double v2, double v3) color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));

            UnityEngine.Rect _rect = rect.rect;
            Imgproc.putText(imgMat, "detection_confidence : " + rect.detection_confidence, (_rect.xMin, _rect.yMin - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
            Imgproc.putText(imgMat, "weight_index : " + rect.weight_index, (_rect.xMin, _rect.yMin - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, (_rect.xMin, _rect.yMin), (_rect.xMax, _rect.yMax), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the provided rectangle data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">
        /// An array containing the rectangle data in the format <c>[x, y, width, height, detection_confidence, weight_index]</c>.
        /// The last two values are optional and used for displaying additional detection information.
        /// </param>
        /// <param name="color">The <see cref="Scalar"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, double[] rect, Scalar color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));
            if (rect.Length < 4)
                throw new ArgumentException("rect must be at least 4 elements long", nameof(rect));

            if (rect.Length > 4)
                Imgproc.putText(imgMat, "detection_confidence : " + rect[4], new Point(rect[0], rect[1] - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR, 1, Imgproc.LINE_AA, false);
            if (rect.Length > 5)
                Imgproc.putText(imgMat, "weight_index : " + rect[5], new Point(rect[0], rect[1] - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, new Point(rect[0], rect[1]), new Point(rect[0] + rect[2], rect[1] + rect[3]), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the provided rectangle data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">
        /// An array containing the rectangle data in the format <c>[x, y, width, height, detection_confidence, weight_index]</c>.
        /// The last two values are optional and used for displaying additional detection information.
        /// </param>
        /// <param name="color">The <see cref="Vec4d"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, double[] rect, in Vec4d color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));
            if (rect.Length < 4)
                throw new ArgumentException("rect must be at least 4 elements long", nameof(rect));

            if (rect.Length > 4)
                Imgproc.putText(imgMat, "detection_confidence : " + rect[4], new Vec2d(rect[0], rect[1] - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
            if (rect.Length > 5)
                Imgproc.putText(imgMat, "weight_index : " + rect[5], new Vec2d(rect[0], rect[1] - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, new Vec2d(rect[0], rect[1]), new Vec2d(rect[0] + rect[2], rect[1] + rect[3]), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the provided rectangle data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">
        /// An array containing the rectangle data in the format <c>[x, y, width, height, detection_confidence, weight_index]</c>.
        /// The last two values are optional and used for displaying additional detection information.
        /// </param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, double[] rect, in (double v0, double v1, double v2, double v3) color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));
            if (rect.Length < 4)
                throw new ArgumentException("rect must be at least 4 elements long", nameof(rect));

            if (rect.Length > 4)
                Imgproc.putText(imgMat, "detection_confidence : " + rect[4], (rect[0], rect[1] - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
            if (rect.Length > 5)
                Imgproc.putText(imgMat, "weight_index : " + rect[5], (rect[0], rect[1] - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, (rect[0], rect[1]), (rect[0] + rect[2], rect[1] + rect[3]), color, thickness);
        }

#if NET_STANDARD_2_1

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the provided rectangle data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">
        /// An array containing the rectangle data in the format <c>[x, y, width, height, detection_confidence, weight_index]</c>.
        /// The last two values are optional and used for displaying additional detection information.
        /// </param>
        /// <param name="color">The <see cref="Scalar"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, ReadOnlySpan<double> rect, Scalar color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));
            if (rect.Length < 4)
                throw new ArgumentException("rect must be at least 4 elements long", nameof(rect));

            if (rect.Length > 4)
                Imgproc.putText(imgMat, "detection_confidence : " + rect[4], new Point(rect[0], rect[1] - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR, 1, Imgproc.LINE_AA, false);
            if (rect.Length > 5)
                Imgproc.putText(imgMat, "weight_index : " + rect[5], new Point(rect[0], rect[1] - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, new Point(rect[0], rect[1]), new Point(rect[0] + rect[2], rect[1] + rect[3]), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the provided rectangle data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">
        /// An array containing the rectangle data in the format <c>[x, y, width, height, detection_confidence, weight_index]</c>.
        /// The last two values are optional and used for displaying additional detection information.
        /// </param>
        /// <param name="color">The <see cref="Vec4d"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, ReadOnlySpan<double> rect, in Vec4d color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));
            if (rect.Length < 4)
                throw new ArgumentException("rect must be at least 4 elements long", nameof(rect));

            if (rect.Length > 4)
                Imgproc.putText(imgMat, "detection_confidence : " + rect[4], new Vec2d(rect[0], rect[1] - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
            if (rect.Length > 5)
                Imgproc.putText(imgMat, "weight_index : " + rect[5], new Vec2d(rect[0], rect[1] - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, new Vec2d(rect[0], rect[1]), new Vec2d(rect[0] + rect[2], rect[1] + rect[3]), color, thickness);
        }

        /// <summary>
        /// Draws a rectangle and detection information on the specified image based on the provided rectangle data.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the rectangle and text.</param>
        /// <param name="rect">
        /// An array containing the rectangle data in the format <c>[x, y, width, height, detection_confidence, weight_index]</c>.
        /// The last two values are optional and used for displaying additional detection information.
        /// </param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color of the rectangle border.</param>
        /// <param name="thickness">The thickness of the rectangle border.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rect"/> is null.</exception>
        public static void DrawFaceRect(Mat imgMat, ReadOnlySpan<double> rect, in (double v0, double v1, double v2, double v3) color, int thickness)
        {
            if (rect == null)
                throw new ArgumentNullException(nameof(rect));
            if (rect.Length < 4)
                throw new ArgumentException("rect must be at least 4 elements long", nameof(rect));

            if (rect.Length > 4)
                Imgproc.putText(imgMat, "detection_confidence : " + rect[4], (rect[0], rect[1] - 20), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
            if (rect.Length > 5)
                Imgproc.putText(imgMat, "weight_index : " + rect[5], (rect[0], rect[1] - 5), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
            Imgproc.rectangle(imgMat, (rect[0], rect[1]), (rect[0] + rect[2], rect[1] + rect[3]), color, thickness);
        }

#endif

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A list of <see cref="Vector2"/> points representing the landmark positions. The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Scalar"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, IReadOnlyList<Vector2> points, Scalar color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            var colorTuple = color.ToValueTuple();

            if (points.Count == 5)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];

                Imgproc.line(imgMat, (p0.x, p0.y), (p1.x, p1.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p1.x, p1.y), (p4.x, p4.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p3.x, p3.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p3.x, p3.y), (p2.x, p2.y), colorTuple, thickness);
            }
            else if (points.Count == 6)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];

                Imgproc.line(imgMat, (p2.x, p2.y), (p3.x, p3.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p5.x, p5.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p3.x, p3.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p0.x, p0.y), (p1.x, p1.y), colorTuple, thickness);
            }
            else if (points.Count == 17)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];
                var p9 = points[9];
                var p10 = points[10];
                var p11 = points[11];
                var p12 = points[12];
                var p13 = points[13];
                var p16 = points[16];

                Imgproc.line(imgMat, (p2.x, p2.y), (p9.x, p9.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p9.x, p9.y), (p3.x, p3.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p3.x, p3.y), (p10.x, p10.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p10.x, p10.y), (p2.x, p2.y), colorTuple, thickness);

                Imgproc.line(imgMat, (p4.x, p4.y), (p11.x, p11.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p11.x, p11.y), (p5.x, p5.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p5.x, p5.y), (p12.x, p12.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p12.x, p12.y), (p4.x, p4.y), colorTuple, thickness);

                Imgproc.line(imgMat, (p3.x, p3.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p0.x, p0.y), (p1.x, p1.y), colorTuple, thickness);

                for (int i = 14; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                Imgproc.line(imgMat, (p16.x, p16.y), (p13.x, p13.y), colorTuple, thickness);

                for (int i = 6; i <= 8; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, (point.x, point.y), 2, colorTuple, -1);
                }
            }
            else if (points.Count == 68)
            {
                for (int i = 1; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }

                for (int i = 28; i <= 30; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }

                for (int i = 18; i <= 21; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                for (int i = 23; i <= 26; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                for (int i = 31; i <= 35; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p30 = points[30];
                var p35 = points[35];
                Imgproc.line(imgMat, (p30.x, p30.y), (p35.x, p35.y), colorTuple, thickness);

                for (int i = 37; i <= 41; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p36 = points[36];
                var p41 = points[41];
                Imgproc.line(imgMat, (p36.x, p36.y), (p41.x, p41.y), colorTuple, thickness);

                for (int i = 43; i <= 47; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p42 = points[42];
                var p47 = points[47];
                Imgproc.line(imgMat, (p42.x, p42.y), (p47.x, p47.y), colorTuple, thickness);

                for (int i = 49; i <= 59; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p48 = points[48];
                var p59 = points[59];
                Imgproc.line(imgMat, (p48.x, p48.y), (p59.x, p59.y), colorTuple, thickness);

                for (int i = 61; i <= 67; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p60 = points[60];
                var p67 = points[67];
                Imgproc.line(imgMat, (p60.x, p60.y), (p67.x, p67.y), colorTuple, thickness);
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, (point.x, point.y), 2, colorTuple, -1);
                }
            }

            // Draw the index number of facelandmark points.
            if (drawIndexNumbers)
            {
                for (int i = 0; i < points.Count; ++i)
                {
                    var point = points[i];
                    Imgproc.putText(imgMat, i.ToString(), (point.x, point.y), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
                }
            }
        }

#if NET_STANDARD_2_1

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A list of <see cref="Vector2"/> points representing the landmark positions. The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Scalar"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, ReadOnlySpan<Vector2> points, Scalar color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            var colorTuple = color.ToValueTuple();

            if (points.Length == 5)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];

                Imgproc.line(imgMat, (p0.x, p0.y), (p1.x, p1.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p1.x, p1.y), (p4.x, p4.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p3.x, p3.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p3.x, p3.y), (p2.x, p2.y), colorTuple, thickness);
            }
            else if (points.Length == 6)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];

                Imgproc.line(imgMat, (p2.x, p2.y), (p3.x, p3.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p5.x, p5.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p3.x, p3.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p0.x, p0.y), (p1.x, p1.y), colorTuple, thickness);
            }
            else if (points.Length == 17)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];
                var p9 = points[9];
                var p10 = points[10];
                var p11 = points[11];
                var p12 = points[12];
                var p13 = points[13];
                var p16 = points[16];

                Imgproc.line(imgMat, (p2.x, p2.y), (p9.x, p9.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p9.x, p9.y), (p3.x, p3.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p3.x, p3.y), (p10.x, p10.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p10.x, p10.y), (p2.x, p2.y), colorTuple, thickness);

                Imgproc.line(imgMat, (p4.x, p4.y), (p11.x, p11.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p11.x, p11.y), (p5.x, p5.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p5.x, p5.y), (p12.x, p12.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p12.x, p12.y), (p4.x, p4.y), colorTuple, thickness);

                Imgproc.line(imgMat, (p3.x, p3.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p4.x, p4.y), (p0.x, p0.y), colorTuple, thickness);
                Imgproc.line(imgMat, (p0.x, p0.y), (p1.x, p1.y), colorTuple, thickness);

                for (int i = 14; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                Imgproc.line(imgMat, (p16.x, p16.y), (p13.x, p13.y), colorTuple, thickness);

                for (int i = 6; i <= 8; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, (point.x, point.y), 2, colorTuple, -1);
                }
            }
            else if (points.Length == 68)
            {
                for (int i = 1; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }

                for (int i = 28; i <= 30; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }

                for (int i = 18; i <= 21; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                for (int i = 23; i <= 26; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                for (int i = 31; i <= 35; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p30 = points[30];
                var p35 = points[35];
                Imgproc.line(imgMat, (p30.x, p30.y), (p35.x, p35.y), colorTuple, thickness);

                for (int i = 37; i <= 41; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p36 = points[36];
                var p41 = points[41];
                Imgproc.line(imgMat, (p36.x, p36.y), (p41.x, p41.y), colorTuple, thickness);

                for (int i = 43; i <= 47; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p42 = points[42];
                var p47 = points[47];
                Imgproc.line(imgMat, (p42.x, p42.y), (p47.x, p47.y), colorTuple, thickness);

                for (int i = 49; i <= 59; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p48 = points[48];
                var p59 = points[59];
                Imgproc.line(imgMat, (p48.x, p48.y), (p59.x, p59.y), colorTuple, thickness);

                for (int i = 61; i <= 67; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, (current.x, current.y), (previous.x, previous.y), colorTuple, thickness);
                }
                var p60 = points[60];
                var p67 = points[67];
                Imgproc.line(imgMat, (p60.x, p60.y), (p67.x, p67.y), colorTuple, thickness);
            }
            else
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, (point.x, point.y), 2, colorTuple, -1);
                }
            }

            // Draw the index number of facelandmark points.
            if (drawIndexNumbers)
            {
                for (int i = 0; i < points.Length; ++i)
                {
                    var point = points[i];
                    Imgproc.putText(imgMat, i.ToString(), (point.x, point.y), Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
                }
            }
        }

#endif

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A list of <see cref="Vec2d"/> points representing the landmark positions. The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Vec4d"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, IReadOnlyList<Vec2d> points, in Vec4d color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            if (points.Count == 5)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];

                Imgproc.line(imgMat, p0, p1, color, thickness);
                Imgproc.line(imgMat, p1, p4, color, thickness);
                Imgproc.line(imgMat, p4, p3, color, thickness);
                Imgproc.line(imgMat, p3, p2, color, thickness);
            }
            else if (points.Count == 6)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];

                Imgproc.line(imgMat, p2, p3, color, thickness);
                Imgproc.line(imgMat, p4, p5, color, thickness);
                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);
            }
            else if (points.Count == 17)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];
                var p9 = points[9];
                var p10 = points[10];
                var p11 = points[11];
                var p12 = points[12];
                var p13 = points[13];
                var p16 = points[16];

                Imgproc.line(imgMat, p2, p9, color, thickness);
                Imgproc.line(imgMat, p9, p3, color, thickness);
                Imgproc.line(imgMat, p3, p10, color, thickness);
                Imgproc.line(imgMat, p10, p2, color, thickness);

                Imgproc.line(imgMat, p4, p11, color, thickness);
                Imgproc.line(imgMat, p11, p5, color, thickness);
                Imgproc.line(imgMat, p5, p12, color, thickness);
                Imgproc.line(imgMat, p12, p4, color, thickness);

                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);

                for (int i = 14; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                Imgproc.line(imgMat, p16, p13, color, thickness);

                for (int i = 6; i <= 8; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }
            else if (points.Count == 68)
            {
                for (int i = 1; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 28; i <= 30; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 18; i <= 21; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 23; i <= 26; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 31; i <= 35; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p30 = points[30];
                var p35 = points[35];
                Imgproc.line(imgMat, p30, p35, color, thickness);

                for (int i = 37; i <= 41; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p36 = points[36];
                var p41 = points[41];
                Imgproc.line(imgMat, p36, p41, color, thickness);

                for (int i = 43; i <= 47; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p42 = points[42];
                var p47 = points[47];
                Imgproc.line(imgMat, p42, p47, color, thickness);

                for (int i = 49; i <= 59; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p48 = points[48];
                var p59 = points[59];
                Imgproc.line(imgMat, p48, p59, color, thickness);

                for (int i = 61; i <= 67; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p60 = points[60];
                var p67 = points[67];
                Imgproc.line(imgMat, p60, p67, color, thickness);
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }

            // Draw the index number of facelandmark points.
            if (drawIndexNumbers)
            {
                for (int i = 0; i < points.Count; ++i)
                {
                    var point = points[i];
                    Imgproc.putText(imgMat, i.ToString(), point, Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
                }
            }
        }

#if NET_STANDARD_2_1

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A list of <see cref="Vec2d"/> points representing the landmark positions. The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Vec4d"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, ReadOnlySpan<Vec2d> points, in Vec4d color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            if (points.Length == 5)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];

                Imgproc.line(imgMat, p0, p1, color, thickness);
                Imgproc.line(imgMat, p1, p4, color, thickness);
                Imgproc.line(imgMat, p4, p3, color, thickness);
                Imgproc.line(imgMat, p3, p2, color, thickness);
            }
            else if (points.Length == 6)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];

                Imgproc.line(imgMat, p2, p3, color, thickness);
                Imgproc.line(imgMat, p4, p5, color, thickness);
                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);
            }
            else if (points.Length == 17)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];
                var p9 = points[9];
                var p10 = points[10];
                var p11 = points[11];
                var p12 = points[12];
                var p13 = points[13];
                var p16 = points[16];

                Imgproc.line(imgMat, p2, p9, color, thickness);
                Imgproc.line(imgMat, p9, p3, color, thickness);
                Imgproc.line(imgMat, p3, p10, color, thickness);
                Imgproc.line(imgMat, p10, p2, color, thickness);

                Imgproc.line(imgMat, p4, p11, color, thickness);
                Imgproc.line(imgMat, p11, p5, color, thickness);
                Imgproc.line(imgMat, p5, p12, color, thickness);
                Imgproc.line(imgMat, p12, p4, color, thickness);

                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);

                for (int i = 14; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                Imgproc.line(imgMat, p16, p13, color, thickness);

                for (int i = 6; i <= 8; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }
            else if (points.Length == 68)
            {
                for (int i = 1; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 28; i <= 30; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 18; i <= 21; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 23; i <= 26; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 31; i <= 35; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p30 = points[30];
                var p35 = points[35];
                Imgproc.line(imgMat, p30, p35, color, thickness);

                for (int i = 37; i <= 41; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p36 = points[36];
                var p41 = points[41];
                Imgproc.line(imgMat, p36, p41, color, thickness);

                for (int i = 43; i <= 47; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p42 = points[42];
                var p47 = points[47];
                Imgproc.line(imgMat, p42, p47, color, thickness);

                for (int i = 49; i <= 59; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p48 = points[48];
                var p59 = points[59];
                Imgproc.line(imgMat, p48, p59, color, thickness);

                for (int i = 61; i <= 67; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p60 = points[60];
                var p67 = points[67];
                Imgproc.line(imgMat, p60, p67, color, thickness);
            }
            else
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }

            // Draw the index number of facelandmark points.
            if (drawIndexNumbers)
            {
                for (int i = 0; i < points.Length; ++i)
                {
                    var point = points[i];
                    Imgproc.putText(imgMat, i.ToString(), point, Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_VEC4D, 1, Imgproc.LINE_AA, false);
                }
            }
        }

#endif

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A list of <see cref="ValueTuple{Double, Double}"/> points representing the landmark positions. The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, IReadOnlyList<(double x, double y)> points, in (double v0, double v1, double v2, double v3) color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            if (points.Count == 5)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];

                Imgproc.line(imgMat, p0, p1, color, thickness);
                Imgproc.line(imgMat, p1, p4, color, thickness);
                Imgproc.line(imgMat, p4, p3, color, thickness);
                Imgproc.line(imgMat, p3, p2, color, thickness);
            }
            else if (points.Count == 6)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];

                Imgproc.line(imgMat, p2, p3, color, thickness);
                Imgproc.line(imgMat, p4, p5, color, thickness);
                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);
            }
            else if (points.Count == 17)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];
                var p9 = points[9];
                var p10 = points[10];
                var p11 = points[11];
                var p12 = points[12];
                var p13 = points[13];
                var p16 = points[16];

                Imgproc.line(imgMat, p2, p9, color, thickness);
                Imgproc.line(imgMat, p9, p3, color, thickness);
                Imgproc.line(imgMat, p3, p10, color, thickness);
                Imgproc.line(imgMat, p10, p2, color, thickness);

                Imgproc.line(imgMat, p4, p11, color, thickness);
                Imgproc.line(imgMat, p11, p5, color, thickness);
                Imgproc.line(imgMat, p5, p12, color, thickness);
                Imgproc.line(imgMat, p12, p4, color, thickness);

                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);

                for (int i = 14; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                Imgproc.line(imgMat, p16, p13, color, thickness);

                for (int i = 6; i <= 8; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }
            else if (points.Count == 68)
            {
                for (int i = 1; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 28; i <= 30; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 18; i <= 21; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 23; i <= 26; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 31; i <= 35; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p30 = points[30];
                var p35 = points[35];
                Imgproc.line(imgMat, p30, p35, color, thickness);

                for (int i = 37; i <= 41; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p36 = points[36];
                var p41 = points[41];
                Imgproc.line(imgMat, p36, p41, color, thickness);

                for (int i = 43; i <= 47; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p42 = points[42];
                var p47 = points[47];
                Imgproc.line(imgMat, p42, p47, color, thickness);

                for (int i = 49; i <= 59; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p48 = points[48];
                var p59 = points[59];
                Imgproc.line(imgMat, p48, p59, color, thickness);

                for (int i = 61; i <= 67; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p60 = points[60];
                var p67 = points[67];
                Imgproc.line(imgMat, p60, p67, color, thickness);
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }

            // Draw the index number of facelandmark points.
            if (drawIndexNumbers)
            {
                for (int i = 0; i < points.Count; ++i)
                {
                    var point = points[i];
                    Imgproc.putText(imgMat, i.ToString(), point, Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR_TUPLE, 1, Imgproc.LINE_AA, false);
                }
            }
        }

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A list of <see cref="Point"/> points representing the landmark positions. The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Scalar"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, IReadOnlyList<Point> points, Scalar color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            if (points.Count == 5)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];

                Imgproc.line(imgMat, p0, p1, color, thickness);
                Imgproc.line(imgMat, p1, p4, color, thickness);
                Imgproc.line(imgMat, p4, p3, color, thickness);
                Imgproc.line(imgMat, p3, p2, color, thickness);
            }
            else if (points.Count == 6)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];

                Imgproc.line(imgMat, p2, p3, color, thickness);
                Imgproc.line(imgMat, p4, p5, color, thickness);
                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);
            }
            else if (points.Count == 17)
            {
                var p0 = points[0];
                var p1 = points[1];
                var p2 = points[2];
                var p3 = points[3];
                var p4 = points[4];
                var p5 = points[5];
                var p9 = points[9];
                var p10 = points[10];
                var p11 = points[11];
                var p12 = points[12];
                var p13 = points[13];
                var p16 = points[16];

                Imgproc.line(imgMat, p2, p9, color, thickness);
                Imgproc.line(imgMat, p9, p3, color, thickness);
                Imgproc.line(imgMat, p3, p10, color, thickness);
                Imgproc.line(imgMat, p10, p2, color, thickness);

                Imgproc.line(imgMat, p4, p11, color, thickness);
                Imgproc.line(imgMat, p11, p5, color, thickness);
                Imgproc.line(imgMat, p5, p12, color, thickness);
                Imgproc.line(imgMat, p12, p4, color, thickness);

                Imgproc.line(imgMat, p3, p0, color, thickness);
                Imgproc.line(imgMat, p4, p0, color, thickness);
                Imgproc.line(imgMat, p0, p1, color, thickness);

                for (int i = 14; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                Imgproc.line(imgMat, p16, p13, color, thickness);

                for (int i = 6; i <= 8; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }
            else if (points.Count == 68)
            {
                for (int i = 1; i <= 16; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 28; i <= 30; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }

                for (int i = 18; i <= 21; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 23; i <= 26; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                for (int i = 31; i <= 35; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p30 = points[30];
                var p35 = points[35];
                Imgproc.line(imgMat, p30, p35, color, thickness);

                for (int i = 37; i <= 41; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p36 = points[36];
                var p41 = points[41];
                Imgproc.line(imgMat, p36, p41, color, thickness);

                for (int i = 43; i <= 47; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p42 = points[42];
                var p47 = points[47];
                Imgproc.line(imgMat, p42, p47, color, thickness);

                for (int i = 49; i <= 59; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p48 = points[48];
                var p59 = points[59];
                Imgproc.line(imgMat, p48, p59, color, thickness);

                for (int i = 61; i <= 67; ++i)
                {
                    var current = points[i];
                    var previous = points[i - 1];
                    Imgproc.line(imgMat, current, previous, color, thickness);
                }
                var p60 = points[60];
                var p67 = points[67];
                Imgproc.line(imgMat, p60, p67, color, thickness);
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    Imgproc.circle(imgMat, point, 2, color, -1);
                }
            }

            // Draw the index number of facelandmark points.
            if (drawIndexNumbers)
            {
                for (int i = 0; i < points.Count; ++i)
                {
                    var point = points[i];
                    Imgproc.putText(imgMat, i.ToString(), point, Imgproc.FONT_HERSHEY_SIMPLEX, 0.5, WHITE_COLOR, 1, Imgproc.LINE_AA, false);
                }
            }
        }

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A <see cref="double"/> array representing the landmark positions, where each pair of consecutive values represents the X and Y coordinates of a point.
        /// The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Scalar"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, double[] points, Scalar color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length % 2 != 0)
                throw new InvalidOperationException("The points array must contain an even number of values");

#if NET_STANDARD_2_1
            ReadOnlySpan<Vec2d> pointsVec2dSpan = MemoryMarshal.Cast<double, Vec2d>(points);
            DrawFaceLandmark(imgMat, pointsVec2dSpan, color.ToVec4d(), thickness, drawIndexNumbers);
#else
            Vec2d[] pointsVec2d = new Vec2d[points.Length / 2];
            for (int i = 0; i < points.Length / 2; i++)
            {
                pointsVec2d[i] = new Vec2d(points[i * 2], points[i * 2 + 1]);
            }
            DrawFaceLandmark(imgMat, pointsVec2d, color.ToVec4d(), thickness, drawIndexNumbers);
#endif
        }

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A <see cref="double"/> array representing the landmark positions, where each pair of consecutive values represents the X and Y coordinates of a point.
        /// The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Vec4d"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, double[] points, in Vec4d color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length % 2 != 0)
                throw new InvalidOperationException("The points array must contain an even number of values");

#if NET_STANDARD_2_1
            ReadOnlySpan<Vec2d> pointsVec2dSpan = MemoryMarshal.Cast<double, Vec2d>(points);
            DrawFaceLandmark(imgMat, pointsVec2dSpan, color, thickness, drawIndexNumbers);
#else
            Vec2d[] pointsVec2d = new Vec2d[points.Length / 2];
            for (int i = 0; i < points.Length / 2; i++)
            {
                pointsVec2d[i] = new Vec2d(points[i * 2], points[i * 2 + 1]);
            }
            DrawFaceLandmark(imgMat, pointsVec2d, color, thickness, drawIndexNumbers);
#endif
        }

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A <see cref="double"/> array representing the landmark positions, where each pair of consecutive values represents the X and Y coordinates of a point.
        /// The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, double[] points, in (double v0, double v1, double v2, double v3) color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length % 2 != 0)
                throw new InvalidOperationException("The points array must contain an even number of values");

#if NET_STANDARD_2_1
            ReadOnlySpan<Vec2d> pointsVec2dSpan = MemoryMarshal.Cast<double, Vec2d>(points);
            DrawFaceLandmark(imgMat, pointsVec2dSpan, new Vec4d(color.v0, color.v1, color.v2, color.v3), thickness, drawIndexNumbers);
#else
            Vec2d[] pointsVec2d = new Vec2d[points.Length / 2];
            for (int i = 0; i < points.Length / 2; i++)
            {
                pointsVec2d[i] = new Vec2d(points[i * 2], points[i * 2 + 1]);
            }
            DrawFaceLandmark(imgMat, pointsVec2d, new Vec4d(color.v0, color.v1, color.v2, color.v3), thickness, drawIndexNumbers);
#endif
        }

#if NET_STANDARD_2_1

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A <see cref="double"/> array representing the landmark positions, where each pair of consecutive values represents the X and Y coordinates of a point.
        /// The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Scalar"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, ReadOnlySpan<double> points, Scalar color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length % 2 != 0)
                throw new InvalidOperationException("The points array must contain an even number of values");

            ReadOnlySpan<Vec2d> pointsVec2dSpan = MemoryMarshal.Cast<double, Vec2d>(points);
            DrawFaceLandmark(imgMat, pointsVec2dSpan, color.ToVec4d(), thickness, drawIndexNumbers);
        }

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A <see cref="double"/> array representing the landmark positions, where each pair of consecutive values represents the X and Y coordinates of a point.
        /// The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="Vec4d"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, ReadOnlySpan<double> points, in Vec4d color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length % 2 != 0)
                throw new InvalidOperationException("The points array must contain an even number of values");

            ReadOnlySpan<Vec2d> pointsVec2dSpan = MemoryMarshal.Cast<double, Vec2d>(points);
            DrawFaceLandmark(imgMat, pointsVec2dSpan, color, thickness, drawIndexNumbers);
        }

        /// <summary>
        /// Draws a face landmark on the specified image.
        /// This method supports drawing landmarks for 68, 17, 6, or 5 landmark points.
        /// The landmarks are drawn by connecting the points with lines, and optionally, index numbers can be drawn on each point.
        /// </summary>
        /// <param name="imgMat">The <see cref="Mat"/> image on which to draw the face landmarks.</param>
        /// <param name="points">
        /// A <see cref="double"/> array representing the landmark positions, where each pair of consecutive values represents the X and Y coordinates of a point.
        /// The number of points must match one of the following:
        /// 5 points for a basic face shape (e.g., eyes, nose, mouth),
        /// 6 points for a face with more detailed landmarks,
        /// 17 points for a detailed face shape, or
        /// 68 points for a full set of face landmarks.
        /// </param>
        /// <param name="color">The <see cref="ValueTuple{Double, Double, Double, Double}"/> color used to draw the landmarks and lines.</param>
        /// <param name="thickness">The thickness of the lines used to connect the landmarks.</param>
        /// <param name="drawIndexNumbers">
        /// If set to <c>true</c>, index numbers will be drawn next to each landmark point.
        /// If set to <c>false</c>, no index numbers will be drawn. Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="points"/> is <c>null</c>.</exception>
        public static void DrawFaceLandmark(Mat imgMat, ReadOnlySpan<double> points, in (double v0, double v1, double v2, double v3) color, int thickness, bool drawIndexNumbers = false)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length % 2 != 0)
                throw new InvalidOperationException("The points array must contain an even number of values");

            ReadOnlySpan<Vec2d> pointsVec2dSpan = MemoryMarshal.Cast<double, Vec2d>(points);
            DrawFaceLandmark(imgMat, pointsVec2dSpan, new Vec4d(color.v0, color.v1, color.v2, color.v3), thickness, drawIndexNumbers);
        }

#endif

        #region ConvertVector2

        /// <summary>
        /// Convert Vector2 list to Vector2 array.
        /// </summary>
        /// <param name="src">List of Vector2.</param>
        /// <param name="dst">Array of Vector2.</param>
        /// <returns>Array of Vector2.</returns>
        public static Vector2[] ConvertVector2ListToVector2Array(List<Vector2> src, Vector2[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vector2[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref Vector2 dstRef = ref dst[i];
                dstRef.x = src[i].x;
                dstRef.y = src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert Vector2 array to Vector2 list.
        /// </summary>
        /// <param name="src">Array of Vector2.</param>
        /// <param name="dst">List of Vector2.</param>
        /// <returns>List of Vector2.</returns>
        public static List<Vector2> ConvertVector2ArrayToVector2List(Vector2[] src, List<Vector2> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Vector2>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Length; i++)
            {
                dst.Add(src[i]);
            }

            return dst;
        }

        /// <summary>
        /// Convert Vector2 list to Point list.
        /// </summary>
        /// <param name="src">List of Vector2.</param>
        /// <param name="dst">List of Point.</param>
        /// <returns>List of Point.</returns>
        public static List<Point> ConvertVector2ListToPointList(IReadOnlyList<Vector2> src, List<Point> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Point>();
            }

            if (dst.Count != src.Count)
            {
                dst.Clear();
                for (int i = 0; i < src.Count; i++)
                {
                    dst.Add(new Point());
                }
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i].x = src[i].x;
                dst[i].y = src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert Vector2 list to Point array.
        /// </summary>
        /// <param name="src">List of Vector2.</param>
        /// <param name="dst">Array of Point.</param>
        /// <returns>Array of Point.</returns>
        public static Point[] ConvertVector2ListToPointArray(IReadOnlyList<Vector2> src, Point[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Point[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i] = new Point(src[i].x, src[i].y);
            }

            return dst;
        }

        /// <summary>
        /// Convert Vector2 list to double array.
        /// </summary>
        /// <param name="src">List of Vector2.</param>
        /// <param name="dst">Array of double.</param>
        /// <returns>Array of double.</returns>
        public static double[] ConvertVector2ListToArray(IReadOnlyList<Vector2> src, double[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count * 2 != dst.Length)
                throw new ArgumentException("src.Count * 2 != dst.Length");

            if (dst == null)
            {
                dst = new double[src.Count * 2];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i * 2] = src[i].x;
                dst[i * 2 + 1] = src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert Vector2 list to Vec2d array.
        /// </summary>
        /// <param name="src">List of Vector2.</param>
        /// <param name="dst">Array of Vec2d.</param>
        /// <returns>Array of Vec2d.</returns>
        public static Vec2d[] ConvertVector2ListToVec2dArray(IReadOnlyList<Vector2> src, Vec2d[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vec2d[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref Vec2d dstRef = ref dst[i];
                dstRef.Item1 = src[i].x;
                dstRef.Item2 = src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert Vector2 list to ValueTuple list.
        /// </summary>
        /// <param name="src">List of Vector2.</param>
        /// <param name="dst">List of ValueTuple.</param>
        /// <returns>List of ValueTuple.</returns>
        public static List<(double x, double y)> ConvertVector2ListToValueTupleList(IReadOnlyList<Vector2> src, List<(double x, double y)> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<(double x, double y)>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst.Add((src[i].x, src[i].y));
            }

            return dst;
        }

        /// <summary>
        /// Convert Vector2 list to ValueTuple array.
        /// </summary>
        /// <param name="src">List of Vector2.</param>
        /// <param name="dst">Array of ValueTuple.</param>
        /// <returns>Array of ValueTuple.</returns>
        public static (double x, double y)[] ConvertVector2ListToValueTupleArray(IReadOnlyList<Vector2> src, (double x, double y)[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new (double x, double y)[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref (double x, double y) dstRef = ref dst[i];
                dstRef.x = src[i].x;
                dstRef.y = src[i].y;
            }

            return dst;
        }

        #endregion

        #region ConvertPoint

        /// <summary>
        /// Convert Point list to Point array.
        /// </summary>
        /// <param name="src">List of Point.</param>
        /// <param name="dst">Array of Point.</param>
        /// <returns>Array of Point.</returns>
        public static Point[] ConvertPointListToPointArray(List<Point> src, Point[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Point[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i] = new Point(src[i].x, src[i].y);
            }

            return dst;
        }

        /// <summary>
        /// Convert Point array to Point list.
        /// </summary>
        /// <param name="src">Array of Point.</param>
        /// <param name="dst">List of Point.</param>
        /// <returns>List of Point.</returns>
        public static List<Point> ConvertPointArrayToPointList(Point[] src, List<Point> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Point>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Length; ++i)
            {
                dst.Add(new Point(src[i].x, src[i].y));
            }

            return dst;
        }

        /// <summary>
        /// Convert Point list to Vector2 list.
        /// </summary>
        /// <param name="src">List of Point.</param>
        /// <param name="dst">List of Vector2.</param>
        /// <returns>List of Vector2.</returns>
        public static List<Vector2> ConvertPointListToVector2List(IReadOnlyList<Point> src, List<Vector2> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Vector2>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst.Add(new Vector2((float)src[i].x, (float)src[i].y));
            }

            return dst;
        }

        /// <summary>
        /// Convert Point list to Vector2 array.
        /// </summary>
        /// <param name="src">List of Point.</param>
        /// <param name="dst">Array of Vector2.</param>
        /// <returns>Array of Vector2.</returns>
        public static Vector2[] ConvertPointListToVector2Array(IReadOnlyList<Point> src, Vector2[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vector2[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref Vector2 dstRef = ref dst[i];
                dstRef.x = (float)src[i].x;
                dstRef.y = (float)src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert Point list to double array.
        /// </summary>
        /// <param name="src">List of Point.</param>
        /// <param name="dst">Array of double.</param>
        /// <returns>Array of double.</returns>
        public static double[] ConvertPointListToArray(IReadOnlyList<Point> src, double[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count * 2 != dst.Length)
                throw new ArgumentException("src.Count * 2 != dst.Length");

            if (dst == null)
            {
                dst = new double[src.Count * 2];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i * 2] = src[i].x;
                dst[i * 2 + 1] = src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert Point list to Vec2d array.
        /// </summary>
        /// <param name="src">List of Point.</param>
        /// <param name="dst">Array of Vec2d.</param>
        /// <returns>Array of Vec2d.</returns>
        public static Vec2d[] ConvertPointListToVec2dArray(IReadOnlyList<Point> src, Vec2d[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vec2d[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref Vec2d dstRef = ref dst[i];
                dstRef.Item1 = src[i].x;
                dstRef.Item2 = src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert Point list to ValueTuple list.
        /// </summary>
        /// <param name="src">List of Point.</param>
        /// <param name="dst">List of ValueTuple.</param>
        /// <returns>List of ValueTuple.</returns>
        public static List<(double x, double y)> ConvertPointListToValueTupleList(IReadOnlyList<Point> src, List<(double x, double y)> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<(double x, double y)>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst.Add((src[i].x, src[i].y));
            }

            return dst;
        }

        /// <summary>
        /// Convert Point list to ValueTuple array.
        /// </summary>
        /// <param name="src">List of Point.</param>
        /// <param name="dst">Array of ValueTuple.</param>
        /// <returns>Array of ValueTuple.</returns>
        public static (double x, double y)[] ConvertPointListToValueTupleArray(IReadOnlyList<Point> src, (double x, double y)[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new (double x, double y)[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref (double x, double y) dstRef = ref dst[i];
                dstRef.x = src[i].x;
                dstRef.y = src[i].y;
            }

            return dst;
        }

        #endregion

        #region ConvertDouble

        /// <summary>
        /// Convert double array to double list.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">List of double.</param>
        /// <returns>List of double.</returns>
        public static List<double> ConvertArrayToList(double[] src, List<double> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<double>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Length; ++i)
            {
                dst.Add(src[i]);
            }

            return dst;
        }

        /// <summary>
        /// Convert double array to Vector2 list.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">List of Vector2.</param>
        /// <returns>List of Vector2.</returns>
        public static List<Vector2> ConvertArrayToVector2List(double[] src, List<Vector2> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Vector2>();
            }
            else
            {
                dst.Clear();
            }

            int len = src.Length / 2;
            for (int i = 0; i < len; ++i)
            {
                dst.Add(new Vector2((float)src[i * 2], (float)src[i * 2 + 1]));
            }

            return dst;
        }

        /// <summary>
        /// Convert double array to Vector2 array.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">Array of Vector2.</param>
        /// <returns>Array of Vector2.</returns>
        public static Vector2[] ConvertArrayToVector2Array(double[] src, Vector2[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Length / 2 != dst.Length)
                throw new ArgumentException("src.Length / 2 != dst.Length");

            if (dst == null)
            {
                dst = new Vector2[src.Length / 2];
            }

            for (int i = 0; i < dst.Length; ++i)
            {
                ref Vector2 dstRef = ref dst[i];
                dstRef.x = (float)src[i * 2];
                dstRef.y = (float)src[i * 2 + 1];
            }

            return dst;
        }

        /// <summary>
        /// Convert double array to Point list.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">List of Point.</param>
        /// <returns>List of Point.</returns>
        public static List<Point> ConvertArrayToPointList(double[] src, List<Point> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Point>();
            }

            if (dst.Count != src.Length / 2)
            {
                dst.Clear();
                for (int i = 0; i < src.Length / 2; i++)
                {
                    dst.Add(new Point());
                }
            }

            for (int i = 0; i < dst.Count; ++i)
            {
                dst[i].x = src[i * 2];
                dst[i].y = src[i * 2 + 1];
            }

            return dst;
        }

        /// <summary>
        /// Convert double array to Point array.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">Array of Point.</param>
        /// <returns>Array of Point.</returns>
        public static Point[] ConvertArrayToPointArray(double[] src, Point[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Length / 2 != dst.Length)
                throw new ArgumentException("src.Length / 2 != dst.Length");

            if (dst == null)
            {
                dst = new Point[src.Length / 2];
            }

            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = new Point(src[i * 2], src[i * 2 + 1]);
            }

            return dst;
        }

        /// <summary>
        /// Convert double array to Vec2d array.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">Array of Vec2d.</param>
        /// <returns>Array of Vec2d.</returns>
        public static Vec2d[] ConvertArrayToVec2dArray(double[] src, Vec2d[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Length / 2 != dst.Length)
                throw new ArgumentException("src.Length / 2 != dst.Length");

            if (dst == null)
            {
                dst = new Vec2d[src.Length / 2];
            }

#if NET_STANDARD_2_1
            var srcSpan = new ReadOnlySpan<double>(src);
            var dstSpan = new Span<Vec2d>(dst);
            srcSpan.CopyTo(MemoryMarshal.Cast<Vec2d, double>(dstSpan));
#else
            for (int i = 0; i < dst.Length; ++i)
            {
                ref Vec2d dstRef = ref dst[i];
                dstRef.Item1 = src[i * 2];
                dstRef.Item2 = src[i * 2 + 1];
            }
#endif
            return dst;
        }

        /// <summary>
        /// Convert double array to ValueTuple list.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">List of ValueTuple.</param>
        /// <returns>List of ValueTuple.</returns>
        public static List<(double x, double y)> ConvertArrayToValueTupleList(double[] src, List<(double x, double y)> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<(double x, double y)>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Length / 2; ++i)
            {
                dst.Add((src[i * 2], src[i * 2 + 1]));
            }

            return dst;
        }

        /// <summary>
        /// Convert double array to ValueTuple array.
        /// </summary>
        /// <param name="src">Array of double.</param>
        /// <param name="dst">Array of ValueTuple.</param>
        /// <returns>Array of ValueTuple.</returns>
        public static (double x, double y)[] ConvertArrayToValueTupleArray(double[] src, (double x, double y)[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Length / 2 != dst.Length)
                throw new ArgumentException("src.Length / 2 != dst.Length");

            if (dst == null)
            {
                dst = new (double x, double y)[src.Length / 2];
            }

            for (int i = 0; i < dst.Length; ++i)
            {
                ref (double x, double y) dstRef = ref dst[i];
                dstRef.x = src[i * 2];
                dstRef.y = src[i * 2 + 1];
            }

            return dst;
        }

        #endregion

        #region ConvertVec2d

        /// <summary>
        /// Convert Vec2d list to Vec2d array.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">Array of Vec2d.</param>
        /// <returns>Array of Vec2d.</returns>
        public static Vec2d[] ConvertVec2dListToVec2dArray(List<Vec2d> src, Vec2d[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vec2d[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i] = src[i];
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d array to Vec2d list.
        /// </summary>
        /// <param name="src">Array of Vec2d.</param>
        /// <param name="dst">List of Vec2d.</param>
        /// <returns>List of Vec2d.</returns>
        public static List<Vec2d> ConvertVec2dArrayToVec2dList(Vec2d[] src, List<Vec2d> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Vec2d>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Length; ++i)
            {
                dst.Add(src[i]);
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d list to Vector2 list.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">List of Vector2.</param>
        /// <returns>List of Vector2.</returns>
        public static List<Vector2> ConvertVec2dListToVector2List(IReadOnlyList<Vec2d> src, List<Vector2> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Vector2>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst.Add(new Vector2((float)src[i].Item1, (float)src[i].Item2));
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d list to Vector2 array.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">Array of Vector2.</param>
        /// <returns>Array of Vector2.</returns>
        public static Vector2[] ConvertVec2dListToVector2Array(IReadOnlyList<Vec2d> src, Vector2[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vector2[src.Count];
            }

            for (int i = 0; i < dst.Length; ++i)
            {
                ref Vector2 dstRef = ref dst[i];
                dstRef.x = (float)src[i].Item1;
                dstRef.y = (float)src[i].Item2;
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d list to Point list.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">List of Point.</param>
        /// <returns>List of Point.</returns>
        public static List<Point> ConvertVec2dListToPointList(IReadOnlyList<Vec2d> src, List<Point> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Point>();
            }

            if (dst.Count != src.Count)
            {
                dst.Clear();
                for (int i = 0; i < src.Count; i++)
                {
                    dst.Add(new Point());
                }
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i].x = src[i].Item1;
                dst[i].y = src[i].Item2;
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d list to Point array.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">Array of Point.</param>
        /// <returns>Array of Point.</returns>
        public static Point[] ConvertVec2dListToPointArray(IReadOnlyList<Vec2d> src, Point[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Point[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i] = new Point(src[i].Item1, src[i].Item2);
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d list to double array.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">Array of double.</param>
        /// <returns>Array of double.</returns>
        public static double[] ConvertVec2dListToArray(List<Vec2d> src, double[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count * 2 != dst.Length)
                throw new ArgumentException("src.Count * 2 != dst.Length");

            if (dst == null)
            {
                dst = new double[src.Count * 2];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i * 2] = src[i].Item1;
                dst[i * 2 + 1] = src[i].Item2;
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d array to double array.
        /// </summary>
        /// <param name="src">Array of Vec2d.</param>
        /// <param name="dst">Array of double.</param>
        /// <returns>Array of double.</returns>
        public static double[] ConvertVec2dArrayToArray(Vec2d[] src, double[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Length * 2 != dst.Length)
                throw new ArgumentException("src.Count * 2 != dst.Length");

            if (dst == null)
            {
                dst = new double[src.Length * 2];
            }

#if NET_STANDARD_2_1
            var srcSpan = new ReadOnlySpan<Vec2d>(src);
            var dstSpan = new Span<double>(dst);
            srcSpan.CopyTo(MemoryMarshal.Cast<double, Vec2d>(dstSpan));
#else
            for (int i = 0; i < src.Length; ++i)
            {
                ref readonly Vec2d srcRef = ref src[i];
                dst[i * 2] = srcRef.Item1;
                dst[i * 2 + 1] = srcRef.Item2;
            }
#endif

            return dst;
        }

        /// <summary>
        /// Convert Vec2d list to ValueTuple list.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">List of ValueTuple.</param>
        /// <returns>List of ValueTuple.</returns>
        public static List<(double x, double y)> ConvertVec2dListToValueTupleList(IReadOnlyList<Vec2d> src, List<(double x, double y)> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<(double x, double y)>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst.Add((src[i].Item1, src[i].Item2));
            }

            return dst;
        }

        /// <summary>
        /// Convert Vec2d list to ValueTuple array.
        /// </summary>
        /// <param name="src">List of Vec2d.</param>
        /// <param name="dst">Array of ValueTuple.</param>
        /// <returns>Array of ValueTuple.</returns>
        public static (double x, double y)[] ConvertVec2dListToValueTupleArray(IReadOnlyList<Vec2d> src, (double x, double y)[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new (double x, double y)[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref (double x, double y) dstRef = ref dst[i];
                dstRef.x = src[i].Item1;
                dstRef.y = src[i].Item2;
            }

            return dst;
        }

        #endregion

        #region ConvertValueTuple

        /// <summary>
        /// Convert ValueTuple list to ValueTuple array.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">Array of ValueTuple.</param>
        /// <returns>Array of ValueTuple.</returns>
        public static (double x, double y)[] ConvertValueTupleListToValueTupleArray(List<(double x, double y)> src, (double x, double y)[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new (double x, double y)[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref (double x, double y) dstRef = ref dst[i];
                dstRef.Item1 = src[i].x;
                dstRef.Item2 = src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple array to ValueTuple list.
        /// </summary>
        /// <param name="src">Array of ValueTuple.</param>
        /// <param name="dst">List of ValueTuple.</param>
        /// <returns>List of ValueTuple.</returns>
        public static List<(double x, double y)> ConvertValueTupleArrayToValueTupleList((double x, double y)[] src, List<(double x, double y)> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<(double x, double y)>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Length; ++i)
            {
                dst.Add(src[i]);
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple list to Vector2 list.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">List of Vector2.</param>
        /// <returns>List of Vector2.</returns>
        public static List<Vector2> ConvertValueTupleListToVector2List(IReadOnlyList<(double x, double y)> src, List<Vector2> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Vector2>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst.Add(new Vector2((float)src[i].x, (float)src[i].y));
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple list to Vector2 array.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">Array of Vector2.</param>
        /// <returns>Array of Vector2.</returns>
        public static Vector2[] ConvertValueTupleListToVector2Array(IReadOnlyList<(double x, double y)> src, Vector2[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vector2[src.Count];
            }

            for (int i = 0; i < dst.Length; ++i)
            {
                ref Vector2 dstRef = ref dst[i];
                dstRef.x = (float)src[i].x;
                dstRef.y = (float)src[i].y;
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple list to Point list.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">List of Point.</param>
        /// <returns>List of Point.</returns>
        public static List<Point> ConvertValueTupleListToPointList(IReadOnlyList<(double x, double y)> src, List<Point> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Point>();
            }

            if (dst.Count != src.Count)
            {
                dst.Clear();
                for (int i = 0; i < src.Count; i++)
                {
                    dst.Add(new Point());
                }
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i].x = src[i].Item1;
                dst[i].y = src[i].Item2;
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple list to Point array.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">Array of Point.</param>
        /// <returns>Array of Point.</returns>
        public static Point[] ConvertValueTupleListToPointArray(IReadOnlyList<(double x, double y)> src, Point[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Point[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i] = new Point(src[i].Item1, src[i].Item2);
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple list to double array.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">Array of double.</param>
        /// <returns>Array of double.</returns>
        public static double[] ConvertValueTupleListToArray(IReadOnlyList<(double x, double y)> src, double[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count * 2 != dst.Length)
                throw new ArgumentException("src.Count * 2 != dst.Length");

            if (dst == null)
            {
                dst = new double[src.Count * 2];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst[i * 2] = src[i].Item1;
                dst[i * 2 + 1] = src[i].Item2;
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple list to Vec2d list.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">List of Vec2d.</param>
        /// <returns>List of Vec2d.</returns>
        public static List<Vec2d> ConvertValueTupleListToVec2dList(IReadOnlyList<(double x, double y)> src, List<Vec2d> dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
            {
                dst = new List<Vec2d>();
            }
            else
            {
                dst.Clear();
            }

            for (int i = 0; i < src.Count; ++i)
            {
                dst.Add(new Vec2d(src[i].Item1, src[i].Item2));
            }

            return dst;
        }

        /// <summary>
        /// Convert ValueTuple list to Vec2d array.
        /// </summary>
        /// <param name="src">List of ValueTuple.</param>
        /// <param name="dst">Array of Vec2d.</param>
        /// <returns>Array of Vec2d.</returns>
        public static Vec2d[] ConvertValueTupleListToVec2dArray(IReadOnlyList<(double x, double y)> src, Vec2d[] dst = null)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst != null && src.Count != dst.Length)
                throw new ArgumentException("src.Count != dst.Length");

            if (dst == null)
            {
                dst = new Vec2d[src.Count];
            }

            for (int i = 0; i < src.Count; ++i)
            {
                ref Vec2d dstRef = ref dst[i];
                dstRef.Item1 = src[i].x;
                dstRef.Item2 = src[i].y;
            }

            return dst;
        }

        #endregion
    }
}

#endif
