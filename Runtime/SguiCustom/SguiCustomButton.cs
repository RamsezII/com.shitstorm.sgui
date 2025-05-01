using UnityEngine;

namespace _SGUI_
{
    public class SguiCustomButton : MonoBehaviour
    {
        public enum Codes : byte
        {
            Slider,
            InputField,
            Dropdown,
            _last_,
        }

        public struct Infos
        {
            public Codes code;
        }

        public Codes code;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {

        }

        //--------------------------------------------------------------------------------------------------------------

        public void Init(in Infos infos)
        {
            this.code = infos.code;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {

        }
    }
}