using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class LoggerOverlay : MonoBehaviour
    {
        public static LoggerOverlay instance;

        [Serializable]
        public class Log
        {
            public readonly string text;
            public readonly float deadline;
            public Log(in string text, in float timer = 2)
            {
                this.text = text;
                deadline = Time.unscaledTime + timer;
            }
        }

        readonly List<Log> logs = new();

        RectTransform rt;
        TextMeshProUGUI text;

        HeartBeat.Operation operation;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            text = GetComponentInChildren<TextMeshProUGUI>();
            rt = (RectTransform)text.transform.parent;
        }

        private void OnEnable()
        {
            NUCLEOR.instance.heartbeat_unscaled.operations.Add(operation = new(.065f, false, RefreshTexts));
        }

        private void OnDisable()
        {
            NUCLEOR.instance.heartbeat_unscaled.operations.Remove(operation);
            rt.sizeDelta = Vector2.zero;
        }

        //--------------------------------------------------------------------------------------------------------------

        public void AddLog(in object text, in float timer = 2) => AddLog(new Log(text.ToString(), timer));
        public void AddLog(in Log log)
        {
            while (logs.Count >= 50)
                logs.RemoveAt(0);
            logs.Add(log);
            gameObject.SetActive(true);
        }

        void RefreshTexts()
        {
            if (logs.Count == 0)
            {
                text.text = string.Empty;
                gameObject.SetActive(false);
                return;
            }

            StringBuilder sb = new();

            for (int i = logs.Count - 1; i >= 0; i--)
                if (Time.unscaledTime > logs[i].deadline)
                    logs.RemoveAt(i);
                else
                    sb.AppendLine(logs[i].text);

            text.SetText(sb);
            rt.sizeDelta = text.GetPreferredValues();
        }
    }
}