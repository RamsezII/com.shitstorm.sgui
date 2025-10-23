using _ARK_;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class SguiCursor : MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        [SerializeField] RectTransform rt_mouse, rt_hover, rt_mouse2;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
            animator.writeDefaultValuesOnDisable = true;

            rt_mouse = (RectTransform)transform.Find("rt_cursor");
            rt_hover = (RectTransform)transform.Find("rt_label");
            rt_mouse2 = (RectTransform)rt_mouse.Find("cursor_1");
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            UsageManager.usages[(int)UsageGroups.GameMouse].AddListener1(this, gameObject.SetActive);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            NUCLEOR.delegates.LateUpdate += MoveCursor;
        }

        private void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= MoveCursor;
        }

        //--------------------------------------------------------------------------------------------------------------

        void MoveCursor()
        {
            rt_mouse.position = Input.mousePosition;
        }
    }
}