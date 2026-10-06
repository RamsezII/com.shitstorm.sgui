using _ARK_;

namespace _SGUI_
{
    public abstract class SguiCustom_Abstract : ArkComponent1
    {
        public SguiCustom window;
        public Traductable trad_label;
        bool initialized;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            if (initialized) return;
            initialized = true;
            window = GetComponentInParent<SguiCustom>(true);
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            window.AutoSizeAtEndOfFrame();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            window.AutoSizeAtEndOfFrame();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleBottomLine(in bool value) => transform.Find("line_bottom").gameObject.SetActive(value);
    }
}
