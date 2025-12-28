using _ARK_;
using TMPro;
using UnityEngine.UI;

namespace _SGUI_.searchbox
{
    public sealed class SearchboxItem : ArkComponent
    {
        public SguiSearchbox searchbox;
        public Button button;
        public TextMeshProUGUI label;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            searchbox = GetComponentInParent<SguiSearchbox>(true);
            button = GetComponentInChildren<Button>();
            label = GetComponentInChildren<TextMeshProUGUI>();

            base.Awake();

            button.onClick.AddListener(searchbox.Close);
        }
    }
}