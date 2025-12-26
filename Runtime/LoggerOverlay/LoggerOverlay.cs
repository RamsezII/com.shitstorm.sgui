using _ARK_;
using _UTIL_;
using System.Text;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class LoggerOverlay : MonoBehaviour
    {
        public static LoggerOverlay instance;

        RectTransform rt;
        TextMeshProUGUI text;

        HeartBeat.Operation operation;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            logs.Clear();
            Application.logMessageReceivedThreaded -= OnLogMessageReceived;
            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            text = GetComponentInChildren<TextMeshProUGUI>();
            rt = (RectTransform)text.transform.parent;
        }

        private void OnEnable()
        {
           operation = NUCLEOR.instance.heartbeat_unscaled.AddOperation(new("refresh logger overlay", .065f, true, RefreshTexts));
        }

        private void OnDisable()
        {
            operation.Dispose();
            rt.sizeDelta = Vector2.zero;
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshTexts()
        {
            operation.timer = 0;

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