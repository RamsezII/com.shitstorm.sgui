using TMPro;

namespace _SGUI_
{
    public class SguiCustomButton_InputField : SguiCustomButton
    {
        public TMP_InputField inputfield;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            inputfield = transform.Find("input_field").GetComponent<TMP_InputField>();
            base.Awake();
        }
    }
}