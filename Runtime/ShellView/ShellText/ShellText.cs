using TMPro;
using UnityEngine;

namespace _SGUI_
{
    internal class ShellText : MonoBehaviour
    {
        public TMP_InputField inputfield;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            inputfield = transform.Find("inputfield").GetComponent<TMP_InputField>();
        }
    }
}