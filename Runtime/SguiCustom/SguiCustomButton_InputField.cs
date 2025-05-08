using TMPro;

namespace _SGUI_
{
    public class SguiCustomButton_InputField : SguiCustomButton_Abstract
    {
        public TMP_InputField inputfield;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            inputfield = transform.Find("input_field").GetComponent<TMP_InputField>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
            inputfield.onValueChanged.RemoveAllListeners();
            inputfield.onEndEdit.RemoveAllListeners();
            inputfield.onSubmit.RemoveAllListeners();
            inputfield.onSelect.RemoveAllListeners();
            inputfield.onDeselect.RemoveAllListeners();
            inputfield.onTextSelection.RemoveAllListeners();
            inputfield.onEndTextSelection.RemoveAllListeners();
        }
    }
}