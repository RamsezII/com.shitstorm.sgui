using UnityEngine;

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

        bool SguiDragManager.IAcceptDraggable.TryAcceptDraggable(in SguiDragManager.IDraggable draggable, in bool onDrop)
        {
            if (draggable is TabHeader newTab)
            {
                if (onDrop)
                    ;
                return true;
            }
            return false;
        }

        void OnTabBeingDragged(TabHeader draggedTab)
        {
            gameObject.SetActive(draggedTab != null);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            SguiSplitView.current_drag.RemoveListener(OnTabBeingDragged);
        }
    }
}