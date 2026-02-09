using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiColorPrompt
    {
        void StartSquare()
        {
            RectTransform rt_clickable = (RectTransform)rt_square.Find("background");

            rt_clickable.GetComponent<PointerClickHandler>().onPointerDown += Drag;
            rt_clickable.GetComponent<DragHandler>().onDrag += Drag;

            void Drag(PointerEventData eventData)
            {
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt_square, eventData.position, eventData.pressEventCamera, out Vector2 lpos))
                    return;

                float saturation = Mathf.Clamp01((lpos.x + rt_square.rect.width / 2) / rt_square.rect.width);
                float value = Mathf.Clamp01((lpos.y + rt_square.rect.height / 2) / rt_square.rect.height);
                Color.RGBToHSV(color, out float h, out _, out _);
                SetColor(Color.HSVToRGB(h, saturation, value));
            }
        }
    }
}