using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_.composer
{
    internal partial class TabInsertDrop : MonoBehaviour, SguiDragManager.IAcceptDraggable
    {
        TabHeader parentTab;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            parentTab = GetComponentInParent<TabHeader>(includeInactive: true);
            SguiSplitView.current_drag.AddListener(OnTabBeingDragged);
            SguiLoggerOverlay.Log(transform.GetPath(true), this);
        }

        //--------------------------------------------------------------------------------------------------------------

        bool SguiDragManager.IAcceptDraggable.TryAcceptDraggable(in PointerEventData eventData, in SguiDragManager.IDraggable draggable, in bool onDrop)
        {
            if (draggable is TabHeader newTab && newTab != parentTab)
            {
                if (onDrop)
                    parentTab.pview.MoveTabHere(newTab, parentTab, after: this == parentTab.insert_R);
                return true;
            }
            return false;
        }

        void OnTabBeingDragged(TabHeader draggedTab)
        {
            gameObject.SetActive(draggedTab != null && draggedTab != parentTab);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            SguiSplitView.current_drag.RemoveListener(OnTabBeingDragged);
        }
    }
}
