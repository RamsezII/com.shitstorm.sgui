using System.IO;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    internal class Button_Hierarchy : MonoBehaviour
    {
        public SguiEditor editor;
        [HideInInspector] public TextMeshProUGUI text;
        public string full_path, short_path;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            editor = GetComponentInParent<SguiEditor>();
            text = transform.Find("name").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {

        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void Init(in string full_path)
        {
            gameObject.SetActive(true);
            this.full_path = full_path;
            short_path = Path.GetFileName(full_path);
            text.text = short_path;
        }
    }
}