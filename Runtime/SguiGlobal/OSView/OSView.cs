using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class OSView : MonoBehaviour
    {
        public static OSView instance;

        [HideInInspector] public Animator animator;
        public readonly ListListener users = new();

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
            transform.Find("task-bar/main-button").GetComponent<Button>().onClick.AddListener(OSMainMenu.instance.Toggle);
            users.AddListener1(this, ToggleView);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleView(bool toggle)
        {
            BaseStates state = state_base;
            float fade = 0, offset = 0;

            switch (state_base)
            {
                case BaseStates.Default:
                    if (toggle)
                    {
                        gameObject.SetActive(true);
                        state = BaseStates.Enable;
                    }
                    break;

                case BaseStates.Enable:
                    if (!toggle)
                    {
                        state = BaseStates.Enable_;
                        offset = 1 - animator.GetNormlizedTimeClamped();
                    }
                    break;

                case BaseStates.Enable_:
                    if (toggle)
                    {
                        state = BaseStates.Enable;
                        offset = 1 - animator.GetNormlizedTimeClamped();
                    }
                    break;
            }

            if (state != state_base)
                animator.CrossFade((int)state, fade, (int)AnimLayers.Base, offset);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (this == instance)
                instance = null;
        }
    }
}