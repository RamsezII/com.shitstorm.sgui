using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiConsole
    {
        [SerializeField] DragHandler drag_ratio;
        readonly ValueNotifier<float> ratio = new();

        //--------------------------------------------------------------------------------------------------------------

        void InitRatio()
        {
            var rt_ratio = (RectTransform)drag_ratio.transform;
            var rt_body = (RectTransform)rt_ratio.parent;
            var rt_list = (RectTransform)scrollview_list.transform;
            var rt_selected = (RectTransform)scrollview_selected.transform;

            ratio.Value = rt_ratio.anchorMin.y;

            ratio.AddListener(value =>
            {
                rt_ratio.anchorMin = new(rt_ratio.anchorMin.x, value);
                rt_ratio.anchorMax = new(rt_ratio.anchorMax.x, value);
                rt_list.anchorMin = new(rt_list.anchorMin.x, value);
                rt_selected.anchorMax = new(rt_selected.anchorMax.x, value);
            });

            drag_ratio.onDrag += (PointerEventData eventData) =>
            {
                if (rt_body.rect.height <= 0 || !ArkUI.instance.ScreenDeltaToLocal(rt_body, eventData.position, eventData.delta, eventData.pressEventCamera, out Vector2 delta))
                    return;

                ratio.Value = Mathf.Clamp(ratio.Value + delta.y / rt_body.rect.height, .1f, .9f);
            };
        }
    }
}