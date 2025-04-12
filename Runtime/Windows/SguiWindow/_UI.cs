using _UTIL_;
using UnityEngine.UI;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow
    {
        [HideInInspector] public Canvas canvas;
        [HideInInspector] public GraphicRaycaster raycaster;

        public Traductable trad_title;
        public Graphic body_background;
        [HideInInspector] public RectTransform rT;
        public Button button_hide, button_fullscreen, button_close;

        [SerializeField, Range(0, 1)] protected float anim_alpha = 1;

        [SerializeField, Range(0, 1)] float ui_hue_start, ui_hue_current;
        [SerializeField] float ui_alpha;

        Rect init_rect;
        protected readonly OnValue<bool> fullscreen = new();

        //--------------------------------------------------------------------------------------------------------------

        void AwakeUI()
        {
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();

            rT = (RectTransform)transform.Find("rT");
            init_rect = rT.rect;

            trad_title = transform.Find("rT/header/title").GetComponent<Traductable>();
            body_background = transform.Find("rT/body/background").GetComponent<Graphic>();
            ui_alpha = body_background.color.a;
            ui_hue_start = body_background.color.GetHue();

            button_hide = transform.Find("rT/header/buttons/hide/button").GetComponent<Button>();
            button_fullscreen = transform.Find("rT/header/buttons/fullscreen/button").GetComponent<Button>();
            button_close = transform.Find("rT/header/buttons/close/button").GetComponent<Button>();

            fullscreen.AddListener(toggle =>
            {
                if (toggle)
                {
                    rT.sizeDelta = Vector2.zero;
                    rT.anchorMin = Vector2.zero;
                    rT.anchorMax = Vector2.one;
                }
                else
                {
                    rT.sizeDelta = init_rect.size;
                    rT.anchorMin = new Vector2(.5f, .5f);
                    rT.anchorMax = new Vector2(.5f, .5f);
                }
            });

            button_fullscreen.onClick.AddListener(fullscreen.Toggle);
        }

        //--------------------------------------------------------------------------------------------------------------

        void UpdateHue()
        {
            const float ui_hue_speed = .03f;
            ui_hue_current = (ui_hue_start + Time.unscaledTime * ui_hue_speed) % 1;
            body_background.color = body_background.color.ModifyHsv(ui_hue_current, ui_alpha * anim_alpha);
        }

        protected virtual void OnUpdateAlpha()
        {
        }
    }
}