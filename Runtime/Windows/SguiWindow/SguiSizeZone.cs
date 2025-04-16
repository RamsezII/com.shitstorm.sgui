using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    internal class SguiWindowSizeZone : SguiZone, IDragHandler
    {
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Debug.Log(eventData.delta);
        }
    }
}