using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiWindow
    {


        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnHeaderBeginDrag(PointerEventData eventData)
        {

        }

        protected virtual void OnHeaderDrag(PointerEventData eventData)
        {
            if (this is SguiWindow1 window1 && window1.fullscreen._value)
                return;

            Vector2 delta = eventData.delta;
            rT.position += (Vector3)delta;

            CheckPosition();
        }

        protected virtual void OnHeaderEndDrag(PointerEventData eventData)
        {

        }

        public void CheckPosition()
        {
            rT.GetWorldCorners(out Vector2 min, out Vector2 max);
            rT_parent.GetWorldCorners(out Vector2 p_min, out Vector2 p_max);

            if (Util.BoundsClamp(min, max, p_min, p_max, out Vector3 correction))
                rT.position += correction;
        }
    }
}