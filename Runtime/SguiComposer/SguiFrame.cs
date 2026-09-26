using _ARK_;

namespace _SGUI_.composer
{
    public abstract partial class SguiFrame : ArkComponent1
    {
        internal SguiSplitView pview;
        internal SguiTabHeader tab;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            pview = GetComponentInParent<SguiSplitView>(includeInactive: true);

            base.Awake();
        }
    }
}