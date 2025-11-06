using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed class SguiBootEntry : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI label;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            button = GetComponent<Button>();
            label = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}