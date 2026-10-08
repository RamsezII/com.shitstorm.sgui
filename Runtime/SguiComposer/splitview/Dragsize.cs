using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_.composer
{
    internal sealed partial class Dragsize : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        //--------------------------------------------------------------------------------------------------------------

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
        }
    }
}