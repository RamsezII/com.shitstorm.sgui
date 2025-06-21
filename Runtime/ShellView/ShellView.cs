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
        public RectTransform scrollview_rT;
        public Scrollbar scrollbar;

        //----------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            stdout_field = transform.Find("scrollview/viewport/content_layout/std_out").GetComponent<ShellField>();
            stdin_field = transform.Find("scrollview/viewport/content_layout/std_in").GetComponent<ShellField>();

            tmp_progress = transform.Find("progress/text").GetComponent<TextMeshProUGUI>();

            scrollview = transform.Find("scrollview").GetComponent<ScrollRect>();
            scrollview_rT = (RectTransform)scrollview.transform;

            scrollbar = transform.Find("scrollview/scrollbar").GetComponent<Scrollbar>();
        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            stdin_field.inputfield.onValidateInput += OnValidateInput;
            stdin_field.inputfield.onValueChanged.AddListener(OnValueChanged);
        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual char OnValidateInput(string text, int charIndex, char addedChar)
        {
            return addedChar;
        }

        protected virtual void OnValueChanged(string value)
        {

        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {

        }
    }
}