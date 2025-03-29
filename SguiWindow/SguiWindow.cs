using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiWindow : MonoBehaviour
    {
        public static SguiWindow focus;

        public Canvas canvas;
        public GraphicRaycaster raycaster;
        public Button button_hide, button_fullscreen, button_close;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();

            button_hide = transform.Find("rT/header/buttons/hide").GetComponent<Button>();
            button_fullscreen = transform.Find("rT/header/buttons/fullscreen").GetComponent<Button>();
            button_close = transform.Find("rT/header/buttons/close").GetComponent<Button>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
        }

        protected virtual void OnDestroy()
        {
            if (this == focus)
                focus = null;
        }
    }
}