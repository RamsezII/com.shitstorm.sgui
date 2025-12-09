using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    class ResizerVisual : MonoBehaviour
    {
        public SguiWindow1 window;
        public RectTransform rt;

        readonly ValueHandler<ResizerDragzone> current_zone = new();
        internal readonly ListListener<ResizerDragzone> current_zones = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            window = GetComponentInParent<SguiWindow1>();
            rt = (RectTransform)transform;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            current_zones.AddListener1(gameObject.SetActive);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal bool TryFocusZone(in ResizerDragzone zone)
        {
            ResizerDragzone current = current_zone._value;
            if (zone == current || current == null || !current.drag_b)
            {
                current_zone.Value = zone;
                return true;
            }
            return false;
        }

        internal void UnfocusZone(in ResizerDragzone zone)
        {
            if (zone == current_zone._value)
                current_zone.Value = null;
        }
    }
}