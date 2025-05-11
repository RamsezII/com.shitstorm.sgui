using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Toggle : SguiCustom_Abstract
    {
        public Toggle toggle;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            toggle = transform.Find("toggle").GetComponent<Toggle>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
            toggle.onValueChanged.RemoveAllListeners();
        }
    }
}