using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiCursor : IOnStateMachine
    {
        public enum AnimLayers : byte { Base, _last_ }

        public enum BaseStates
        {
            Default = 1613679313,
            Off = 1398980678,
            Enable = 1357840677,
            _Enable = 1106434325,
        }

        [Header("~@ States @~")]
        public BaseStates state_base;

        //--------------------------------------------------------------------------------------------------------------
        void IOnStateMachine.OnStateMachine(in AnimatorStateInfo stateInfo, in int layerIndex, in bool onEnter)
        {
            if (onEnter)
                switch ((AnimLayers)layerIndex)
                {
                    case AnimLayers.Base:
                        state_base = (BaseStates)stateInfo.fullPathHash;
                        break;
                }
        }
    }
}