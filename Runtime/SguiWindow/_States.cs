﻿using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow : IOnStateMachine
    {
        public enum AnimLayers : byte { Base, _last_ }

        public enum BaseStates
        {
            Default = 1613679313,
            Active = -397331179,
            toActive = -738518269,
            fromActive_ = -572966423,
        }

        [Header("~@ States @~")]
        public BaseStates state_base;

        //--------------------------------------------------------------------------------------------------------------

        public virtual void OnStateMachine(in AnimatorStateInfo stateInfo, in int layerIndex, in bool onEnter)
        {
            if (onEnter)
                switch ((AnimLayers)layerIndex)
                {
                    case AnimLayers.Base:
                        {
                            NUCLEOR.delegates.onLateUpdate -= OnUpdateAlpha;

                            BaseStates state = (BaseStates)stateInfo.fullPathHash;

                            switch (state)
                            {
                                case BaseStates.Default:
                                    if (oblivionized)
                                        Destroy(gameObject);
                                    else
                                    {
                                        animator.Update(0);
                                        animator.enabled = false;
                                        OnUpdateAlpha();
                                        gameObject.SetActive(false);
                                    }
                                    break;

                                case BaseStates.toActive:
                                case BaseStates.fromActive_:
                                    NUCLEOR.delegates.onLateUpdate += OnUpdateAlpha;
                                    break;

                                case BaseStates.Active:
                                    animator.enabled = true;
                                    animator.Update(0);
                                    animator.enabled = false;
                                    OnUpdateAlpha();
                                    break;
                            }

                            onState?.Invoke(state, onEnter);
                            onState_once?.Invoke(state, onEnter);
                            onState_once = null;

                            state_base = state;
                        }
                        break;
                }
        }
    }
}