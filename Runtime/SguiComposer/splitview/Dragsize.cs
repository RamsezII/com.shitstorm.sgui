using _ARK_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_.composer
{
    internal sealed partial class Dragsize : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        SguiSplitView pview;
        RectTransform rt_group, rt_low, rt_high;
        int axis;
        float drag_ratio, min_low, min_high;
        const float min_size = 80;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            pview = GetComponentInParent<SguiSplitView>(includeInactive: true);
        }

        //--------------------------------------------------------------------------------------------------------------

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            rt_group = rt_low = rt_high = null;
            var handle = (RectTransform)transform;
            Vector2 side = handle.anchorMin + handle.anchorMax - Vector2.one;
            axis = side.x != 0 ? 0 : 1;
            bool highSide = side[axis] > 0;

            for (var branch = (RectTransform)pview.transform; branch.parent is RectTransform parent && parent != pview.composer.rt_body; branch = parent)
            {
                if (parent.childCount != 2 || (highSide ? branch.anchorMax[axis] >= 1 : branch.anchorMin[axis] <= 0))
                    continue;

                var sibling = (RectTransform)parent.GetChild(1 - branch.GetSiblingIndex());
                rt_group = parent;
                rt_low = highSide ? branch : sibling;
                rt_high = highSide ? sibling : branch;
                min_low = MinimumSize(rt_low);
                min_high = MinimumSize(rt_high);
                drag_ratio = rt_low.anchorMax[axis];
                pview.composer.TakeFocus();
                break;
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (rt_group == null || rt_low == null || rt_high == null)
                return;

            float size = rt_group.rect.size[axis];
            if (size <= 0)
                return;
            float minRatio = min_low / size;
            float maxRatio = 1 - min_high / size;
            if (minRatio > maxRatio)
                return;
            if (!ArkUI.instance.ScreenDeltaToLocal(rt_group, eventData.position, eventData.delta, eventData.pressEventCamera, out Vector2 delta))
                return;

            drag_ratio += delta[axis] / size;
            float ratio = Mathf.Clamp(drag_ratio, minRatio, maxRatio);
            if (Mathf.Approximately(ratio, rt_low.anchorMax[axis]))
                return;

            var anchorMax = rt_low.anchorMax;
            var anchorMin = rt_high.anchorMin;
            anchorMax[axis] = anchorMin[axis] = ratio;
            rt_low.anchorMax = anchorMax;
            rt_high.anchorMin = anchorMin;
            pview.composer.OnResized();
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            rt_group = rt_low = rt_high = null;
        }

        void OnDisable() => rt_group = rt_low = rt_high = null;

        //--------------------------------------------------------------------------------------------------------------

        float MinimumSize(RectTransform branch)
        {
            if (branch.TryGetComponent<SguiSplitView>(out _))
                return min_size;

            float minimum = 0;
            for (int i = 0; i < branch.childCount; i++)
            {
                var child = (RectTransform)branch.GetChild(i);
                float fraction = child.anchorMax[axis] - child.anchorMin[axis];
                minimum = Mathf.Max(minimum, MinimumSize(child) / fraction);
            }
            return minimum;
        }
    }
}
