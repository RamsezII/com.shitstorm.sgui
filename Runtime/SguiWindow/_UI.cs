using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiWindow
    {
        [HideInInspector] public Canvas canvas;
        [HideInInspector] public GraphicRaycaster raycaster;
        [HideInInspector] public RectTransform rT, rT_parent;

        internal SguiZone zone_header, zone_outline;

        public Traductable trad_title;
        public Graphic huable_background;
        public Button button_hide, button_fullscreen, button_close;

        [SerializeField, Range(0, 1)] protected float anim_alpha = 1;

        [SerializeField, Range(0, 1)] float ui_hue_start, ui_hue_current;
        [SerializeField] float ui_alpha;

        Rect init_rect;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeUI()
        {
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
            rT = (RectTransform)transform.Find("rT");
            rT_parent = (RectTransform)rT.parent;

            init_rect = rT.rect;
        }

        //--------------------------------------------------------------------------------------------------------------

        void StartUI()
        {
            if (animate_hue)
            {
                ui_alpha = huable_background.color.a;
                ui_hue_start = huable_background.color.GetHue();
            }

            if (fullscreen != null)
            {
                fullscreen.AddListener(toggle =>
                {
                    if (toggle)
                    {
                        rT.sizeDelta = Vector2.zero;
                        rT.anchorMin = Vector2.zero;
                        rT.anchorMax = Vector2.one;
                        rT.anchoredPosition = Vector2.zero;
                    }
                    else
                    {
                        rT.sizeDelta = init_rect.size;
                        rT.anchorMin = rT.anchorMax = Vector2.zero;
                        rT.anchoredPosition = 50 * Vector2.one;
                    }
                });

                button_fullscreen.onClick.AddListener(fullscreen.Toggle);
            }

            button_hide?.onClick.AddListener(() => sgui_toggle_window.Update(false));
            button_close?.onClick.AddListener(() =>
            {
                if (hide_on_close)
                    sgui_toggle_window.Update(false);
                else
                    Oblivionize();
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void UpdateHue()
        {
            const float ui_hue_speed = .03f;
            ui_hue_current = (ui_hue_start + Time.unscaledTime * ui_hue_speed) % 1;
            huable_background.color = huable_background.color.ModifyHsv(ui_hue_current, ui_alpha * anim_alpha);
        }

        protected virtual void OnUpdateAlpha()
        {
        }
    }
}