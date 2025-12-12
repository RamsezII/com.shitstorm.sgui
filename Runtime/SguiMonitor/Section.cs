using _ARK_;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Monitor
{
    public abstract class Section : MonoBehaviour
    {
        public Page page;
        public RectTransform prt, rt;
        [SerializeField] internal Toggle toggle;
        public Traductable trad;
        [SerializeField] RectTransform arrow_rt;
        internal VerticalLayoutGroup vlayout;

        internal readonly Dictionary<Type, SectionChild> elements_prefabs = new();
        internal readonly List<SectionChild> elements_clones = new();

        private void OnValidate()
        {
            if (didStart)
                AutoSize();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            page = GetComponentInParent<Page>();
            prt = (RectTransform)transform.parent;
            rt = (RectTransform)transform;
            toggle = GetComponent<Toggle>();
            trad = GetComponentInChildren<Traductable>();
            arrow_rt = (RectTransform)transform.Find("text/arrow");
            vlayout = GetComponentInChildren<VerticalLayoutGroup>(includeInactive: true);

            foreach (var addable in vlayout.GetComponentsInChildren<SectionChild>(includeInactive: true))
                elements_prefabs.Add(addable.GetType(), addable);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnable()
        {
            if (didStart)
                Refresh();
        }

        protected virtual void OnDisable()
        {
            if (didStart)
                Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            foreach (var pair in elements_prefabs)
                pair.Value.gameObject.SetActive(false);

            toggle.onValueChanged.AddListener(value => Refresh());
            Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------

        public T AddElement<T>() where T : SectionChild, new()
        {
            var prefab = elements_prefabs[typeof(T)];

            var element = Instantiate(prefab, prefab.transform.parent);
            element.section = this;
            elements_clones.Add(element);
            element.transform.SetSiblingIndex(transform.GetSiblingIndex() + elements_clones.Count);
            element.gameObject.SetActive(true);

            OnElement(element);

            page.AutoSize();

            return (T)element;
        }

        protected virtual void OnElement(in SectionChild element)
        {
        }

        void Refresh()
        {
            bool isOn = isActiveAndEnabled && toggle.isOn;
            OnToggle(isOn);
            AutoSize();
            page.AutoSize();
        }

        protected virtual void OnToggle(in bool isOn)
        {
            arrow_rt.localRotation = Quaternion.Euler(0, 0, isOn ? 0 : 90);
        }

        public void AutoSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, OnAutoSize);
        protected abstract void OnAutoSize();

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            for (int i = 0; i < elements_clones.Count; ++i)
                if (elements_clones[i] != null)
                    Destroy(elements_clones[i].gameObject);
        }
    }
}