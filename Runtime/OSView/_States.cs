using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class OSView : IOnStateMachine
    {
        public enum AnimLayers : byte { Base, _last_ }

        public enum BaseStates
        {
            Default = 1613679313,
            Enable = 1357840677,
            Enable_ = 1652725934,
        }

        [Header("~@ States @~")]
        public BaseStates state_base;

        //--------------------------------------------------------------------------------------------------------------

        void IOnStateMachine.OnStateMachine(in AnimatorStateInfo stateInfo, in int layerIndex, in bool onEnter)
        {
            switch ((AnimLayers)layerIndex)
            {
                case AnimLayers.Base:
                    {
                        BaseStates state = (BaseStates)stateInfo.fullPathHash;

                        switch (state)
                        {
                            case BaseStates.Enable:
                                RefreshDatetime();
                                UsageManager.AddUser(this, UsageGroups.BlockPlayer, UsageGroups.TrueMouse);
                                break;

                            default:
                                UsageManager.RemoveUser(this);
                                break;
                        }

                        if (onEnter)
                        {
                            state_base = state;
                            ResizeSguiGlobal2DrT();
                        }
                    }
                    break;
            }
        }
    }
}