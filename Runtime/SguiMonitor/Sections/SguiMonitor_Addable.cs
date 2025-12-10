using UnityEngine;

namespace _SGUI_
{
    public abstract class SguiMonitor_Addable : MonoBehaviour
    {
        public SguiMonitor monitor;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            monitor = GetComponentInParent<SguiMonitor>();
        }
    }
}