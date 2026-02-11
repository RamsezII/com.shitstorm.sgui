using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class OSView
    {
        public readonly ListListener users = new();
        static readonly object auto_usage = new();
        public void ToggleSelf(in bool toggle) => users.ToggleElement(auto_usage, toggle);

        [SerializeField] float header_height, footer_height;
        [SerializeField, Range(0, 1)] float toggle_lerp;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeToggle()
        {
            toggle_lerp = .5f;
        }

        //--------------------------------------------------------------------------------------------------------------

        void StartToggle()
        {
            header_height = header_rt.rect.height;
            footer_height = taskbar_rt.rect.height;

            users.AddListener1(isNotEmpty =>
            {
                if (isNotEmpty)
                    UsageManager.AddUser(this, UsageGroups.BlockPlayer, UsageGroups.TrueMouse);
                else
                    UsageManager.RemoveUser(this);

                NUCLEOR.delegates.LateUpdate -= RefreshToggle;
                NUCLEOR.delegates.LateUpdate += RefreshToggle;
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshToggle()
        {
            bool toggle = users.IsNotEmpty;
            int target = toggle ? 1 : 0;
            float speed = toggle ? 3.5f : 6f;
            toggle_lerp = Mathf.MoveTowards(toggle_lerp, target, speed * Time.unscaledDeltaTime);
            float smooth = Mathf.SmoothStep(0, 1, toggle_lerp);

            SguiGlobal.instance.rT_2D.anchoredPosition = new(0, smooth * footer_height);
            SguiGlobal.instance.rT_2D.sizeDelta = new(0, 1 - smooth * (header_height + footer_height));

            header_rt.pivot = new(.5f, smooth);
            taskbar_rt.pivot = new(.5f, 1 - smooth);

            canvasGroup.alpha = Mathf.InverseLerp(.5f, 1, smooth);
            canvasGroup.interactable = toggle_lerp > .5f;

            if (toggle_lerp == target)
                NUCLEOR.delegates.LateUpdate -= RefreshToggle;
        }
    }
}