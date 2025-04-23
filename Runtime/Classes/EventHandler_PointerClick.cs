using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed class EventHandler_PointerClick : MonoBehaviour, IPointerClickHandler
    {
        public Action<PointerEventData> onClick;

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(eventData);
        }
    }
}