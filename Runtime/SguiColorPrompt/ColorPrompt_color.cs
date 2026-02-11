using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.prompts.color_prompt
{
    internal class ColorPrompt_color : MonoBehaviour
    {
        [SerializeField] Graphic graphic;
        [SerializeField] RectTransform rt_fill;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            graphic = transform.Find("background").GetComponent<Graphic>();
            rt_fill = (RectTransform)transform.Find("alpha/fill");
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetColor(in Color color)
        {
            Color c = color;
            c.a = 1;
            graphic.color = c;
            rt_fill.anchorMax = new Vector2(color.a, 1);
        }
    }
}