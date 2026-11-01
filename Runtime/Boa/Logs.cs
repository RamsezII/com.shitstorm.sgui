using UnityEngine;

namespace _SGUI_.Boa
{
    static partial class Debug
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            //Boa.Debug.onLog += SguiLoggerOverlay.Log;
        }
    }
}