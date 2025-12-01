using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow1
    {

        //--------------------------------------------------------------------------------------------------------------

        void StartResize()
        {
            fullscreen.AddListener(toggle =>
            {
                if (toggle)
                {
                    rect_current = new(rT);
                    rT.sizeDelta = Vector2.zero;
                    rT.anchorMin = Vector2.zero;
                    rT.anchorMax = Vector2.one;
                    rT.anchoredPosition = Vector2.zero;
                }
                else
                    rect_current.Apply(rT);
                OnResized();
            });

            button_fullscreen.onClick.AddListener(fullscreen.Toggle);
        }

        public virtual void OnResized()
        {

        }
    }
}