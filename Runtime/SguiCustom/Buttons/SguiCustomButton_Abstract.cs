using System;
using _UTIL_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public abstract class SguiCustomButton_Abstract : MonoBehaviour, IDisposable
    {
        [HideInInspector] public RectTransform rt, rt_label;
        [HideInInspector] public TextMeshProUGUI tmp_label;
        [HideInInspector] public Traductable trad_label;
        public bool disposed;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            rt = (RectTransform)transform;
            rt_label = (RectTransform)transform.Find("label");
            tmp_label = rt_label.GetComponent<TextMeshProUGUI>();
            trad_label = rt_label.GetComponent<Traductable>();
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