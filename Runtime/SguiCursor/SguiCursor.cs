using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _SGUI_
{
    public sealed partial class SguiCursor : MonoBehaviour
    {
        public static SguiCursor instance;

        [HideInInspector] public Animator animator;
        [SerializeField] RectTransform rt_mouse, rt_hover, rt_mouse2;
        public IA_SguiCursor inputActions;

        public readonly ListListener blocking_users = new();
        public Vector2 last_position;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
            animator.writeDefaultValuesOnDisable = true;

            rt_mouse = (RectTransform)transform.Find("rt_cursor");
            rt_hover = (RectTransform)transform.Find("rt_label");
            rt_mouse2 = (RectTransform)rt_mouse.Find("cursor_1");

            inputActions?.Dispose();
            inputActions = new();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            UsageManager.usages[(int)UsageGroups.GameMouse].AddListener1(this, gameObject.SetActive);

            inputActions.Movement.Position.performed += context =>
            {
                if (blocking_users.IsEmpty)
                    last_position = context.ReadValue<Vector2>();
                else
                    ((Mouse)context.control.device).WarpCursorPosition(last_position);
                rt_mouse.position = last_position;
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            inputActions.Dispose();
            blocking_users.Reset();
        }
    }
}