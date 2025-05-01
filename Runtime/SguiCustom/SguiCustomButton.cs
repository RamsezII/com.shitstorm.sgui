using System;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public abstract class SguiCustomButton : MonoBehaviour, IDisposable
    {
        public Traductable label;

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

        public virtual void Dispose()
        {
        }
    }
}