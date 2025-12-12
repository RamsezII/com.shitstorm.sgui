using _ARK_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiWindow
    {
        [HideInInspector] public Canvas canvas;
        [HideInInspector] public GraphicRaycaster raycaster;
        [HideInInspector] public RectTransform rt, rt_parent;

        public Traductable trad_title;
        public Graphic huable_background;

        public RectTransform buttons_rt;
        public Button button_close;

        [SerializeField, Range(0, 1)] protected float anim_alpha = 1;

        [SerializeField, Range(0, 1)] float ui_hue_start, ui_hue_current;
        [SerializeField] float ui_alpha;

        [SerializeField] internal Button_Folder prefab_hierarchy_folder;
        [SerializeField] internal Button_File prefab_hierarchy_file;

        protected SguiRect rect_current;

        bool startedHue;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeUI()
        {
            canvas = GetComponentInParent<Canvas>();
            raycaster = GetComponentInParent<GraphicRaycaster>();

            rt = (RectTransform)transform.Find("rT");
            rt_parent = (RectTransform)rt.parent;

            rect_current = new(rt);

            trad_title = transform.Find("rT/header/title").GetComponent<Traductable>();

            ui_hue_start = Random.Range(0f, 1f);
        }

        //--------------------------------------------------------------------------------------------------------------

        void StartUI()
        {
            prefab_hierarchy_folder?.gameObject.SetActive(false);
            prefab_hierarchy_file?.gameObject.SetActive(false);

            if (animate_hue)
                NUCLEOR.delegates.LateUpdate_onEndOfFrame_once += () =>
                {
                    ui_alpha = huable_background.color.a;
                    startedHue = true;
                };

            button_close?.onClick.AddListener(OnClickClose);
        }

        protected void OnClickClose()
        {
            if (!oblivionized)
                if (onFunc_close != null && !onFunc_close())
                    return;
            onAction_close?.Invoke();
            Oblivionize();
        }

        //--------------------------------------------------------------------------------------------------------------

        void UpdateHue()
        {
            if (!startedHue)
                return;
            const float ui_hue_speed = .03f;
            ui_hue_current = (ui_hue_start + Time.unscaledTime * ui_hue_speed) % 1;
            huable_background.color = huable_background.color.ModifyHsv(ui_hue_current, ui_alpha * anim_alpha);
        }

        protected virtual void OnUpdateAlpha()
        {
        }
    }
}