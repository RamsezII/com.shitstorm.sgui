using UnityEngine;

namespace _SGUI_
{
    public class SguiContextClick : MonoBehaviour
    {
        public static SguiContextClick instance;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
        }
    }
}