using _ARK_;
using _SGUI_.context_click;
using _SGUI_.window1;
using _UTIL_;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public abstract partial class SguiWindow1 : SguiWindow
    {
        public Button button_hide, button_fullscreen;
        [SerializeField] HeaderButton prefab_headerbutton;
        [SerializeField] ResizerDragzone resizer_dragzone;
        [SerializeField] RectTransform rt_unselected;

        public readonly ValueHandler<bool> fullscreen = new();

        public const int
            min_width = 200,
            min_height = 150;

        public HeaderButton AddHeaderButton() => prefab_headerbutton.Clone(true);

        public static Action<SguiWindow1, ContextList> onHeaderButtonContextList_settings, onHeaderButtonContextList_help;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            onHeaderButtonContextList_settings = onHeaderButtonContextList_help = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            huable_background = transform.Find("rT/body/background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)transform.Find("rT/header/buttons");
            button_hide = buttons_rt.Find("hide/button").GetComponent<Button>();
            button_fullscreen = buttons_rt.Find("fullscreen/button").GetComponent<Button>();
            button_close = buttons_rt.Find("close/button").GetComponent<Button>();

            prefab_headerbutton = GetComponentInChildren<HeaderButton>(true);

            resizer_dragzone = transform.Find("rT/_SGUI_.ResizerDragzone").GetComponent<ResizerDragzone>();

            rt_unselected = (RectTransform)transform.Find("rT/unselected");

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

            prefab_headerbutton.gameObject.SetActive(false);

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

            if (onHeaderButtonContextList_help != null)
            {
                var button = AddHeaderButton();
                button.transform.SetAsFirstSibling();
                button.trad.SetTrads(new()
                {
                    french = "Aide",
                    english = "Help",
                });
                button.onContextList = list => onHeaderButtonContextList_help(this, list);
            }

            if (onHeaderButtonContextList_settings != null)
            {
                var button = AddHeaderButton();
                button.transform.SetAsFirstSibling();
                button.trad.SetTrads(new()
                {
                    french = "Réglages",
                    english = "Settings",
                });
                button.onContextList = list => onHeaderButtonContextList_settings(this, list);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnFocus(in bool has_focus)
        {
            base.OnFocus(has_focus);
            rt_unselected.gameObject.SetActive(!has_focus);
        }

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
            Vector2 maxsize = rt_parent.rect.size;
            Vector2 minsize = new(min_width, min_height);
            Vector2 size = rt.rect.size;

            size = new(
                Mathf.Clamp(size.x, minsize.x, maxsize.x),
                Mathf.Clamp(size.y, minsize.y, maxsize.y)
            );

            rt.sizeDelta = size;

            CheckPosition(out _);
        }

        public virtual void OnResized()
        {
        }
    }
}