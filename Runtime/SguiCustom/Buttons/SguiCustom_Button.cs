using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Button : SguiCustom_Abstract
    {
        public Button button;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            button = transform.Find("button").GetComponent<Button>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            button.onClick.RemoveAllListeners();
        }
    }
}