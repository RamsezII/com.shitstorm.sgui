using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class OSMainMenu : IOnStateMachine
    {
        public enum AnimLayers : byte { Base, _last_ }

        public enum BaseStates
        {
            Init = -1234776474,
            Enable = 1357840677,
            Enable_ = 1652725934,
            Off = 1398980678,
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
                        {
                            BaseStates state = (BaseStates)stateInfo.fullPathHash;
                            switch (state)
                            {
                                case BaseStates.Off:
                                    gameObject.SetActive(false);
                                    break;
                            }
                            state_base = state;
                        }
                        break;
                }
        }
    }
}