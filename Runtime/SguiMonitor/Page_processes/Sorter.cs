using _ARK_;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.Monitor.Processes
{
    public class Sorter : MonoBehaviour, SguiContextHover.IUser, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
    {
        public ProcessesPage page;
        public RectTransform rt;
        internal Toggle toggle;
        internal RawImage rimg_arrow;
        public Traductable trad;
        public Traductions hover_infos;

        public Action<bool> onIsAscendingOrder;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            page = GetComponentInParent<ProcessesPage>();
            rt = (RectTransform)transform;
            toggle = GetComponent<Toggle>();
            rimg_arrow = transform.Find("text/arrow").GetComponent<RawImage>();
            trad = GetComponentInChildren<Traductable>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            onIsAscendingOrder += asc => rimg_arrow.rectTransform.localRotation = Quaternion.Euler(0, 0, asc ? 180 : 0);
            toggle.onValueChanged.AddListener(value => onIsAscendingOrder(value));
            onIsAscendingOrder(toggle.isOn);
        }

        //--------------------------------------------------------------------------------------------------------------

        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            SguiContextHover.instance.SetTarget(this);
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            SguiContextHover.instance.SetTarget(this);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            SguiContextHover.instance.UnsetTarget(this);
        }
    }
}