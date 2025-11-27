using UnityEngine;

namespace _SGUI_
{
    public class SguiMonitor : SguiWindow1
    {

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            OSView.instance.GetSoftwareButton<SguiMonitor>(force: true);
        }
    }
}