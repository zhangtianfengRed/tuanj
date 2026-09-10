using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlinkDetection
{
    /// <summary>
    /// 眨眼事件中枢。检测器调用 RaiseBlink，外部通过 OnBlinkDetected 事件或 RegisterHandler 接收。
    /// </summary>
    public static class BlinkEventSystem
    {
        /// <summary>C# 事件，直接订阅/取消订阅。</summary>
        public static event Action<BlinkEventData> OnBlinkDetected;

        private static readonly List<IBlinkEventHandler> handlers = new List<IBlinkEventHandler>();

        /// <summary>注册接口式处理器。</summary>
        public static void RegisterHandler(IBlinkEventHandler handler)
        {
            if (handler != null && !handlers.Contains(handler))
            {
                handlers.Add(handler);
            }
        }

        /// <summary>注销接口式处理器。</summary>
        public static void UnregisterHandler(IBlinkEventHandler handler)
        {
            handlers.Remove(handler);
        }

        /// <summary>由检测器内部调用，广播眨眼事件。</summary>
        public static void RaiseBlink(BlinkEventData data)
        {
            OnBlinkDetected?.Invoke(data);

            for (int i = 0; i < handlers.Count; i++)
            {
                if (handlers[i] != null)
                {
                    handlers[i].HandleBlink(data);
                }
            }
        }

        /// <summary>清除所有订阅（场景切换时调用）。</summary>
        public static void ClearAll()
        {
            OnBlinkDetected = null;
            handlers.Clear();
        }
    }
}
