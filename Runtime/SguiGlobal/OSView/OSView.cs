using UnityEngine;

namespace _SGUI_
{
    public partial class OSView : MonoBehaviour
    {
        public static OSView instance;

        [HideInInspector] public Animator animator;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            animator = GetComponent<Animator>();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleView(bool toggle)
        {
            switch (state_base)
            {
                case BaseStates.Default:
                    if (toggle)
                    {
                        gameObject.SetActive(true);
                        //animator.CrossFade
                    }
                    break;

                case BaseStates.Enable:
                    break;

                case BaseStates.Enable_:
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