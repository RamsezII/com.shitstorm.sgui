using TMPro;
using UnityEngine;

namespace _SGUI_.context_tools.settings
{
    public sealed class TextLabel : MonoBehaviour
    {
        public TextMeshProUGUI text;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>(true);
        }
    }
}