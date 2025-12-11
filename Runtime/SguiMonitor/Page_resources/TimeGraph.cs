using _ARK_;
using _UTIL_;

namespace _SGUI_.Monitor.Resources
{
    public class TimeGraph : ResourcesSectionChild
    {
        public new UI_TimeGraphRenderer renderer;
        HeartBeat.Operation op_refresh;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            renderer = GetComponentInChildren<UI_TimeGraphRenderer>();

            base.Awake();

            op_refresh = new(.2f, true, renderer.SetVerticesDirty);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            NUCLEOR.instance.heartbeat_unscaled.operations.Add(op_refresh);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            NUCLEOR.instance.heartbeat_unscaled.operations.Remove(op_refresh);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            op_refresh.Dispose();
        }
    }
}