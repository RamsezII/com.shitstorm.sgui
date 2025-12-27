using _ARK_;
using _UTIL_;
using TMPro;
using UnityEngine.UI;

namespace _SGUI_.tab_control
{
    public class SguiTabButton : ArkComponent
    {
        public Button button_select, button_close;
        public TextMeshProUGUI text;
        public readonly ValueHandler<bool> selected = new();

        //--------------------------------------------------------------------------------------------------------------
        protected override void Awake()
        {
            button_select = GetComponent<Button>();
            button_close = transform.Find("close").GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>(true);

            base.Awake();
        }
    }
}