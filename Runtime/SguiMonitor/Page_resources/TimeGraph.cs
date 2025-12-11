using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_.Monitor.Resources
{
    public class TimeGraph : MonoBehaviour, ResourcesPage.IResourcesSection
    {
        public new UI_TimeGraphRenderer renderer;
        HeartBeat.Operation op_refresh;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            renderer = GetComponentInChildren<UI_TimeGraphRenderer>();
            op_refresh = new(.2f, true, renderer.SetVerticesDirty);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            NUCLEOR.instance.heartbeat_unscaled.operations.Add(op_refresh);
        }

        private void OnDisable()
        {
            NUCLEOR.instance.heartbeat_unscaled.operations.Remove(op_refresh);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            op_refresh.Dispose();
        }
    }
}