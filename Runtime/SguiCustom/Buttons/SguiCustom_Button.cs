using _ARK_;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Button : SguiCustom_Abstract, SguiContextHover.IUser
    {
        public Button button;
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

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