using System;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public abstract class SguiCustomButton_Abstract : MonoBehaviour, IDisposable
    {
        public Traductable label;
        public bool disposed;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            label = transform.Find("label").GetComponent<Traductable>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleBottomLine(in bool value) => transform.Find("line_bottom").gameObject.SetActive(value);

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }
}