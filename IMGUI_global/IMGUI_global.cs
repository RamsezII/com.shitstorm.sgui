using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public sealed class IMGUI_global : MonoBehaviour
    {
        public interface IUser
        {
            void OnOnGUI();
        }

        public static IMGUI_global instance;

        public readonly ListListener<IUser> users = new();

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            Util.InstantiateOrCreateIfAbsent<IMGUI_global>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            users.Reset();
            DontDestroyOnLoad(gameObject);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            users.AddListener1(isNotEmpty =>
            {
                if (gameObject != null)
                    gameObject.SetActive(isNotEmpty);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnGUI()
        {
            for (int i = 0; i < users._list.Count; i++)
                users._list[i].OnOnGUI();
        }
    }
}