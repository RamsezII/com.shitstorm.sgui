using System;
using TMPro;
using UnityEngine;

namespace _SGUI_.context_tools.settings
{
    public sealed class TextInput : MonoBehaviour
    {
        public TMP_InputField inputfield;
        [SerializeField] int frame_flag;
        public Action<string> onSubmit;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            inputfield = GetComponentInChildren<TMP_InputField>(true);
            inputfield.onSubmit.AddListener(text =>
            {
                if (frame_flag < Time.frameCount)
                {
                    frame_flag = 0;
                    onSubmit?.Invoke(text);
                }
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetValueNoSubmit(in string text)
        {
            inputfield.text = text;
            frame_flag = Time.frameCount;
        }
    }
}