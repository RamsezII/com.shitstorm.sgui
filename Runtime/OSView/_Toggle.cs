using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class OSView
    {
        public readonly HashSetListener
            users_forceOpen = new(),
            users_allowClosed = new();

        readonly ValueNotifier<bool> toggle = new();
        public readonly ValueNotifier<bool> isVisible = new();

        static readonly object auto_usage = new();
        public void ToggleSelf(in bool toggle) => users_forceOpen.ToggleElement(auto_usage, toggle);

        [SerializeField, Range(0, 1)] float toggle_lerp;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeToggle()
        {
            void NotifyToggle(bool _)
            {
                toggle.Value = users_forceOpen.IsNotEmpty || users_allowClosed.IsEmpty;
            }

            users_forceOpen.AddListener1(NotifyToggle, doNotCallThisTime: false);
            users_allowClosed.AddListener1(NotifyToggle, doNotCallThisTime: true);

            toggle_lerp = .5f;
        }

        //--------------------------------------------------------------------------------------------------------------

        void StartToggle()
        {
            toggle.AddListener(value =>
            {
                if (value)
                    UsageManager.AddUser(this, UsageGroups.BlockPlayer, UsageGroups.TrueMouse, UsageGroups.Keyboard);
                else
                    UsageManager.RemoveUser(this);

                NUCLEOR.delegates.LateUpdate -= RefreshToggle;
                NUCLEOR.delegates.LateUpdate += RefreshToggle;
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshToggle()
        {
            int target = toggle._value ? 1 : 0;
            toggle_lerp = Mathf.MoveTowards(toggle_lerp, target, 3 * Time.unscaledDeltaTime);
            float smooth = Mathf.SmoothStep(0, 1, toggle_lerp);

            header_rt.pivot = new(.5f, smooth);
            taskbar_rt.pivot = new(.5f, 1 - smooth);

            rootGroup.alpha = Mathf.InverseLerp(.5f, 1, smooth);
            rootGroup.interactable = toggle_lerp > .5f;

            isVisible.Value = toggle_lerp > 0;

            if (toggle_lerp == target)
                NUCLEOR.delegates.LateUpdate -= RefreshToggle;
        }
    }
}