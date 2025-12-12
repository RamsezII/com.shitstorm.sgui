using _ARK_;
using _UTIL_;

namespace _SGUI_.Monitor.Resources
{
    public class TimeGraph : ResourcesSectionChild
    {
        public new UI_TimeGraphRenderer renderer;
        public HeartBeat.Operation op_refresh;
        public float renderStep = .2f;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            renderer = GetComponentInChildren<UI_TimeGraphRenderer>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            NUCLEOR.instance.heartbeat_unscaled.AddOperation(op_refresh = new(renderStep, true, () =>
            {
                op_refresh.delay = renderStep;
                renderer.SetVerticesDirty();
            }));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            op_refresh.Dispose();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            op_refresh.Dispose();
        }
    }
}