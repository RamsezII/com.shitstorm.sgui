using _SGUI_.prompts.color_prompt;
using _UTIL_;
using System;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class SguiColorPrompt : MonoBehaviour
    {
        public static SguiColorPrompt instance;

        [SerializeField] RectTransform rt;
        [SerializeField] ColorPrompt_color current_color, new_color;
        [SerializeField] TMP_InputField tmp_hex;

        Action<Color> onSubmit;
        Action onCancel;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            rt = (RectTransform)transform.Find("rt");

            current_color = transform.Find("rt/values/current").GetComponent<ColorPrompt_color>();
            new_color = transform.Find("rt/values/color").GetComponent<ColorPrompt_color>();
            tmp_hex = transform.Find("rt/vlayout/hex-value/inputfield").GetComponent<TMP_InputField>();

            transform.Find("background").GetComponent<PointerClickHandler>().onClick += eventData =>
            {
                gameObject.SetActive(false);
                onCancel?.Invoke();
                onSubmit = null;
                onCancel = null;
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ShowColorPrompt(in Vector2 position, in Color currentColor, in Action<Color> onSubmit, in Action onCancel = null)
        {
            this.onSubmit = onSubmit;
            this.onCancel = onCancel;
            current_color.SetColor(currentColor);
            gameObject.SetActive(true);
            rt.position = position;
        }
    }
}