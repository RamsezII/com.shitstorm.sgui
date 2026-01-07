using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    class ResizerVisual : ArkComponent
    {
        public static ResizerVisual instance;

        public RectTransform rt;
        readonly ValueHandler<object> current_user = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            instance = this;
            rt = (RectTransform)transform;

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            current_user.AddListener(value => gameObject.SetActive(value != null));
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool TryTakeFocus(in object user)
        {
            if (current_user._value == null)
                current_user.Value = null;

            if (current_user._value == null || current_user._value == user)
            {
                current_user.Value = user;
                return true;
            }
            else
                Debug.LogWarning($"{user} could not user Resizer (used by: {current_user._value})", this);
            return false;
        }

        public bool UntakeFocus(in object user)
        {
            if (current_user._value == user)
            {
                current_user.Value = null;
                return true;
            }

            if (current_user._value == null)
                current_user.Value = null;

            return false;
        }
    }
}