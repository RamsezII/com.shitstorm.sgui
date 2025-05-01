using TMPro;

namespace _SGUI_
{
    public class SguiCustomButton_Dropdown : SguiCustomButton
    {
        public TMP_Dropdown dropdown;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void Init(in Infos infos)
        {
            base.Init(infos);
            dropdown.AddOptions(infos.items);
        }
    }
}