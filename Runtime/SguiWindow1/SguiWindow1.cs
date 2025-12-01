using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public abstract partial class SguiWindow1 : SguiWindow
    {
        public Button button_hide, button_fullscreen;

        public readonly ValueHandler<bool> fullscreen = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            huable_background = transform.Find("rT/body/background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)transform.Find("rT/header/buttons");
            button_hide = buttons_rt.Find("hide/button").GetComponent<Button>();
            button_fullscreen = buttons_rt.Find("fullscreen/button").GetComponent<Button>();
            button_close = buttons_rt.Find("close/button").GetComponent<Button>();

            dropdown_settings = transform.Find("rT/buttons/layout/button_Settings")?.GetComponent<HeaderDropdown>();
            if (dropdown_settings != null)
                dropdown_settings.onItemClick += OnClickDropdown_Settings;

            dropdown_help = transform.Find("rT/buttons/layout/button_Help")?.GetComponent<HeaderDropdown>();
            if (dropdown_help != null)
                dropdown_help.onItemClick += OnClickDropdown_Help;

            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            IMGUI_global.instance.users_inputs.AddElement(OnImguiInputs, this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            IMGUI_global.instance.users_inputs.RemoveKey(OnImguiInputs);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            DragHandler drag_handler = transform.Find("rT/header/header_mask/padding/drag-button").GetComponent<DragHandler>();
            drag_handler.onBeginDrag += OnHeaderBeginDrag;
            drag_handler.onDrag += OnHeaderDrag;
            drag_handler.onEndDrag += OnHeaderEndDrag;

            StartResize();

            button_hide.onClick.AddListener(() =>
            {
                SetScalePivot(os_button);
                ToggleWindow(false);
            });

            OnPopulateDropdowns();
        }

        //--------------------------------------------------------------------------------------------------------------

        bool OnImguiInputs(Event e)
        {
            if (e.type == EventType.KeyDown)
                if (e.keyCode == KeyCode.F11)
                {
                    fullscreen.Toggle();
                    return true;
                }
            return false;
        }
    }
}