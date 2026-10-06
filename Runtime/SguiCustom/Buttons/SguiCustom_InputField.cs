using TMPro;

namespace _SGUI_
{
    public class SguiCustom_InputField : SguiCustom_Abstract
    {
        public TMP_InputField input_field;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();

            input_field.onValueChanged.RemoveAllListeners();
            input_field.onEndEdit.RemoveAllListeners();
            input_field.onSubmit.RemoveAllListeners();
            input_field.onSelect.RemoveAllListeners();
            input_field.onDeselect.RemoveAllListeners();
            input_field.onTextSelection.RemoveAllListeners();
            input_field.onEndTextSelection.RemoveAllListeners();
        }
    }
}