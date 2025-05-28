using TMPro;
using UnityEngine;

namespace _SGUI_
{
    internal class CompletorItem : MonoBehaviour
    {
        public TextMeshProUGUI label;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            label = transform.Find("label").GetComponent<TextMeshProUGUI>();
        }
    }
}