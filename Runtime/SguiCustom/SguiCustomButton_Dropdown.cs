using TMPro;

namespace _SGUI_
{
    public class SguiCustomButton_Dropdown : SguiCustomButton
    {
        public readonly struct Output
        {
            public readonly int index;
            public readonly TMP_Dropdown.OptionData option_data;
            public override string ToString() => $"{index} {option_data.text.QuoteStringSafely()}";

            //--------------------------------------------------------------------------------------------------------------

            public Output(in int index, in TMP_Dropdown.OptionData option_data)
            {
                this.index = index;
                this.option_data = option_data;
            }
        }

        public TMP_Dropdown dropdown;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            dropdown.options.Clear();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
            dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}