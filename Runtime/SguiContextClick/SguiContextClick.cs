using System;
using UnityEngine;

namespace _SGUI_
{
    public class SguiContextClick : MonoBehaviour
    {
        public interface IUser
        {
            void OnSguiContextClick(SguiContextClick_List context_list);
        }

        public static SguiContextClick instance;

        [SerializeField] internal SguiContextClick_List prefab_list;
        [SerializeField] internal SguiContextClick_List scrollview_lastRootList;

        public static Action<SguiContextClick_List> onGlobalContextList;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            onGlobalContextList = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            prefab_list = GetComponentInChildren<SguiContextClick_List>(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            prefab_list.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiContextClick_List RightClickHere(in Vector2 mousePosition)
        {
            if (scrollview_lastRootList != null)
                Destroy(scrollview_lastRootList.gameObject);

            scrollview_lastRootList = prefab_list.Clone(true);
            scrollview_lastRootList.rt.position = mousePosition;

            return scrollview_lastRootList;
        }
    }
}