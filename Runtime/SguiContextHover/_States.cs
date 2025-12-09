using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiContextHover : IOnStateMachine
    {
        enum AnimLayers : byte { Base, _last_ }

        enum BaseStates
        {
            Default = 1613679313,
            Enable = 1357840677,
        }

        [Header("~@ States @~")]
        [SerializeField] BaseStates state_base;

        //--------------------------------------------------------------------------------------------------------------

        void IOnStateMachine.OnStateMachine(in AnimatorStateInfo stateInfo, in int layerIndex, in bool onEnter)
        {
            if (onEnter)
                if (layerIndex == 0)
                    state_base = (BaseStates)stateInfo.fullPathHash;
        }
    }
}