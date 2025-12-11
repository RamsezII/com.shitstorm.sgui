using _ARK_;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Monitor
{
    public abstract class Section : MonoBehaviour
    {
        public SguiMonitor monitor;
        [SerializeField] internal Toggle toggle;
        public Traductable trad;
        [SerializeField] RectTransform arrow_rt;

        internal readonly Dictionary<Type, SectionChild> elements_prefabs = new();
        readonly List<SectionChild> elements_clones = new();

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            monitor = GetComponentInParent<SguiMonitor>();
            toggle = GetComponent<Toggle>();
            trad = GetComponentInChildren<Traductable>();
            arrow_rt = (RectTransform)transform.Find("arrow");

            foreach (var addable in GetComponentInParent<VerticalLayoutGroup>().GetComponentsInChildren<SectionChild>(includeInactive: true))
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

        public T AddElement<T>() where T : SectionChild, new()
        {
            var prefab = elements_prefabs[typeof(T)];

            var clone = Instantiate(prefab, prefab.transform.parent);
            elements_clones.Add(clone);
            clone.transform.SetSiblingIndex(transform.GetSiblingIndex() + elements_clones.Count);
            clone.gameObject.SetActive(true);

            return (T)clone;
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

        void Refresh()
        {
            bool isOn = isActiveAndEnabled && toggle.isOn;
            arrow_rt.localRotation = Quaternion.Euler(0, 0, isOn ? 0 : 90);
            for (int i = 0; i < elements_clones.Count; i++)
                elements_clones[i].gameObject.SetActive(isOn);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            for (int i = 0; i < elements_clones.Count; ++i)
                if (elements_clones[i] != null)
                    Destroy(elements_clones[i].gameObject);
        }
    }
}