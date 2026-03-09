using _ARK_;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum SguiLogLevel
{
    _ConsolRedirect = -1,
    Info,
    Sublog,
    Warning,
    Error,
    Fatal
}

partial class SguiLoggerOverlay
{
    static readonly List<(string text, float deadline)> logs = new();

    //--------------------------------------------------------------------------------------------------------------

    static void OnLogMessageReceived(string message, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Log:
            case LogType.Warning when message.StartsWith("The character with Unicode value "):
                return;
        }

        message = message.TrimEnd('\n', '\r');

        message = type switch
        {
            LogType.Error => $"<color=\"orange\">{message}</color>",
            LogType.Assert => $"<color=\"red\">{message.Bold()}</color>",
            LogType.Warning => $"<color=\"yellow\">{message}</color>",
            LogType.Exception => $"<color=\"red\">{message.Bold()}</color>",
            _ => message,
        };

        NUCLEOR.delegates.Update_OnStartOfFrame_once += () => Log(message, timer: type < LogType.Log ? 3 : 2, logLevel: SguiLogLevel._ConsolRedirect);
    }

    public static void Log(in object o, in UnityEngine.Object context = null, in float timer = 2, in SguiLogLevel logLevel = SguiLogLevel.Info, in bool doNotRedirectToConsole = false)
    {
        while (logs.Count >= 20)
            logs.RemoveAt(0);

        string text = $"[{DateTime.Now:HH:mm:ss}, {(Time.inFixedTimeStep ? $"fupd{NUCLEOR.instance.fixedFrameCount}" : $"upd{Time.frameCount}")}] ".SetSize_percent(75);

        if (o != null)
        {
            string s = o.ToString();

            if (s.Contains('\n', StringComparison.OrdinalIgnoreCase))
                text += "\n";

            text += logLevel switch
            {
                SguiLogLevel.Sublog => s.ToSubLog(),
                SguiLogLevel.Warning => s.SetColor(Colors.yellow),
                SguiLogLevel.Error => s.SetColor(Colors.orange),
                SguiLogLevel.Fatal => s.SetColor(Colors.red),
                _ => s,
            };

            if (!doNotRedirectToConsole)
                if (timer > 0)
                    switch (logLevel)
                    {
                        case SguiLogLevel._ConsolRedirect:
                            break;

                        case SguiLogLevel.Warning:
                            Debug.LogWarning(text, context);
                            break;

                        case SguiLogLevel.Error:
                        case SguiLogLevel.Fatal:
                            Debug.LogError(text, context);
                            break;

                        default:
                            Debug.Log(text, context);
                            break;
                    }
        }

        logs.Add((text, timer + Time.unscaledTime));

        if (timer == 0)
            instance.RefreshTexts();

        if (instance != null)
            instance.gameObject.SetActive(true);
    }
}