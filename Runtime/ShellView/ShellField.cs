using _ARK_;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed class ShellField : TMP_InputField
    {
        ScrollRect scrollview;
        public RectTransform rT;
        public TextMeshProUGUI lint;
        public new Action<string> onValueChanged;

        public static bool zspaces_check = true;

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            scrollview = GetComponentInParent<ScrollRect>();
            rT = (RectTransform)transform;
            lint = transform.Find("area/lint").GetComponent<TextMeshProUGUI>();

            base.Awake();

            onSelect.AddListener(_ => IMGUI_global.instance.clipboard_users.AddElement(OnClipboardOperation));
            onDeselect.AddListener(_ => IMGUI_global.instance.clipboard_users.RemoveElement(OnClipboardOperation));

            base.onValueChanged.AddListener(OnValueChanged);
        }

        //----------------------------------------------------------------------------------------------------------

        protected override void OnDisable()
        {
            IMGUI_global.instance.clipboard_users.RemoveElement(OnClipboardOperation);
        }

        //----------------------------------------------------------------------------------------------------------

        bool OnClipboardOperation(Event e, IMGUI_global.ClipboardOperations operation)
        {
            LoggerOverlay.Log($"{GetType()} Clipboard Operation: \"{operation}\"", this);
            switch (operation)
            {
                case IMGUI_global.ClipboardOperations.Copy:
                    OnCtrlC();
                    return true;

                case IMGUI_global.ClipboardOperations.Cut:
                    OnCtrlX();
                    return true;

                case IMGUI_global.ClipboardOperations.Paste:
                    OnCtrlV();
                    return true;
            }
            return false;
        }

        void OnValueChanged(string arg0)
        {
            if (zspaces_check && arg0.ZSpaced(out string zspaced))
                textComponent.text = zspaced;
            else
                onValueChanged?.Invoke(arg0);
        }

        public override void OnScroll(PointerEventData eventData)
        {
            base.OnScroll(eventData);
            scrollview.OnScroll(eventData);
        }

        public bool TryGetSelectedString(out string selectedString, out int start, out int end)
        {
            selectedString = GetSelectedString(out start, out end);
            return !string.IsNullOrEmpty(selectedString);
        }

        public string GetSelectedString(out int start, out int end)
        {
            start = Mathf.Min(selectionStringFocusPosition, selectionStringAnchorPosition);
            end = Mathf.Max(selectionStringFocusPosition, selectionStringAnchorPosition);

            if (selectionStringFocusPosition == selectionStringAnchorPosition)
                return string.Empty;

            return text[start..end];
        }

        void OnCtrlC()
        {
            if (TryGetSelectedString(out string selected, out _, out _))
            {
                if (zspaces_check)
                    selected.UnZSpaced(out selected);

                GUIUtility.systemCopyBuffer = selected;
            }
        }

        void OnCtrlV()
        {
            GetSelectedString(out int start, out int end);

            string text = this.text;
            text = text[..start] + GUIUtility.systemCopyBuffer + text[end..];

            if (zspaces_check)
                text.ZSpaced(out text);

            textComponent.text = text;
        }

        void OnCtrlX()
        {
            if (TryGetSelectedString(out string selected, out int start, out int end))
            {
                if (zspaces_check)
                    selected.UnZSpaced(out selected);

                GUIUtility.systemCopyBuffer = selected;

                string text = this.text;
                text = text[..start] + text[end..];

                if (zspaces_check)
                    text.ZSpaced(out text);

                textComponent.text = text;
            }
        }
    }
}