using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed class ShellField : MonoBehaviour
    {
        public RectTransform rT, parent_rT;
        public TMP_InputField inputfield;
        public TextMeshProUGUI lint;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            rT = (RectTransform)transform;
            parent_rT = (RectTransform)rT.parent;
            inputfield = GetComponent<TMP_InputField>();
            lint = transform.Find("area/lint").GetComponent<TextMeshProUGUI>();
        }
    }
}