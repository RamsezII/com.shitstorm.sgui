using _ARK_;
using UnityEngine;

namespace _SGUI_
{
    public sealed class SguiBootCategory : MonoBehaviour
    {
        public Traductable trad;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            trad = GetComponent<Traductable>();
        }
    }
}