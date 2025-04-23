using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class Button_Hierarchy : MonoBehaviour
    {
        public SguiEditor editor;
        [HideInInspector] public Button button;
        [HideInInspector] public RectTransform offset_rT;
        [HideInInspector] public TextMeshProUGUI text;
        public string full_path, short_path;
        public int depth;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            editor = GetComponentInParent<SguiEditor>();
            button = GetComponent<Button>();
            offset_rT = (RectTransform)transform.Find("offset");
            text = transform.Find("offset/name").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {

        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void Init(in string full_path, in int depth)
        {
            gameObject.SetActive(true);
            this.depth = depth;
            offset_rT.anchoredPosition += depth * editor.hierarchy_width * Vector2.right;
            this.full_path = full_path.ToLinuxPath();
            short_path = Path.GetFileName(full_path);
            text.text = short_path;
        }
    }
}