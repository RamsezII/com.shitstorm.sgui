using _SGUI_.console;
using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiConsole
    {
        [Serializable]
        internal readonly struct LogEntry
        {
            public readonly string text, stacktrace;
            public readonly LogType type;
            public LogEntry(string text, string stacktrace, LogType type)
            {
                this.text = text.TrimmedExceptionMessage();
                this.stacktrace = stacktrace.TrimmedExceptionMessage();
                this.type = type;
            }
        }

        const int max_entry_count = 50;
        [AutoStaticsCleanup] static readonly Queue<LogEntry> entries = new(max_entry_count);

        [SerializeField] SguiConsoleEntry prefab_entry;
        readonly Queue<SguiConsoleEntry> entries_clones = new();

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitLogs()
        {
            Application.logMessageReceivedThreaded -= OnLogMessageReceived;
            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        //--------------------------------------------------------------------------------------------------------------

        void InitEntries()
        {
            prefab_entry.gameObject.SetActive(false);

            var button_logs = header.AddButton(new("Logs"), use_toggle: true);
            var button_warnings = header.AddButton(new("Warnings"), use_toggle: true);
            var button_errors = header.AddButton(new() { french = "Erreurs", english = "Errors", }, use_toggle: true);

            button_logs.toggle.AddListener(OnFilters);
            button_warnings.toggle.AddListener(OnFilters);
            button_errors.toggle.AddListener(OnFilters);

            foreach (var msg in entries)
                AddEntry(msg);

            void OnFilters()
            {
                foreach (var entry in entries_clones)
                {
                    var locked = (entry.message.type switch
                    {
                        LogType.Error or LogType.Assert or LogType.Exception => button_errors,
                        LogType.Warning => button_warnings,
                        LogType.Log => button_logs,
                        _ => throw new NotImplementedException(),
                    }).toggle._value;

                    entry.gameObject.SetActive(!locked);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        static void OnLogMessageReceived(string text, string stackTrace, LogType type)
        {
            try
            {
                LogEntry entry = new(text, stackTrace, type);

                while (entries.Count >= max_entry_count - 1)
                    entries.Dequeue();
                entries.Enqueue(entry);

                foreach (var instance in instances)
                    if (instance != null)
                        instance.AddEntry(entry);
            }
            catch (Exception e)
            {
            }
        }

        void AddEntry(LogEntry entry)
        {
            while (entries_clones.Count >= max_entry_count - 1)
            {
                entries_clones.TryDequeue(out var old_clone);
                Destroy(old_clone.gameObject);
            }

            var clone = Instantiate(prefab_entry, parent: prefab_entry.transform.parent);
            entries_clones.Enqueue(clone);
            clone.gameObject.SetActive(true);
            clone.Initialize(entry);

            clone.tmptext.text = entry.text;
        }

        internal void OnEntrySelected(SguiConsoleEntry entry)
        {
            foreach (var e in entries_clones)
                e.graphic_select.gameObject.SetActive(e == entry);

            string text = $"{entry.message.stacktrace}\n{entry.message.text}";
            tmp_selected.text = text;

            scrollview_selected.content.sizeDelta = new(
                x: 0,
                y: tmp_selected.textComponent.GetPreferredValues(
                    text: text,
                    width: tmp_selected.textComponent.rectTransform.rect.width - tmp_selected.textComponent.margin.x - tmp_selected.textComponent.margin.z,
                    height: float.MaxValue
                ).y
            );
        }

        public void Clear()
        {
            foreach (var clone in entries_clones)
                Destroy(clone.gameObject);
            entries_clones.Clear();
        }
    }
}
