using _ARK_;
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
        protected float offset_top_h = 2, offset_bottom_h = 5;

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
            stdout_field.rT.anchoredPosition = new Vector2(0, -offset_top_h);
            stdin_field.onValidateInput += OnValidateStdin_char;
            stdin_field.onValueChanged.AddListener(OnStdinChanged);
            stdin_field.onSelect.AddListener(OnSelectStdin);
            stdin_field.onDeselect.AddListener(OnDeselectStdin);
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
            IMGUI_global.instance.users_inputs.AddElement(OnImguiInputs, this);
        }

        protected virtual void OnDeselectStdin(string arg0)
        {
            IMGUI_global.instance.users_inputs.RemoveKeysByValue(this);
        }

        protected virtual bool OnImguiInputs(Event e)
        {
            return false;
        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
        }
    }
}