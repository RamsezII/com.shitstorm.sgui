using _ARK_;
using _SGUI_.composer;
using _UTIL_;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiComposer : SguiWindow
    {
        public readonly ValueNotifier<bool> fullscreen = new();
        [SerializeField] RectTransform rt_body, rt_unselected;
        [SerializeField] Button button_hide, button_fullscreen;
        bool closing, close_requested;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();

            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.AddElement(OnImguiInputs);
        }

        protected override void OnDisable()
        {
            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);

            RefreshFrameFocus();

            base.OnDisable();
        }

        //--------------------------------------------------------------------------------------------------------------

        internal protected override void OnInitialize()
        {
            base.OnInitialize();

            splitviews = GetComponentsInChildren<SguiSplitView>(true);

            foreach (var view in splitviews)
                view.Initialize();

            active_frame.AddListener(_ => RefreshFrameFocus());
            fullscreen.AddListener(OnFullscreen, do_not_call_this_time: true);
            onFunc_close = RequestCloseFrames;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            var drag = rt.Find("header/header_mask/padding/drag-button").GetComponent<DragHandler>();
            drag.onBeginDrag += OnHeaderBeginDrag;
            drag.onDrag += OnHeaderDrag;
            drag.onEndDrag += OnHeaderEndDrag;

            rt.Find("header/header_mask/padding/drag-button").GetComponent<PointerClickHandler>().onClick += data =>
            {
                if (data.clickCount == 2)
                    fullscreen.ToggleAuto();
            };

            button_fullscreen.onClick.AddListener(fullscreen.ToggleAuto);

            button_hide.onClick.AddListener(() =>
            {
                SetScalePivot(active_frame._value.os_button);
                toggle.Value = false;
            });

            toggle.AddListener(value =>
            {
                if (!value)
                    ResizerVisual.instance?.UntakeFocus(this);

                RefreshFrameFocus();
            });

            CheckBounds();

#if UNITY_EDITOR
            ArkUI._VisibleInEditor.Add(gameObject);
#endif
        }

        //--------------------------------------------------------------------------------------------------------------

        bool OnImguiInputs(Event e)
        {
            if (!isFocused._value || !toggle._value || e.type != EventType.KeyDown)
                return false;

            if (e.keyCode == KeyCode.F10)
            {
                CheckBounds();
                return true;
            }

            if (e.keyCode == KeyCode.F11)
            {
                fullscreen.ToggleAuto();
                return true;
            }

            return false;
        }

        void OnFullscreen(bool value)
        {
            if (value)
            {
                rect_current = new(rt);
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.sizeDelta = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;
            }
            else
                rect_current.Apply(rt);

            OnResized();
        }

        protected override void OnToggleFocus(bool focus)
        {
            base.OnToggleFocus(focus);

            rt_unselected.gameObject.SetActive(!focus);
            RefreshFrameFocus();
        }

        public void CheckBounds()
        {
            if (!fullscreen._value)
            {
                const int min_width = 200, min_height = 150;

                var available = rt_root.rect.size;
                var size = rt.rect.size;

                rt.sizeDelta = new(Mathf.Clamp(size.x, Mathf.Min(min_width, available.x), available.x), Mathf.Clamp(size.y, Mathf.Min(min_height, available.y), available.y));
            }
            CheckPosition(out _);
            OnResized();
        }

        public void OnResized()
        {
            Canvas.ForceUpdateCanvases();
            foreach (var view in splitviews)
                if (view != null)
                    foreach (var frame in view.frames)
                        if (frame != null && frame.isActiveAndEnabled) frame.OnResized();
        }

        protected override void OnHeaderEndDrag(PointerEventData eventData)
        {
            base.OnHeaderEndDrag(eventData);
            OnResized();
        }

        internal void CancelCloseRequest() => close_requested = false;

        bool RequestCloseFrames()
        {
            close_requested = true;
            closing = true;
            try
            {
                foreach (var frame in splitviews.Where(v => v != null).SelectMany(v => v.frames).ToArray())
                    if (frame != null && !frame.RequestClose())
                        return false;
                return true;
            }
            finally
            {
                closing = false;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnOblivion()
        {
            foreach (var frame in splitviews.Where(v => v != null).SelectMany(v => v.frames).ToArray())
                if (frame != null)
                    frame.Oblivionize();

            active_frame.Value = null;

            base.OnOblivion();
        }

        protected override void OnDestroy()
        {
            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);

            base.OnDestroy();

            active_frame.Clear();
            fullscreen.Clear();

#if UNITY_EDITOR
            ArkUI._VisibleInEditor.Remove(gameObject);
#endif
        }
    }
}
