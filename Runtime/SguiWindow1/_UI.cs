using _UTIL_;
using UnityEngine.UI;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow1
    {
        [HideInInspector] public Canvas canvas;
        [HideInInspector] public GraphicRaycaster raycaster;
        [HideInInspector] public RectTransform rT, rT_parent;

        public Traductable trad_title;
        public Graphic body_background;
        public Button button_hide, button_fullscreen, button_close;
        internal SguiZone zone_drag, zone_size;

        internal HeaderDropdown dropdown_help;

        [SerializeField, Range(0, 1)] protected float anim_alpha = 1;

        [SerializeField, Range(0, 1)] float ui_hue_start, ui_hue_current;
        [SerializeField] float ui_alpha;

        Rect init_rect;
        public readonly OnValue<bool> fullscreen = new();

        //--------------------------------------------------------------------------------------------------------------

        void AwakeUI()
        {
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
            rT = (RectTransform)transform.Find("rT");
            rT_parent = (RectTransform)rT.parent;

            init_rect = rT.rect;

            trad_title = transform.Find("rT/header/title").GetComponent<Traductable>();
            body_background = transform.Find("rT/body/background").GetComponent<Graphic>();
            ui_alpha = body_background.color.a;
            ui_hue_start = body_background.color.GetHue();

            button_hide = transform.Find("rT/header/buttons/hide/button").GetComponent<Button>();
            button_fullscreen = transform.Find("rT/header/buttons/fullscreen/button").GetComponent<Button>();
            button_close = transform.Find("rT/header/buttons/close/button").GetComponent<Button>();

            zone_drag = transform.Find("rT/header/gradient").GetComponent<SguiZone>();
            zone_size = transform.Find("rT/selected").GetComponent<SguiZone>();

            dropdown_help = transform.Find("rT/buttons/layout/button_Help")?.GetComponent<HeaderDropdown>();

            if (dropdown_help != null)
                dropdown_help.onItemClick += OnClickDropdown_Help;

            zone_drag.onDragDelta += OnHeaderDrag;
            zone_size.onDragBegin += OnSizeDrag_begin;
            zone_size.onDragDelta += OnSizeDrag;

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

            button_close.onClick.AddListener(() => sgui_toggle_window.Update(false));
            button_hide.onClick.AddListener(() => sgui_toggle_window.Update(false));
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