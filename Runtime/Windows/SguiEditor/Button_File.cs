using UnityEngine.UI;

namespace _SGUI_
{
    internal class Button_File : Button_Hierarchy
    {
        public Button button;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            button = GetComponent<Button>();
            base.Awake();
        }
    }
}