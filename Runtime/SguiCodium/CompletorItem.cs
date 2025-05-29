using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class CompletorItem : MonoBehaviour
    {
        RawImage background;
        public TextMeshProUGUI label;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            background = GetComponent<RawImage>();
            label = transform.Find("label").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------


    }
}