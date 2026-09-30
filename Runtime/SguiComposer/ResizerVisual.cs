using _ARK_;
using _UTIL_;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace _SGUI_
{
    internal partial class ResizerVisual : ArkComponent1
    {
        [AutoStaticsCleanup] public static ResizerVisual instance;

        public RectTransform rt;
        readonly ValueNotifier<object> current_user = new();

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

            current_user.AddListener(OnCurrentUserChanged);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void TakeFocus(in object user)
        {
            if (current_user._value is Object current_object && current_object == null)
                current_user.Value = null;

            if (current_user.Has && current_user._value != user)
                Debug.LogWarning($"user({user}) tried taking UIResizer from user({current_user._value})", this);
            current_user.Value = user;
        }

        public bool UntakeFocus(in object user)
        {
            if (current_user._value == user)
            {
                current_user.Value = null;
                return true;
            }

            if (current_user.HasNot)
                current_user.Value = null;

            return false;
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnCurrentUserChanged(object value) => gameObject.SetActive(value != null);

        protected override void OnDestroy()
        {
            current_user.RemoveListener(OnCurrentUserChanged);
            current_user.Clear();

            if (instance == this)
                instance = null;

            base.OnDestroy();
        }
    }
}
