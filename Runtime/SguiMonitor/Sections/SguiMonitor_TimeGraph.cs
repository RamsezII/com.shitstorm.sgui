using _UTIL_;

namespace _SGUI_
{
    public class SguiMonitor_TimeGraph : SguiMonitor_Addable
    {
        public UI_TimeGraphRenderer graph;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            graph = GetComponentInChildren<UI_TimeGraphRenderer>();
            base.Awake();
        }
    }
}