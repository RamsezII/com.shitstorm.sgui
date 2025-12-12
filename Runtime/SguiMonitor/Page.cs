using _ARK_;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Monitor
{
    public abstract class Page : MonoBehaviour
    {
        static readonly Dictionary<Type, HashSet<Page>> all_active_pages = new();
        static readonly Dictionary<Type, Action<Page>> pages_populators = new();
        HashSet<Page> active_pages;

        public SguiMonitor monitor;
        public ScrollRect scrollview;
        public VerticalLayoutGroup vlayout;
        [SerializeField] Section prefab_section;

        readonly List<Section> sections = new();

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            all_active_pages.Clear();
            pages_populators.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------

        internal protected virtual void OnAwake()
        {
            monitor = GetComponentInParent<SguiMonitor>(includeInactive: true);
            scrollview = GetComponentInChildren<ScrollRect>(includeInactive: true);
            vlayout = GetComponentInChildren<VerticalLayoutGroup>(includeInactive: true);
            prefab_section = GetComponentInChildren<Section>(includeInactive: true);
        }

        protected virtual void Awake()
        {
            Type type = GetType();
            if (!all_active_pages.TryGetValue(type, out active_pages))
                all_active_pages.Add(type, active_pages = new());
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnable()
        {
            active_pages.Add(this);
        }

        protected virtual void OnDisable()
        {
            active_pages.Remove(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            prefab_section.gameObject.SetActive(false);
            foreach (var pair in prefab_section.elements_prefabs)
                pair.Value.gameObject.SetActive(false);

            if (pages_populators.TryGetValue(GetType(), out var populators))
                populators?.Invoke(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void AddPopulator<T>(Action<Page> onPopulate) where T : Page, new()
        {
            Type type = typeof(T);
            if (pages_populators.ContainsKey(type))
                pages_populators[type] += onPopulate;
            else
                pages_populators[type] = onPopulate;
            RegenerateAllPagesOfType<T>();
        }

        public static void RemovePopulator<T>(Action<Page> onPopulate) where T : Page, new()
        {
            Type type = typeof(T);
            if (pages_populators.ContainsKey(type))
                pages_populators[type] -= onPopulate;
            RegenerateAllPagesOfType<T>();
        }

        static void RegenerateAllPagesOfType<T>() where T : Page, new()
        {
            Type type = typeof(T);
            if (all_active_pages.TryGetValue(type, out var pages))
            {
                foreach (var page in pages)
                {
                    for (int i = 0; i < page.sections.Count; i++)
                        Destroy(page.sections[i].gameObject);
                    page.sections.Clear();
                }

                if (pages_populators.TryGetValue(type, out var populators))
                    if (populators != null)
                        foreach (var page in pages)
                            populators(page);
            }
        }

        public T AddSection<T>() where T : Section, new()
        {
            T section = (T)Instantiate(prefab_section, prefab_section.transform.parent);
            section.gameObject.SetActive(true);
            sections.Add(section);
            OnSection(section);
            return section;
        }

        protected virtual void OnSection(in Section section)
        {
        }

        public void AutoSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, _AutoSize);
        void _AutoSize()
        {
            if (this == null)
                return;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);
            scrollview.content.sizeDelta = new(vlayout.preferredWidth, vlayout.preferredHeight);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            if (active_pages.Count == 0)
                all_active_pages.Remove(GetType());
        }
    }
}