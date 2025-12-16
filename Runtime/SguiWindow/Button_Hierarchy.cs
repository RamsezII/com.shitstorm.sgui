using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class Button_Hierarchy : MonoBehaviour
    {
        public SguiWindow window;
        [HideInInspector] public Button button;
        [HideInInspector] public RectTransform offset_rT;
        [HideInInspector] public TextMeshProUGUI text;
        public string full_path, short_path;
        public int depth;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            window = GetComponentInParent<SguiWindow>();
            button = GetComponent<Button>();
            offset_rT = (RectTransform)transform.Find("offset");
            text = transform.Find("offset/name").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
        }
    }
}