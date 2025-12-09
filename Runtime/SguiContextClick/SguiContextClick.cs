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

        [SerializeField] internal SguiContextClick_List prefab_scrollview;
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
            prefab_scrollview = GetComponentInChildren<SguiContextClick_List>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            prefab_scrollview.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiContextClick_List RightClickHere(in Vector2 mousePosition)
        {
            if (scrollview_lastRootList != null)
                Destroy(scrollview_lastRootList.gameObject);

            scrollview_lastRootList = Instantiate(prefab_scrollview, prefab_scrollview.transform.parent);

            scrollview_lastRootList.gameObject.SetActive(true);
            scrollview_lastRootList.list_rt.position = mousePosition;

            return scrollview_lastRootList;
        }
    }
}