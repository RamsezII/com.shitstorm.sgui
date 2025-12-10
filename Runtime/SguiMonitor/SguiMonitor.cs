using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    public class SguiMonitor : SguiWindow2
    {
        static readonly HashSet<SguiMonitor> active_monitors = new();

        readonly Dictionary<Type, SguiMonitor_Addable> addables_prefabs = new();
        readonly List<SguiMonitor_Addable> addables_clones = new();

        public static Action<SguiMonitor> onOpenedMonitor;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            onOpenedMonitor = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            var button = OSView.instance.GetSoftwareButton<SguiMonitor>(force: true);
            button.hover_info = new()
            {
                french = "Moniteur de Ressources",
                english = "Resources Monitor",
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            foreach (var addable in GetComponentsInChildren<SguiMonitor_Addable>())
                addables_prefabs.Add(addable.GetType(), addable);

            base.OnAwake();

            trad_title.SetTrads(new()
            {
                french = "Moniteur",
                english = "Monitor",
            });

            rimg_background.gameObject.SetActive(false);
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

            onOpenedMonitor?.Invoke(this);
        }

        public T AddSection<T>() where T : SguiMonitor_Addable
        {
            var prefab = addables_prefabs[typeof(T)];

            var clone = Instantiate(prefab, prefab.transform.parent);
            addables_clones.Add(clone);

            clone.gameObject.SetActive(true);

            return (T)clone;
        }
    }
}