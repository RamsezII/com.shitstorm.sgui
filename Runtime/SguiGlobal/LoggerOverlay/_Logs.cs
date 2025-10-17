using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    partial class LoggerOverlay
    {
        public enum LogLevel
        {
            Info,
            Sublog,
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

            string text = $"[{DateTime.Now:HH:mm:ss tt}] " + logLevel switch
            {
                LogLevel.Sublog => o.ToSubLog(),
                LogLevel.Warn => o.ToString().SetColor(Colors.yellow),
                LogLevel.Error => o.ToString().SetColor(Colors.orange),
                LogLevel.Fatal => o.ToString().SetColor(Colors.red),
                _ => o.ToString(),
            };

            logs.Add((text, timer + Time.unscaledTime));

            if (instance != null)
                instance.gameObject.SetActive(true);
        }
    }
}