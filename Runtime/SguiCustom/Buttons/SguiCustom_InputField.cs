using TMPro;

namespace _SGUI_
{
    public class SguiCustom_InputField : SguiCustom_Abstract
    {
        public TMP_InputField input_field;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            input_field = transform.Find("input_field").GetComponent<TMP_InputField>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
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