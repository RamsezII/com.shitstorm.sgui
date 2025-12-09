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

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            scrollview = GetComponentInParent<ScrollRect>();
            rT = (RectTransform)transform;
            lint = transform.Find("area/lint").GetComponent<TextMeshProUGUI>();
            base.onValueChanged.AddListener(OnValueChanged);

            base.Awake();
        }

        //----------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            if (this == null)
                return;

            base.OnEnable();

            if (Application.isPlaying)
                IMGUI_global.instance.inputs_users.AddElement(OnImguiInputs);
        }

        protected override void OnDisable()
        {
            if (this == null)
                return;

            base.OnDisable();

            if (Application.isPlaying)
                IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);
        }

        //----------------------------------------------------------------------------------------------------------

        void OnValueChanged(string arg0)
        {
            onValueChanged?.Invoke(arg0);
            return;

            if (arg0.ZSpaced(out string zspaced))
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

        bool OnImguiInputs(Event e)
        {
            return false;

            if (!isFocused)
                return false;

            if (e != null && e.isKey && e.type == EventType.KeyDown)
                if (e.control || e.command)
                    switch (e.keyCode)
                    {
                        case KeyCode.C:
                            OnCtrlC();
                            return true;

                        case KeyCode.X:
                            OnCtrlX();
                            return true;

                        case KeyCode.V:
                            OnCtrlV();
                            return true;
                    }

            return false;
        }

        void OnCtrlC()
        {
            if (TryGetSelectedString(out string selected, out _, out _))
            {
                selected.UnZSpaced(out selected);
                GUIUtility.systemCopyBuffer = selected;
            }
        }

        void OnCtrlV()
        {
            GetSelectedString(out int start, out int end);
            string text = this.text;
            text = text[..start] + GUIUtility.systemCopyBuffer + text[end..];
            text.ZSpaced(out string zspaced);
            textComponent.text = zspaced;
        }

        void OnCtrlX()
        {
            if (TryGetSelectedString(out string selected, out int start, out int end))
            {
                selected.UnZSpaced(out selected);
                GUIUtility.systemCopyBuffer = selected;
                string text = this.text;
                text = text[..start] + text[end..];
                text.ZSpaced(out string zspaced);
                textComponent.text = zspaced;
            }
        }
    }
}