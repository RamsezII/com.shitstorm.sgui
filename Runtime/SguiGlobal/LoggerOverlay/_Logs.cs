using _ARK_;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    partial class LoggerOverlay
    {
        public enum LogLevel
        {
            Sublog = -1,
            Info,
            Warn,
            Error,
            Fatal
        }

        static readonly List<(string text, float deadline)> logs = new();

        //--------------------------------------------------------------------------------------------------------------

        static void OnLogMessageReceived(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Warning && message.StartsWith("The character with Unicode value "))
                return;

            switch (type)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Warning:
                case LogType.Exception:
                    message = message.TrimEnd('\n', '\r');
                    message = type switch
                    {
                        LogType.Error => $"<color=\"orange\">{message}</color>",
                        LogType.Assert => $"<color=\"red\">{message.Bold()}</color>",
                        LogType.Warning => $"<color=\"yellow\">{message}</color>",
                        LogType.Exception => $"<color=\"red\">{message.Bold()}</color>",
                        _ => message,
                    };
                    Log(message);
                    break;
            }
        }

        public static void Log(in object o, in float timer = 2, in LogLevel logLevel = 0)
        {
            while (logs.Count >= 20)
                logs.RemoveAt(0);

            string text = $"[{DateTime.Now:HH:mm:ss}, {(Time.inFixedTimeStep ? $"ff{NUCLEOR.instance.fixedFrameCount}" : $"f{Time.frameCount}")}] ".SetSize_percent(75);

            if (o != null)
            {
                string s = o.ToString();

                text += logLevel switch
                {
                    LogLevel.Sublog => s.ToSubLog(),
                    LogLevel.Warn => s.SetColor(Colors.yellow),
                    LogLevel.Error => s.SetColor(Colors.orange),
                    LogLevel.Fatal => s.SetColor(Colors.red),
                    _ => s,
                };

                if (logLevel <= 0)
                    Debug.Log(text);
            }

            logs.Add((text, timer + Time.unscaledTime));

            if (instance != null)
                instance.gameObject.SetActive(true);
        }
    }
}