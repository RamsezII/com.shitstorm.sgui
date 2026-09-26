using _ARK_;
using _SGUI_.context_click;
using _UTIL_;
using UnityEngine;

namespace _SGUI_.composer
{
    internal sealed partial class SguiTabHeader : ArkComponent1
    {
        SguiSplitView pview;
        [SerializeField] ContextListHandler listhandler;
        public SguiFrame frame;
        public readonly ValueNotifier<bool> isSelected = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            pview = GetComponentInParent<SguiSplitView>(includeInactive: true);

            pview.current_tab.AddListener(OnCurrentTab);

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            listhandler.callback += (ContextList list) =>
            {

            };
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnCurrentTab(SguiTabHeader tab)
        {
            isSelected.Value = this == tab;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            pview.current_tab.RemoveListener(OnCurrentTab);

            base.OnDestroy();
        }
    }
}