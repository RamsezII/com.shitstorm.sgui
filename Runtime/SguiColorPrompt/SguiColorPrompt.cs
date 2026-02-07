using _UTIL_;
using System;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class SguiColorPrompt : MonoBehaviour
    {
        static SguiColorPrompt instance;

        Action<Color> onSubmit;
        Action onCancel;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
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

        public static void RequestColor(in Action<Color> onSubmit, in Action onCancel)
        {
            instance.onSubmit = onSubmit;
            instance.gameObject.SetActive(true);
        }
    }
}