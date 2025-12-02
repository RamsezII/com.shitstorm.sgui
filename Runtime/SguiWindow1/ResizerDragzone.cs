using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    class ResizerDragzone : MonoBehaviour
    {
        public SguiWindow1 window;
        [SerializeField] internal DIRS_FLAGS direction;
        [SerializeField] internal bool hover_b, drag_b;
        readonly ValueHandler<bool> resize_visible = new();
        Vector2 dragVector, save_min, save_max;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            window = GetComponentInParent<SguiWindow1>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            resize_visible.AddListener(value => window.resizer_visual.current_zones.ToggleElement(this, value));

            var click_handler = transform.GetComponent<PointerClickHandler>();
            var drag_handler = transform.GetComponent<DragHandler>();
            var hover_handler = transform.GetComponent<PointerEnterExitHandler>();
            var move_handler = transform.GetComponent<PointerMoveHandler>();

            hover_handler.onEnterExit += (PointerEventData eventData, bool onEnter) =>
            {
                if (eventData.dragging)
                    return;

                hover_b = onEnter;

                if (onEnter)
                {
                    window.resizer_visual.TryFocusZone(this);

                    window.resizer_visual.rt.position = window.rt.position;
                    window.resizer_visual.rt.sizeDelta = window.rt.sizeDelta;
                    window.resizer_visual.rt.anchorMin = window.resizer_visual.rt.anchorMax = .5f * Vector2.one;
                }
                else
                    window.resizer_visual.UnfocusZone(this);

                RefreshVisibility();
            };

            move_handler.onMove += (PointerEventData eventData) =>
            {
                if (eventData.dragging)
                    return;

                window.resizer_visual.TryFocusZone(this);
            };

            click_handler.onPointerDown += (PointerEventData eventData) =>
            {
                dragVector = Vector2.zero;
                window.rt.GetWorldCorners(out save_min, out save_max);
            };

            drag_handler.onBeginDrag += (PointerEventData eventData) =>
            {
                drag_b = true;
                RefreshVisibility();
            };

            drag_handler.onDrag += (PointerEventData eventData) =>
            {
                dragVector += eventData.delta;

                Vector2 min = save_min;
                Vector2 max = save_max;

                if (direction.HasFlag(DIRS_FLAGS.Top))
                    max.y = save_max.y + dragVector.y;

                if (direction.HasFlag(DIRS_FLAGS.Right))
                    max.x = save_max.x + dragVector.x;

                if (direction.HasFlag(DIRS_FLAGS.Left))
                    min.x = save_min.x + dragVector.x;

                if (direction.HasFlag(DIRS_FLAGS.Down))
                    min.y = save_min.y + dragVector.y;

                window.resizer_visual.rt.position = .5f * (min + max);
                window.resizer_visual.rt.sizeDelta = .5f * (max - min);
            };

            drag_handler.onEndDrag += (PointerEventData eventData) =>
            {
                drag_b = false;

                window.rt.position = window.resizer_visual.rt.position;
                window.rt.sizeDelta = window.resizer_visual.rt.sizeDelta;
                window.rt.anchorMin = window.rt.anchorMax = .5f * Vector2.one;

                window.CheckBounds();
                window.OnResized();
                RefreshVisibility();
            };

            void RefreshVisibility()
            {
                resize_visible.Value = hover_b || drag_b;
            }
        }
    }
}