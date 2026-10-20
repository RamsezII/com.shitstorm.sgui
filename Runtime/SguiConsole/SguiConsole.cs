using _SGUI_.composer;
using _SGUI_.console;
using _UTIL_;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiConsole : SguiFrame
    {
        [AutoStaticsCleanup] static new readonly HashSet<SguiConsole> instances = new();

        [Serializable]
        internal readonly struct LogMessage
        {
            public readonly string text, stacktrace;
            public readonly LogType type;
            public LogMessage(string text, string stacktrace, LogType type)
            {
                this.text = text.TrimmedExceptionMessage();
                this.stacktrace = stacktrace.TrimmedExceptionMessage();
                this.type = type;
            }
        }

        [AutoStaticsCleanup] static readonly Queue<LogMessage> messages = new();

        [SerializeField] TMP_InputField tmp_selected;
        [SerializeField] DragHandler drag_ratio;
        [SerializeField] ScrollRect scrollview_list, scrollview_selected;
        [SerializeField] ScrollHandler scroll_selected;
        [SerializeField] VerticalLayoutGroup vlayout;
        [SerializeField] SguiConsoleMessage prefab_message;
        [SerializeField] internal Texture icon_log, icon_warn, icon_err;
        readonly Queue<SguiConsoleMessage> clones = new();
        readonly ValueNotifier<float> ratio = new();
        const int limit = 50;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            Application.logMessageReceivedThreaded -= OnLogMessageReceived;
            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnInitialize()
        {
            base.OnInitialize();

            tab.trad_title.SetText("Console");

            prefab_message.gameObject.SetActive(false);

            tmp_selected.text = string.Empty;

            var button_clear = header.AddButton(new() { french = "Vider", english = "Clear", });
            button_clear.button.onClick.AddListener(Clear);

            var button_logs = header.AddButton(new("Logs"), toggle: true);

            var button_warnings = header.AddButton(new("Warnings"), toggle: true);

            var button_errors = header.AddButton(new() { french = "Erreurs", english = "Errors", }, toggle: true);

            scroll_selected.onScroll += scrollview_selected.OnScroll;

            instances.Add(this);

            foreach (var msg in messages)
                AddMessage(msg);
        }

        //--------------------------------------------------------------------------------------------------------------

        static void OnLogMessageReceived(string text, string stackTrace, LogType type)
        {
            LogMessage message = new(text, stackTrace, type);

            while (messages.Count >= limit - 1)
                messages.Dequeue();
            messages.Enqueue(message);

            foreach (var instance in instances)
                if (instance != null)
                    instance.AddMessage(message);
        }

        void AddMessage(LogMessage message)
        {
            while (clones.Count >= limit - 1)
            {
                clones.TryDequeue(out var old_clone);
                Destroy(old_clone.gameObject);
            }

            var clone = Instantiate(prefab_message, parent: prefab_message.transform.parent);
            clones.Enqueue(clone);
            clone.gameObject.SetActive(true);
            clone.Initialize(message);

            clone.tmptext.text = message.text;
        }

        internal void OnSelectEntry(SguiConsoleMessage entry)
        {
            foreach (var e in clones)
                e.graphic_select.gameObject.SetActive(e == entry);

            string text = $"{entry.message.stacktrace}\n{entry.message.text}";
            tmp_selected.text = text;

            scrollview_selected.content.sizeDelta = new(
                x: 0,
                y: tmp_selected.textComponent.GetPreferredValues(
                    text: text,
                    width: scrollview_selected.content.rect.width,
                    height: float.MaxValue
                ).y
            );
        }

        public void Clear()
        {
            foreach (var clone in clones)
                Destroy(clone.gameObject);
            clones.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            instances.Remove(this);

            base.OnDestroy();
        }
    }
}