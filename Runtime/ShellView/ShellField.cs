using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed class ShellField : MonoBehaviour
    {
        public TMP_InputField inputfield;
        public TextMeshProUGUI lint;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            inputfield = GetComponent<TMP_InputField>();
            lint = transform.Find("area/lint").GetComponent<TextMeshProUGUI>();
        }

        //----------------------------------------------------------------------------------------------------------

        public void ResetText()
        {
            if (!string.IsNullOrEmpty(inputfield.text))
                inputfield.text = string.Empty;
            lint.text = string.Empty;
        }
    }
}