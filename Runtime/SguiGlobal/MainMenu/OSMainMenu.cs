using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    internal partial class OSMainMenu : MonoBehaviour
    {
        internal static OSMainMenu instance;
        [HideInInspector] public Animator animator;
        public readonly OnValue<bool> toggle = new(true);

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
            animator.writeDefaultValuesOnDisable = true;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            toggle.AddListener(value =>
            {
                switch (state_base)
                {
                    case BaseStates.Off:
                        if (value)
                        {
                            gameObject.SetActive(true);
                            animator.CrossFadeInFixedTime((int)BaseStates.Enable, 0, 0);
                        }
                        break;

                    case BaseStates.Enable:
                        if (!value)
                            animator.CrossFade((int)BaseStates.Enable_, 0, 0, 1 - animator.GetNormlizedTimeClamped(0));
                        break;

                    case BaseStates.Enable_:
                        if (value)
                            animator.CrossFade((int)BaseStates.Enable, 0, 0, 1 - animator.GetNormlizedTimeClamped(0));
                        break;

                    default:
                        gameObject.SetActive(value);
                        animator.PlayInFixedTime((int)(value ? BaseStates.Enable : BaseStates.Off));
                        break;
                }
            });
            toggle.Update(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnDestroy()
        {
            if (this == instance)
                instance = null;
        }
    }
}