using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    internal partial class Dragzone : Button, SguiDragManager.IAcceptDraggable
    {
        SguiSplitView pview;
        [SerializeField] internal Dragzone opposite_dragzone;
        [SerializeField] internal RectTransform rt_zone;
        [SerializeField] SelectionState previousState;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            pview = GetComponentInParent<SguiSplitView>(includeInactive: true);
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            OnState(SelectionState.Normal);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnState(SelectionState.Normal);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (previousState == state)
                return;
            previousState = state;

            OnState(state);
        }

        void OnState(SelectionState state)
        {
            rt_zone.gameObject.SetActive(state == SelectionState.Highlighted);
        }

        bool SguiDragManager.IAcceptDraggable.TryAcceptDraggable(in PointerEventData eventData, in SguiDragManager.IDraggable draggable, in bool onDrop)
        {
            if (draggable is TabHeader tab)
            {
                if (onDrop)
                    pview.OnDropTab(this, tab);
                return true;
            }
            return false;
        }
    }
}