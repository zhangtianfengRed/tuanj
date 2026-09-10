namespace BlinkDetection
{
    /// <summary>
    /// 眨眼事件数据。每次检测到眨眼时由 WebCamBlinkDetector 生成。
    /// </summary>
    public struct BlinkEventData
    {
        /// <summary>眨眼完成时的时间戳（Time.time）。</summary>
        public double timestamp;

        /// <summary>闭眼持续时间（秒）。</summary>
        public float duration;

        /// <summary>检测置信度 0~1，越高越可靠。</summary>
        public float confidence;

        public BlinkEventData(double timestamp, float duration, float confidence)
        {
            this.timestamp = timestamp;
            this.duration = duration;
            this.confidence = confidence;
        }
    }
}
