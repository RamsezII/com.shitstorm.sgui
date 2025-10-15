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

        TextMeshProUGUI text;

        HeartBeat.Operation operation;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            NUCLEOR.instance.heartbeat_unscaled.operations.Add(operation = new(.065f, false, RefreshTexts));
            RefreshTexts();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void AddLog(in object text) => AddLog(new Log(text.ToString()));
        public void AddLog(in Log log)
        {
            while (logs.Count > 50)
                logs.RemoveAt(0);
            logs.Add(log);
            RefreshTexts();
        }

        void RefreshTexts()
        {
            StringBuilder sb = new();

            for (int i = logs.Count - 1; i >= 0; i--)
                if (Time.unscaledTime >= logs[i].deadline)
                    logs.RemoveAt(i);
                else
                    sb.AppendLine(logs[i].text);

            text.text = sb.ToString();
            gameObject.SetActive(sb.Length > 0);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (NUCLEOR.instance != null)
                NUCLEOR.instance.heartbeat_unscaled.operations.Remove(operation);
        }
    }
}