using UnityEngine;

namespace _SGUI_
{
    public class SGUI_global : MonoBehaviour
    {
        public static SGUI_global instance;

        public Canvas canvas;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            Util.InstantiateOrCreateIfAbsent<SGUI_global>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            canvas = GetComponent<Canvas>();
        }
    }
}