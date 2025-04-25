using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    internal class SguiZone : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum Codes : byte
        {
            None,
            Click,
            DoubleClick,
            Enter,
            Move,
            BeginDrag,
            Drag,
            EndDrag,
            Exit,
        }

        public SguiWindow window;

        const float doubleckick_time = .3f;
        float doubleclick_last;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            window = GetComponentInParent<SguiWindow>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (Time.unscaledTime - doubleclick_last <= doubleckick_time)
            {
                window.fullscreen.Toggle();
                window.OnZoneEvent(Codes.DoubleClick, this, eventData);
            }
            else
                window.OnZoneEvent(Codes.Click, this, eventData);

            doubleclick_last = Time.unscaledTime;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            window.OnZoneEvent(Codes.Enter, this, eventData);
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            window.OnZoneEvent(Codes.Move, this, eventData);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            window.OnZoneEvent(Codes.Exit, this, eventData);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            window.OnZoneEvent(Codes.BeginDrag, this, eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            window.OnZoneEvent(Codes.Drag, this, eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            window.OnZoneEvent(Codes.EndDrag, this, eventData);
        }
    }
}