namespace BlinkDetection
{
    /// <summary>
    /// 眨眼事件处理器接口。实现此接口并通过 BlinkEventSystem.RegisterHandler 注册即可接收眨眼事件。
    /// </summary>
    public interface IBlinkEventHandler
    {
        void HandleBlink(BlinkEventData data);
    }
}
