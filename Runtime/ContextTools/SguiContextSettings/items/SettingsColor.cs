using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.context_tools.settings
{
    public sealed class SettingsColor : ContextSetting_item
    {
        [SerializeField] Button button;
        [SerializeField] Graphic graphic_color;
        [SerializeField] RectTransform rt_alpha;

        public Color current_color;
        public Action<Color> onSubmit;

        bool promptIsUp;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            button = GetComponentInChildren<Button>(true);
            graphic_color = button.transform.Find("background").GetComponent<Graphic>();
            rt_alpha = (RectTransform)button.transform.Find("alpha/fill");

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            button.onClick.AddListener(() =>
            {
                if (_destroyed)
                    return;

                SguiColorPrompt.instance.ShowColorPrompt(Input.mousePosition, current_color, value =>
                {
                    SetColor(value);
                    onSubmit?.Invoke(value);
                });

                promptIsUp = true;
                SguiColorPrompt.instance._onClose += () => promptIsUp = false;
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetColor(in Color color)
        {
            current_color = color;

            Color c = color;
            c.a = 1;
            graphic_color.color = c;
            rt_alpha.anchorMax = new(color.a, 1);

            if (promptIsUp)
                SguiColorPrompt.instance.SetNewColor(color);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (promptIsUp)
                SguiColorPrompt.instance.Cancel();
        }
    }
}