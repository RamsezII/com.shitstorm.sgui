using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _SGUI_
{
    public enum SguiCursorTypes : byte
    {
        Default,
        Grab,
        Search,
        Eye,
        Move,
        Vertical,
        Horizontal,
        Diagonal1,
        Diagonal2,
        _last_,
    }

    public sealed partial class SguiCursor : MonoBehaviour
    {
        public static SguiCursor instance;

        public interface IUser
        {
            bool IsStillUsingCursor();
        }

        [HideInInspector] public Animator animator;
        [SerializeField] RectTransform rt_cursor, rt_label, rt_icon_default;
        public IA_SguiCursor inputActions;

        readonly ListListener block_users = new();
        public Vector2 last_position;

        readonly ValueHandler<(SguiCursorTypes cursor, IUser user)> cursor_user = new();

        readonly RectTransform[] rimgs = new RectTransform[(int)SguiCursorTypes._last_];

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
            animator.writeDefaultValuesOnDisable = true;

            rt_cursor = (RectTransform)transform.Find("rt_cursor");
            rt_label = (RectTransform)transform.Find("rt_label");
            rt_icon_default = (RectTransform)rt_cursor.Find("default");

            inputActions?.Dispose();
            inputActions = new();

            for (int i = 0; i < rimgs.Length; i++)
                rimgs[i] = (RectTransform)rt_cursor.GetChild(i);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            NUCLEOR.delegates.Update_OnStartOfFrame += EvaluateJoystick;
            inputActions.Enable();
        }

        private void OnDisable()
        {
            NUCLEOR.delegates.Update_OnStartOfFrame -= EvaluateJoystick;
            inputActions.Disable();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            UsageManager.mouse_status.AddListener(value =>
            {
                if (this == null)
                    return;
                gameObject.SetActive(value == MouseStatus.GameMouse);
            });

            inputActions.Movement.Position.performed += context =>
            {
                if (block_users.IsEmpty)
                    last_position = context.ReadValue<Vector2>();
                else
                    ((Mouse)context.control.device).WarpCursorPosition(last_position);
                MoveMouse(last_position);
            };

            cursor_user.AddListener(value =>
            {
                for (int i = 0; i < (int)SguiCursorTypes._last_; ++i)
                    rimgs[i].gameObject.SetActive(i == (int)value.cursor);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void EvaluateJoystick()
        {
            if (block_users.IsEmpty)
            {
                Vector2 value = inputActions.Movement.Joystick.ReadValue<Vector2>();

                if (value.sqrMagnitude > .01f)
                {
                    last_position += Mathf.Max(Screen.width, Screen.height) * Time.unscaledDeltaTime * value;

                    Mouse.current?.WarpCursorPosition(last_position);

                    rt_cursor.position = last_position;
                }
            }
        }

        public void MoveMouse(in Vector2 position)
        {
            rt_cursor.position = last_position = position;
            Mouse.current?.WarpCursorPosition(last_position);
        }

        public void BlockMouse(in object user, in Vector2 mousePos)
        {
            last_position = mousePos;
            block_users.AddElement(user);
        }

        public void UnblockMouse(in object user)
        {
            block_users.RemoveElement(user);
        }

        public void UnsetSpecificUser(in IUser user)
        {
            if (user == cursor_user._value.user)
                UnsetUser();
        }

        public void UnsetUser()
        {
            if (cursor_user._value.user != null)
                UsageManager.ToggleUser(cursor_user._value.user, false, UsageGroups.GameMouse);
            cursor_user.Value = (0, null);
            NUCLEOR.delegates.LateUpdate -= EvaluateCursorUser;
        }

        public void SetUser(in SguiCursorTypes cursor, in IUser user)
        {
            UnsetUser();
            UsageManager.ToggleUser(user, true, UsageGroups.GameMouse);
            cursor_user.Value = (cursor, user);
            NUCLEOR.delegates.LateUpdate += EvaluateCursorUser;
        }


        void EvaluateCursorUser()
        {
            if (cursor_user._value.user != null)
                if (!cursor_user._value.user.IsStillUsingCursor())
                    UnsetUser();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            NUCLEOR.delegates.Update_OnStartOfFrame -= EvaluateJoystick;
            NUCLEOR.delegates.LateUpdate -= EvaluateCursorUser;

            inputActions.Dispose();
        }
    }
}