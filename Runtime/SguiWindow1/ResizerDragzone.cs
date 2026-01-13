using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    class ResizerDragzone : ArkComponent
    {
        public SguiWindow1 window;
        [SerializeField] internal DIRS_FLAGS direction;
        [SerializeField] internal bool hover_b, drag_b;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            window = GetComponentInParent<SguiWindow1>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            var click_handler = transform.GetComponent<PointerClickHandler>();
            var drag_handler = transform.GetComponent<DragHandler>();
            var hover_handler = transform.GetComponent<PointerEnterExitHandler>();

            hover_handler.onEnterExit += (PointerEventData eventData, bool onEnter) =>
            {
                if (eventData.dragging)
                    return;

                hover_b = onEnter;

                if (!onEnter)
                    ResizerVisual.instance.UntakeFocus(this);
                else
                {
                    ResizerVisual.instance.TakeFocus(this);
                    ApplyWindowDims();
                }
            };

            click_handler.onPointerDown += (PointerEventData eventData) =>
            {
                ApplyWindowDims();
                ResizerVisual.instance.TakeFocus(this);
            };

            drag_handler.onBeginDrag += (PointerEventData eventData) =>
            {
                drag_b = true;
                ResizerVisual.instance.TakeFocus(this);
            };

            drag_handler.onDrag += (PointerEventData eventData) =>
            {
                if (!drag_b)
                    return;

                {
                    Vector2 pos = ResizerVisual.instance.rt.position;

                    if ((direction & (DIRS_FLAGS.Top | DIRS_FLAGS.Down)) != 0)
                        pos.y += .5f * eventData.delta.y;

                    if ((direction & (DIRS_FLAGS.Left | DIRS_FLAGS.Right)) != 0)
                        pos.x += .5f * eventData.delta.x;

                    ResizerVisual.instance.rt.position = pos;
                }

                {
                    SguiGlobal.instance.ScreenPointToLocalPoint(eventData.delta, out Vector2 ldelta);
                    LoggerOverlay.Log(ldelta, this, timer: 0);

                    Rect r = ResizerVisual.instance.rt.rect;

                    if (direction.HasFlag(DIRS_FLAGS.Top))
                        r.yMax += ldelta.y;

                    if (direction.HasFlag(DIRS_FLAGS.Right))
                        r.xMax += ldelta.x;

                    if (direction.HasFlag(DIRS_FLAGS.Left))
                        r.xMin += ldelta.x;

                    if (direction.HasFlag(DIRS_FLAGS.Down))
                        r.yMin += ldelta.y;

                    ResizerVisual.instance.rt.sizeDelta = r.size;
                    ResizerVisual.instance.rt.anchorMin = ResizerVisual.instance.rt.anchorMax = .5f * Vector2.one;
                }
            };

            drag_handler.onEndDrag += (PointerEventData eventData) =>
            {
                drag_b = false;
                ResizerVisual.instance.UntakeFocus(this);

                window.rt.position = ResizerVisual.instance.rt.position;
                window.rt.sizeDelta = ResizerVisual.instance.rt.sizeDelta;
                window.rt.anchorMin = ResizerVisual.instance.rt.anchorMin;
                window.rt.anchorMax = ResizerVisual.instance.rt.anchorMax;

                window.CheckBounds();
                window.OnResized();
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        void ApplyWindowDims()
        {
            ResizerVisual.instance.rt.anchorMin = window.rt.anchorMin;
            ResizerVisual.instance.rt.anchorMax = window.rt.anchorMax;
            ResizerVisual.instance.rt.sizeDelta = window.rt.sizeDelta;
            ResizerVisual.instance.rt.position = window.rt.position;
        }
    }
}