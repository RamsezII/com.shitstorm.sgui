using _ARK_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    partial class OSView
    {
        Scheduler.Operation op_framerate;

        //--------------------------------------------------------------------------------------------------------------

        void StartFramerate()
        {
            NUCLEOR.instance.scheduler_unscaled.AddOperation(op_framerate = new Scheduler.Operation("refresh framerate monitor", 1, true, () =>
            {
                float framerate = 1 / NUCLEOR.instance.averageUnscaledDeltatime;
                text_framerate.text = $"{Mathf.RoundToInt(framerate)}";

                if (framerate >= 28)
                    text_framerate.color = Color.white;
                else if (framerate >= 18)
                    text_framerate.color = Color.orange;
                else
                    text_framerate.color = Color.red;
            }));
        }
    }
}