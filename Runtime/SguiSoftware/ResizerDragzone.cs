using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    class ResizerDragzone : ArkComponent1
    {
        public SguiSoftware window;
        [SerializeField] internal DIRS_FLAGS direction;
        [SerializeField] internal bool hover_b, drag_b;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            window = GetComponentInParent<SguiSoftware>();
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

                RectTransform resizeRt = ResizerVisual.instance.rt;
                if (resizeRt.parent is not RectTransform resizeSpace
                    || !ArkUI.instance.ScreenDeltaToLocal(resizeSpace, eventData.position, eventData.delta, eventData.pressEventCamera, out Vector2 localDelta))
                    return;

                {
                    Vector2 pos = resizeRt.anchoredPosition;

                    if ((direction & (DIRS_FLAGS.Top | DIRS_FLAGS.Down)) != 0)
                        pos.y += .5f * localDelta.y;

                    if ((direction & (DIRS_FLAGS.Left | DIRS_FLAGS.Right)) != 0)
                        pos.x += .5f * localDelta.x;

                    resizeRt.anchoredPosition = pos;
                }

                {
                    Rect r = resizeRt.rect;

                    if (direction.HasFlag(DIRS_FLAGS.Top))
                        r.yMax += localDelta.y;

                    if (direction.HasFlag(DIRS_FLAGS.Right))
                        r.xMax += localDelta.x;

                    if (direction.HasFlag(DIRS_FLAGS.Left))
                        r.xMin += localDelta.x;

                    if (direction.HasFlag(DIRS_FLAGS.Down))
                        r.yMin += localDelta.y;

                    resizeRt.sizeDelta = r.size;
                    resizeRt.anchorMin = resizeRt.anchorMax = .5f * Vector2.one;
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

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDisable()
        {
            drag_b = false;
            ResizerVisual.instance?.UntakeFocus(this);
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            drag_b = false;
            ResizerVisual.instance?.UntakeFocus(this);
            base.OnDestroy();
        }
    }
}
