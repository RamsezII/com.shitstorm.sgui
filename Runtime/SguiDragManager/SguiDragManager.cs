using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiDragManager : ArkComponent
    {
        public interface IDraggable<T> : IBeginDragHandler, IDragHandler, IEndDragHandler
        {
            Traductions DragInfos { get; }
            T DragData { get; }

            void OnDropAccepted(in IAcceptDraggable<T> acceptor);

            void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
            {
                instance.gameObject.SetActive(true);
                instance.trad.SetTrads(DragInfos);
                instance.rt_size.sizeDelta = instance.trad.tmpro.GetPreferredValues();
                instance.rt_pos.position = eventData.position;
            }

            void IDragHandler.OnDrag(PointerEventData eventData)
            {
                instance.rt_pos.position = eventData.position;
                instance.RaycastUnder(this, eventData);
            }

            void IEndDragHandler.OnEndDrag(PointerEventData eventData)
            {
                instance.gameObject.SetActive(false);
            }
        }

        public interface IAcceptDraggable<T> : IDropHandler
        {
            void IDropHandler.OnDrop(PointerEventData eventData)
            {
                if (eventData.pointerDrag.TryGetComponent<IDraggable<T>>(out var draggable))
                    if (CanAcceptDraggable(draggable))
                    {
                        AcceptDraggable(draggable);
                        draggable.OnDropAccepted(this);
                    }
                instance.gameObject.SetActive(false);
            }

            bool CanAcceptDraggable(in IDraggable<T> draggable);
            void AcceptDraggable(in IDraggable<T> draggable);
        }

        public static SguiDragManager instance;

        [SerializeField] RectTransform rt_pos, rt_size;
        [SerializeField] RawImage rimg_ok, rimg_no, rimg_none;
        [SerializeField] Traductable trad;

        readonly List<RaycastResult> results = new();

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            instance = this;

            rt_pos = (RectTransform)transform.Find("rt_pos");
            rt_size = (RectTransform)rt_pos.transform.Find("rt_size");
            rimg_ok = rt_pos.transform.Find("status/ok").GetComponent<RawImage>();
            rimg_no = rt_pos.transform.Find("status/no").GetComponent<RawImage>();
            rimg_none = rt_pos.transform.Find("status/none").GetComponent<RawImage>();
            trad = transform.GetComponentInChildren<Traductable>(true);

            base.Awake();
        }

        //----------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            gameObject.SetActive(false);
        }

        //----------------------------------------------------------------------------------------------------------

        void RaycastUnder<T>(in IDraggable<T> draggable, in PointerEventData eventData)
        {
            results.Clear();
            SguiGlobal.instance.raycaster_2D.Raycast(eventData, results);

            bool found_acceptor = false;
            bool can_accept = false;

            for (int i = 0; i < results.Count; i++)
            {
                IAcceptDraggable<T> handler = results[i].gameObject.GetComponentInParent<IAcceptDraggable<T>>(true);
                if (handler != null)
                {
                    found_acceptor = true;
                    if (handler.CanAcceptDraggable(draggable))
                    {
                        can_accept = true;
                        break;
                    }
                }
            }

            rimg_ok.gameObject.SetActive(found_acceptor && can_accept);
            rimg_no.gameObject.SetActive(found_acceptor && !can_accept);
            rimg_none.gameObject.SetActive(!found_acceptor);

            results.Clear();
        }
    }
}