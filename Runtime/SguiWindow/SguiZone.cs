using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    internal class SguiZone : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
    {
        public SguiWindow window;

        const float doubleckick_time = .3f;
        float doubleclick_last;

        public Action<Vector2> onDragBegin, onDragDelta;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            window = GetComponentInParent<SguiWindow>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (Time.unscaledTime - doubleclick_last <= doubleckick_time)
                window.fullscreen.Toggle();
            doubleclick_last = Time.unscaledTime;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            onDragDelta?.Invoke(eventData.delta);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            onDragBegin?.Invoke(eventData.position);
        }
    }
}