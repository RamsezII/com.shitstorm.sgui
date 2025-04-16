using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    internal abstract class SguiZone : MonoBehaviour, IPointerClickHandler
    {
        const float doubleckick_time = .3f;
        float doubleclick_last;

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (Time.unscaledTime - doubleclick_last <= doubleckick_time)
                Debug.Log("Double clic!");
            doubleclick_last = Time.unscaledTime;
        }
    }
}