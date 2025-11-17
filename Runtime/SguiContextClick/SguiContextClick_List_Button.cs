using _ARK_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed class SguiContextClick_List_Button : MonoBehaviour
    {
        public Button button;
        public Traductable trad;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            button = GetComponentInChildren<Button>();
            trad = GetComponentInChildren<Traductable>();
        }
    }
}