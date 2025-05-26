using System;
using _ARK_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public abstract class SguiCustom_Abstract : MonoBehaviour, IDisposable
    {
        [HideInInspector] public RectTransform rT, rT_parent, rT_label;
        [HideInInspector] public TextMeshProUGUI tmp_label;
        [HideInInspector] public Traductable trad_label;
        public bool disposed;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            rT = (RectTransform)transform;
            rT_parent = (RectTransform)transform.parent;
            rT_label = (RectTransform)transform.Find("label");
            tmp_label = rT_label.GetComponent<TextMeshProUGUI>();
            trad_label = rT_label.GetComponent<Traductable>();
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