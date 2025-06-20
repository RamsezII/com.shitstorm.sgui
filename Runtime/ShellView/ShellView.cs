using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public abstract partial class ShellView : MonoBehaviour
    {
        public ShellField std_out, std_in;
        public TextMeshProUGUI tmp_progress;
        public ScrollRect scrollview;
        public Scrollbar scrollbar;

        //----------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            std_out = transform.Find("scrollview/viewport/content_layout/std_out").GetComponent<ShellField>();
            std_in = transform.Find("scrollview/viewport/content_layout/std_in").GetComponent<ShellField>();
            tmp_progress = transform.Find("progress/text").GetComponent<TextMeshProUGUI>();
            scrollview = transform.Find("scrollview").GetComponent<ScrollRect>();
            scrollbar = transform.Find("scrollview/scrollbar").GetComponent<Scrollbar>();
        }

        //----------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            std_in.inputfield.onValidateInput += OnValidateInput;
            std_in.inputfield.onValueChanged.AddListener(OnValueChanged);
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