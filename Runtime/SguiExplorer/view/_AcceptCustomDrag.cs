using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiExplorerView : SguiDragManager.IAcceptDraggable
    {
        [AutoStaticsCleanup] public static readonly List<Func<PointerEventData, SguiDragManager.IDraggable, bool, bool>> onTryAcceptDraggable = new();

        //--------------------------------------------------------------------------------------------------------------

        bool SguiDragManager.IAcceptDraggable.TryAcceptDraggable(in PointerEventData eventData, in SguiDragManager.IDraggable draggable, in bool onDrop)
        {
            for (int i = onTryAcceptDraggable.Count - 1; i >= 0; i--)
            {
                var callback = onTryAcceptDraggable[i];
                if (callback(eventData, draggable, onDrop))
                    return true;
            }
            return false;
        }
    }
}