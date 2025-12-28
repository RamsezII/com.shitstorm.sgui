using _ARK_;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiDragManager : ArkComponent
    {
        public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler
        {
            string DragDisplay { get; }
            object DragData { get; }

            void OnDropAccepted(in IAcceptDraggable acceptor);

            void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
            {
                instance.gameObject.SetActive(true);
                instance.text_label.text = DragDisplay;
                instance.rt_size.sizeDelta = instance.text_label.GetPreferredValues();
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

        public interface IAcceptDraggable : IDropHandler
        {
            virtual Type AcceptedDropType => null;
            bool TryAcceptDraggable(in IDraggable draggable, in bool onDrop);
            void IDropHandler.OnDrop(PointerEventData eventData)
            {
                if (eventData.pointerDrag.TryGetComponent<IDraggable>(out var draggable))
                {
                    TryAcceptDraggable(draggable, true);
                    draggable.OnDropAccepted(this);
                }
                instance.gameObject.SetActive(false);
            }
        }

        public static SguiDragManager instance;

        [SerializeField] RectTransform rt_pos, rt_size;
        [SerializeField] RawImage rimg_ok, rimg_no, rimg_none;
        [SerializeField] TextMeshProUGUI text_label;

        readonly List<RaycastResult> results = new();

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            instance = this;

            rt_pos = (RectTransform)transform.Find("rt_pos");
            rt_size = (RectTransform)rt_pos.transform.Find("rt_size");
            rimg_ok = rt_size.transform.Find("status/ok").GetComponent<RawImage>();
            rimg_no = rt_size.transform.Find("status/no").GetComponent<RawImage>();
            rimg_none = rt_size.transform.Find("status/none").GetComponent<RawImage>();
            text_label = rt_size.Find("text").GetComponent<TextMeshProUGUI>();

            base.Awake();
        }

        //----------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            gameObject.SetActive(false);
        }

        //----------------------------------------------------------------------------------------------------------

        void RaycastUnder(in IDraggable draggable, in PointerEventData eventData)
        {
            results.Clear();
            SguiGlobal.instance.raycaster_2D.Raycast(eventData, results);

            bool found = false;
            bool accepts = false;

            for (int i = 0; i < results.Count; i++)
            {
                IAcceptDraggable handler = results[i].gameObject.GetComponentInParent<IAcceptDraggable>(true);
                if (handler != null)
                {
                    found = true;
                    if (handler.TryAcceptDraggable(draggable, false))
                    {
                        accepts = true;
                        break;
                    }
                }
            }

            rimg_ok.gameObject.SetActive(found && accepts);
            rimg_no.gameObject.SetActive(found && !accepts);
            rimg_none.gameObject.SetActive(!found);

            results.Clear();
        }
    }
}