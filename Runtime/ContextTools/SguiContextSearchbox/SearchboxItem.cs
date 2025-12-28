using _ARK_;
using TMPro;
using UnityEngine.UI;

namespace _SGUI_.searchbox
{
    public sealed class SearchboxItem : ArkComponent, SguiContextHover.IUser
    {
        public SguiSearchbox searchbox;
        public Button button;
        public TextMeshProUGUI label;
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            searchbox = GetComponentInParent<SguiSearchbox>(true);
            button = GetComponentInChildren<Button>();
            label = GetComponentInChildren<TextMeshProUGUI>();

            base.Awake();
        }
    }
}