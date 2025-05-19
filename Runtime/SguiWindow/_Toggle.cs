namespace _SGUI_
{
    partial class SguiWindow
    {
        public bool IsWindowOpened => state_base switch
        {
            BaseStates.Active or BaseStates.toActive => true,
            _ => false,
        };

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleWindow(bool toggle)
        {
            BaseStates state = state_base;
            float offset = 0;

            switch (state)
            {
                default:
                case BaseStates.Default:
                    if (toggle)
                    {
                        gameObject.SetActive(true);
                        animator.enabled = true;
                        animator.Update(0);
                        state = BaseStates.toActive;
                    }
                    break;

                case BaseStates.Active:
                    if (!toggle)
                    {
                        animator.enabled = true;
                        animator.Update(0);
                        state = BaseStates.fromActive_;
                    }
                    break;

                case BaseStates.toActive:
                    if (!toggle)
                    {
                        state = BaseStates.fromActive_;
                        offset = 1 - animator.GetNormlizedTimeClamped((int)AnimLayers.Base);
                    }
                    break;

                case BaseStates.fromActive_:
                    if (toggle)
                    {
                        state = BaseStates.toActive;
                        offset = 1 - animator.GetNormlizedTimeClamped((int)AnimLayers.Base);
                    }
                    break;
            }

            if (state != state_base)
                animator.CrossFade((int)state, 0, (int)AnimLayers.Base, offset);
        }
    }
}