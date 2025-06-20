using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public abstract partial class ShellView : MonoBehaviour
    {
        public ShellField stdout_field, stdin_field;
        public TextMeshProUGUI tmp_progress;
        public ScrollRect scrollview;
        public RectTransform content_rT;
        public Scrollbar scrollbar;

        //----------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            stdout_field = transform.Find("scrollview/viewport/content/std_out").GetComponent<ShellField>();
            stdin_field = stdout_field.transform.Find("std_in").GetComponent<ShellField>();

            tmp_progress = transform.Find("progress/text").GetComponent<TextMeshProUGUI>();

            scrollview = transform.Find("scrollview").GetComponent<ScrollRect>();

            content_rT = (RectTransform)transform.Find("scrollview/viewport/content");

            scrollbar = transform.Find("scrollview/scrollbar").GetComponent<Scrollbar>();
        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            stdin_field.inputfield.onValidateInput += OnValidateStdin_char;
            stdin_field.inputfield.onValueChanged.AddListener(OnStdinChanged);
            stdin_field.inputfield.onSelect.AddListener(OnSelectStdin);
        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual char OnValidateStdin_char(string text, int charIndex, char addedChar)
        {
            return addedChar;
        }

        protected virtual void OnStdinChanged(string value)
        {
        }

        protected virtual void OnSelectStdin(string text)
        {
        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
        }
    }
}