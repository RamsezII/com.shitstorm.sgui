using _UTIL_;
using _ARK_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiColorPrompt
    {
        void StartDisk()
        {
            RectTransform rt_disk = (RectTransform)transform.Find("rt/graphic/disk");
            rt_disk.GetComponent<PointerClickHandler>().onPointerDown += Drag;
            rt_disk.GetComponent<DragHandler>().onDrag += Drag;

            void Drag(PointerEventData eventData)
            {
                if (!ArkUI.instance.ScreenPointToLocalPoint(rt_disk, eventData.position, eventData.pressEventCamera, out Vector2 lpos))
                    return;

                float angle = Mathf.Atan2(lpos.y, lpos.x);
                if (angle < 0)
                    angle += 2 * Mathf.PI;

                float hue = angle / (2 * Mathf.PI);

                Color.RGBToHSV(ReadFromSliders(), out _, out float s, out float v);
                Color newColor = Color.HSVToRGB(hue, s, v);
                newColor.a = color.a;
                SetNewColor(newColor);
            }
        }
    }
}
