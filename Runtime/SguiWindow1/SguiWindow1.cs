using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public abstract partial class SguiWindow1 : SguiWindow
    {
        public Button button_hide, button_fullscreen;
        [SerializeField] internal ResizerDragzone resizer_dragzone;
        [SerializeField] internal ResizerVisual resizer_visual;

        public readonly ValueHandler<bool> fullscreen = new();

        public const int
            min_width = 300,
            min_height = 250;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            huable_background = transform.Find("rT/body/background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)transform.Find("rT/header/buttons");
            button_hide = buttons_rt.Find("hide/button").GetComponent<Button>();
            button_fullscreen = buttons_rt.Find("fullscreen/button").GetComponent<Button>();
            button_close = buttons_rt.Find("close/button").GetComponent<Button>();

            resizer_dragzone = transform.Find("rT/_SGUI_.ResizerDragzone").GetComponent<ResizerDragzone>();
            resizer_visual = transform.Find("_SGUI_.ResizerVisual").GetComponent<ResizerVisual>();

            dropdown_settings = transform.Find("rT/buttons/layout/button_Settings")?.GetComponent<HeaderDropdown>();
            if (dropdown_settings != null)
                dropdown_settings.onItemClick += OnClickDropdown_Settings;

            dropdown_help = transform.Find("rT/buttons/layout/button_Help")?.GetComponent<HeaderDropdown>();
            if (dropdown_help != null)
                dropdown_help.onItemClick += OnClickDropdown_Help;

            base.OnAwake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            IMGUI_global.instance.inputs_users.AddElement(OnImguiInputs);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            RectTransform header_zone = (RectTransform)transform.Find("rT/header/header_mask/padding/drag-button");

            DragHandler drag_handler = header_zone.GetComponent<DragHandler>();
            drag_handler.onBeginDrag += OnHeaderBeginDrag;
            drag_handler.onDrag += OnHeaderDrag;
            drag_handler.onEndDrag += OnHeaderEndDrag;

            PointerClickHandler click_handler = header_zone.GetComponent<PointerClickHandler>();
            click_handler.onClick += (PointerEventData eventData) =>
            {
                if (eventData.clickCount == 2)
                    fullscreen.Toggle();
            };

            fullscreen.AddListener(toggle =>
            {
                if (toggle)
                {
                    rect_current = new(rt);
                    rt.sizeDelta = Vector2.zero;
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.one;
                    rt.anchoredPosition = Vector2.zero;
                }
                else
                    rect_current.Apply(rt);
                OnResized();
            });

            button_fullscreen.onClick.AddListener(fullscreen.Toggle);

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
                switch (e.keyCode)
                {
                    case KeyCode.F10:
                        CheckBounds();
                        return true;

                    case KeyCode.F11:
                        fullscreen.Toggle();
                        return true;
                }
            return false;
        }

        public void CheckBounds()
        {
            rt_parent.GetWorldCorners(out Vector2 pmin, out Vector2 pmax);
            rt.GetWorldCorners(out Vector2 min, out Vector2 max);

            Vector2 min_dims = new(min_width, min_height);

            for (int i = 0; i < 2; ++i)
            {
                min[i] = Mathf.Clamp(min[i], pmin[i], pmax[i] - min_dims[i]);
                max[i] = Mathf.Clamp(max[i], min[i] + min_dims[i], pmax[i]);
            }

            Vector2 size = .5f * (max - min);
            Vector2 position = .5f * (min + max);

            rt.sizeDelta = size;
            rt.position = position;
        }

        public virtual void OnResized()
        {
        }
    }
}