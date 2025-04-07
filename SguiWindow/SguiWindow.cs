using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiWindow : MonoBehaviour
    {
        public static readonly ListListener<SguiWindow> instances = new();
        public bool HasFocus => instances.IsLast(this);

        [HideInInspector] public Animator animator;

        [SerializeField] bool animated_toggle = true;

        public readonly OnValue<bool> isActive = new();

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            animator.writeDefaultValuesOnDisable = true;
            animator.keepAnimatorStateOnDisable = true;
            AwakeUI();
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            NUCLEOR.delegates.onLateUpdate += UpdateHue;

            IMGUI_global.instance.users_ongui.RemoveElement(OnIMGui_toggle_fullscreen);
            IMGUI_global.instance.users_ongui.AddElement(OnIMGui_toggle_fullscreen, this);
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            IMGUI_global.instance.users_ongui.RemoveElement(OnIMGui_toggle_fullscreen);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            if (!animated_toggle)
                isActive.AddListener(toggle => gameObject.SetActive(toggle));
            else
                isActive.AddListener(toggle =>
                {
                    BaseStates state = state_base;
                    float offset = 0;

                    switch (state)
                    {
                        case BaseStates.Default:
                            if (toggle)
                            {
                                gameObject.SetActive(true);
                                state = BaseStates.toActive;
                            }
                            break;

                        case BaseStates.Active:
                            if (!toggle)
                                state = BaseStates.fromActive_;
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
                });
        }

        //--------------------------------------------------------------------------------------------------------------

        bool OnIMGui_toggle_fullscreen(Event e)
        {
            if (e.type == EventType.KeyDown)
                if (e.keyCode == KeyCode.F11)
                {
                    fullscreen.Toggle();
                    return true;
                }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            instances.RemoveElement(this);
        }
    }
}