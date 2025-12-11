using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Monitor
{
    class PageButton : MonoBehaviour
    {
        [SerializeField] internal Button button;
        [SerializeField] internal Image img_selected;
        [SerializeField] internal TextMeshProUGUI text;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            button = GetComponent<Button>();
            img_selected = transform.Find("selected").GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}