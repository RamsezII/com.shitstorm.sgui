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
        bool initialized;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake() => Initialize();

        internal void Initialize()
        {
            if (initialized) return;
            initialized = true;
            rt = (RectTransform)transform;
            trad = GetComponentInChildren<Traductable>(true);
            init_height = rt.sizeDelta.y;
        }
    }
}
