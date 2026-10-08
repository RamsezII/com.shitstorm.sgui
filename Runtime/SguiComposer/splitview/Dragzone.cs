using UnityEngine;
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
            rt_zone.gameObject.SetActive(previousState == SelectionState.Highlighted);
        }

        protected override void OnDisable()
        {
            rt_zone.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (previousState == state)
                return;
            previousState = state;

            rt_zone.gameObject.SetActive(state == SelectionState.Highlighted);
        }

        bool SguiDragManager.IAcceptDraggable.TryAcceptDraggable(in SguiDragManager.IDraggable draggable, in bool onDrop)
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