using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_.Monitor.Resources
{
    public sealed partial class ResourcesPage : Page
    {
        public interface IResourcesSection : SguiMonitor.IMonitorSection
        {
        }

        static readonly HashSet<ResourcesPage> active_monitors = new();

        readonly Dictionary<Type, IResourcesSection> addables_prefabs = new();
        readonly List<IResourcesSection> addables_clones = new();

        public static Action<ResourcesPage> onOpenResources;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            onOpenResources = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            foreach (var addable in GetComponentsInChildren<IResourcesSection>())
                addables_prefabs.Add(addable.GetType(), addable);

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            active_monitors.Add(this);
            if (didStart)
                RegenerateAddables();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            active_monitors.Remove(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            foreach (var pair in addables_prefabs)
                pair.Value.gameObject.SetActive(false);

            RegenerateAddables();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void RegenerateAllAddables()
        {
            foreach (var instance in active_monitors)
                instance.RegenerateAddables();
        }

        public void RegenerateAddables()
        {
            for (int i = 0; i < addables_clones.Count; i++)
                Destroy(addables_clones[i].gameObject);
            addables_clones.Clear();

            onOpenResources?.Invoke(this);
        }

        public T AddSection<T>() where T : IResourcesSection
        {
            var prefab = addables_prefabs[typeof(T)];

            var clone = Instantiate(prefab.gameObject, prefab.transform.parent).GetComponent<T>();
            addables_clones.Add(clone);

            clone.gameObject.SetActive(true);

            return (T)clone;
        }
    }
}