using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public class SguiContextClick : MonoBehaviour
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

                    var list = instance.InstantiateListHere(eventData.position);
                    OnSguiContextClick(list);
                }
            }
        }

        public static SguiContextClick instance;

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

        public ContextList InstantiateListHere(in Vector2 mousePosition)
        {
            if (scrollview_lastRootList != null)
                Destroy(scrollview_lastRootList.gameObject);

            scrollview_lastRootList = prefab_list.Clone(true);
            scrollview_lastRootList.rt.position = mousePosition;

            return scrollview_lastRootList;
        }
    }
}