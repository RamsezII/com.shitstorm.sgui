using _ARK_;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.Monitor.Processes
{
    public class SectionColumn : MonoBehaviour, SguiContextHover.IUser, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler, IDragHandler
    {
        public ProcessesSorters sorters;
        public RectTransform rt;
        internal Toggle toggle;
        internal RawImage rimg_arrow;
        public Traductable trad;
        public Traductions hover_infos;
        public int column_index;

        public Action<bool> onIsAscendingOrder;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            sorters = GetComponentInParent<ProcessesSorters>(includeInactive: true);
            column_index = 0;
            rt = (RectTransform)transform;
            toggle = GetComponent<Toggle>();
            rimg_arrow = transform.Find("arrow").GetComponent<RawImage>();
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
            SguiContextHover.instance.AssignUser(this);
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            SguiContextHover.instance.AssignUser(this);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (column_index == 0)
                return;

            RectTransform prev_rt = sorters.columns[column_index - 1].rt;

            float w = prev_rt.sizeDelta.x;
            w += eventData.delta.x;
            w = Mathf.Clamp(w, 20, 200);
            prev_rt.sizeDelta = new Vector2(w, sorters.init_height);

            sorters.section.RefreshColumnWidth(column_index - 1);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            SguiContextHover.instance.UnassignUser(this);
        }
    }
}