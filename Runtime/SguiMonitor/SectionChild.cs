using UnityEngine;

namespace _SGUI_.Monitor
{
    public abstract class SectionChild : MonoBehaviour
    {
        public Section section;
        bool initialized;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake() => Initialize();

        internal void Initialize()
        {
            if (initialized) return;
            initialized = true;
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
            section = GetComponentInParent<Section>(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            section?.elements_clones.Remove(this);
        }
    }
}