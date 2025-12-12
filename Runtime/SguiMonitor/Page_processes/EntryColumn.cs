using _ARK_;
using UnityEngine;

namespace _SGUI_.Monitor.Processes
{
    public class EntryColumn : MonoBehaviour
    {
        internal RectTransform rt;
        public Traductable trad;
        public int column_index;
        internal float init_height;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            rt = (RectTransform)transform;
            trad = GetComponentInChildren<Traductable>();
            init_height = rt.sizeDelta.y;
        }
    }
}
