using _UTIL_;

namespace _SGUI_
{
    partial class SguiWindow
    {
        public readonly ValueNotifier<bool> toggle = new(true);

        //--------------------------------------------------------------------------------------------------------------

        void InitToggle()
        {
            toggle.AddListener(value =>
            {
                if (value)
                {
                    gameObject.SetActive(true);
                    TakeFocus();
                }
                else
                    openWindows.RemoveElement(this);

                BaseStates state = state_base;
                float offset = 0;

                switch (state)
                {
                    default:
                    case BaseStates.Default:
                        if (value)
                        {
                            gameObject.SetActive(true);
                            state = BaseStates.toActive;
                        }
                        break;

                    case BaseStates.Active:
                        if (!value)
                            state = BaseStates.fromActive_;
                        break;

                    case BaseStates.toActive:
                        if (!value)
                        {
                            state = BaseStates.fromActive_;
                            offset = 1 - animator.GetNormalizedTime01((int)AnimLayers.Base);
                        }
                        break;

                    case BaseStates.fromActive_:
                        if (value)
                        {
                            state = BaseStates.toActive;
                            offset = 1 - animator.GetNormalizedTime01((int)AnimLayers.Base);
                        }
                        break;
                }

                if (state != state_base)
                    animator.CrossFade((int)state, 0, (int)AnimLayers.Base, offset);
            });
        }
    }
}