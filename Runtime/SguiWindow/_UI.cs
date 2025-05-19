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

        public RectTransform buttons_rt;
        public Button button_hide, button_fullscreen, button_close;

        [SerializeField, Range(0, 1)] protected float anim_alpha = 1;

        [SerializeField, Range(0, 1)] float ui_hue_start, ui_hue_current;
        [SerializeField] float ui_alpha;

        [SerializeField] internal Button_Folder prefab_hierarchy_folder;
        [SerializeField] internal Button_File prefab_hierarchy_file;

        SguiRect rect_current;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeUI()
        {
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
            rT = (RectTransform)transform.Find("rT");
            rT_parent = (RectTransform)rT.parent;

            rect_current = new(rT);
        }

        //--------------------------------------------------------------------------------------------------------------

        void StartUI()
        {
            if (prefab_hierarchy_folder != null)
                prefab_hierarchy_folder.gameObject.SetActive(false);
            if (prefab_hierarchy_file != null)
                prefab_hierarchy_file.gameObject.SetActive(false);

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
                        rect_current = new(rT);
                        rT.sizeDelta = Vector2.zero;
                        rT.anchorMin = Vector2.zero;
                        rT.anchorMax = Vector2.one;
                        rT.anchoredPosition = Vector2.zero;
                    }
                    else
                        rect_current.Apply(rT);
                });

                button_fullscreen.onClick.AddListener(fullscreen.Toggle);
            }

            button_hide?.onClick.AddListener(() => ToggleWindow(false));
            button_close?.onClick.AddListener(Oblivionize);
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