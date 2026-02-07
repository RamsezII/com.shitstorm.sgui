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
            graphic.color = color;
            rt_fill.anchorMax = new Vector2(color.a, 1);
        }
    }
}