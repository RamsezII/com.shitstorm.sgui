using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_.Monitor
{
    public abstract class Page : MonoBehaviour
    {
        static readonly Dictionary<Type, HashSet<Page>> all_active_pages = new();
        static readonly Dictionary<Type, Action<Page>> pages_populators = new();
        HashSet<Page> active_pages;

        [SerializeField] Section prefab_section;

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
            if (pages_populators.TryGetValue(type, out var populators))
                if (populators != null)
                    if (all_active_pages.TryGetValue(type, out var pages))
                        foreach (var page in pages)
                            populators(page);
        }

        public T AddSection<T>() where T : Section, new()
        {
            T clone = (T)Instantiate(prefab_section, prefab_section.transform.parent);
            clone.gameObject.SetActive(true);
            return clone;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            if (active_pages.Count == 0)
                all_active_pages.Remove(GetType());
        }
    }
}