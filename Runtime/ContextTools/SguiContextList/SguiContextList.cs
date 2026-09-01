using _ARK_;
using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public class SguiContextList : MonoBehaviour
    {
        public interface IUser : IPointerClickHandler
        {
            void OnSguiContextClick(ContextList list);
            void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    if (eventData.dragging)
                        return;

                    var list = instance.InstantiateListAtScreenPoint(eventData.position, eventData.pressEventCamera);
                    OnSguiContextClick(list);
                }
            }
        }

        public static SguiContextList instance;

        [SerializeField] internal ContextList prefab_list;
        [SerializeField] internal ContextList scrollview_lastRootList;

        public static Action<ContextList> onGlobalContextList;

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
            prefab_list = GetComponentInChildren<ContextList>(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            prefab_list.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        ContextList InstantiateList()
        {
            if (scrollview_lastRootList != null)
                Destroy(scrollview_lastRootList.gameObject);

            scrollview_lastRootList = prefab_list.Clone(true);
            return scrollview_lastRootList;
        }

        public ContextList InstantiateListAtScreenPoint(in Vector2 screenPoint, Camera eventCamera = null)
        {
            ContextList list = InstantiateList();
            ArkUI.instance.SetScreenPosition(list.rt, screenPoint, eventCamera);
            return list;
        }

        public ContextList InstantiateListAtWorldPoint(in Vector3 worldPoint)
        {
            ContextList list = InstantiateList();
            list.rt.position = worldPoint;
            return list;
        }
    }
}
