using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.osview
{
    public sealed partial class BackgroundReceiver : MonoBehaviour, IPointerClickHandler, SguiDragManager.IAcceptDraggable
    {
        [SerializeField] internal RaycastReceiver raycastReceiver;
        public Action<PointerEventData> onPointerClick_data;
        public List<Func<PointerEventData, SguiDragManager.IDraggable, bool, bool>> stack_onTryAcceptDraggable = new();

        //----------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onPointerClick_data?.Invoke(eventData);
        }

        bool SguiDragManager.IAcceptDraggable.TryAcceptDraggable(in PointerEventData eventData, in SguiDragManager.IDraggable draggable, in bool onDrop)
        {
            foreach (var callback in stack_onTryAcceptDraggable)
                if (callback(eventData, draggable, onDrop))
                    return true;
            return false;
        }
    }
}