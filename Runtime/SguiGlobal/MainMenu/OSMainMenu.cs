using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    internal partial class OSMainMenu : MonoBehaviour
    {
        internal static OSMainMenu instance;
        [HideInInspector] public Animator animator;
        public bool IsActive => state_base == BaseStates.Enable;

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
            RectTransform rt_layout = (RectTransform)transform.Find("buttons/layout");
            VerticalLayoutGroup layout = rt_layout.GetComponent<VerticalLayoutGroup>();
            rt_layout.sizeDelta = new Vector2(rt_layout.sizeDelta.x, layout.preferredHeight);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Toggle() => Toggle(!IsActive);
        public void Toggle(in bool value)
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
                        animator.CrossFade((int)BaseStates.Enable_, 0, 0, 1 - animator.GetNormalizedTimeClamped(0));
                    break;

                case BaseStates.Enable_:
                    if (value)
                        animator.CrossFade((int)BaseStates.Enable, 0, 0, 1 - animator.GetNormalizedTimeClamped(0));
                    break;

                default:
                    gameObject.SetActive(value);
                    animator.PlayInFixedTime((int)(value ? BaseStates.Enable : BaseStates.Off));
                    break;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (this == instance)
                instance = null;
        }
    }
}