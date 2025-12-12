using UnityEngine;

namespace _SGUI_.Monitor
{
    public abstract class SectionChild : MonoBehaviour
    {
        public Section section;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            section = GetComponentInParent<Section>(includeInactive: true);
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