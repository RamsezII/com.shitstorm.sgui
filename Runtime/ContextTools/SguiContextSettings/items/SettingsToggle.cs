using UnityEngine.UI;

namespace _SGUI_.context_tools.settings
{
    public sealed class SettingsToggle : ContextSetting_item
    {
        public Toggle toggle;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            toggle = GetComponentInChildren<Toggle>(true);
            base.Awake();
        }
    }
}