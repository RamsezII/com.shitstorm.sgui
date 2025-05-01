using System;
using System.Collections.Generic;
using _UTIL_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public abstract class SguiCustomButton : MonoBehaviour, IDisposable
    {
        public struct Infos
        {
            public Traductions label;
            public Type type;
            public List<TMP_Dropdown.OptionData> items;
        }

        [SerializeField] Traductable label;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            label = transform.Find("label").GetComponent<Traductable>();
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void Init(in Infos infos)
        {
            label.SetTrads(infos.label);
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