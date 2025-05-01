using TMPro;

namespace _SGUI_
{
    public class SguiCustomButton_InputField : SguiCustomButton
    {
        public TMP_InputField input_field;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            input_field = transform.Find("input_field").GetComponent<TMP_InputField>();
            base.Awake();
        }
    }
}