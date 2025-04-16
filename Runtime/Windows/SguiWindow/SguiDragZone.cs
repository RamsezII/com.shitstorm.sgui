using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    internal class SguiWindowDragZone : SguiZone, IDragHandler
    {
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Debug.Log(eventData.delta);
        }
    }
}