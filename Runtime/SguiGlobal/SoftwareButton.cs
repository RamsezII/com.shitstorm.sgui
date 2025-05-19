using _UTIL_;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SoftwareButton : OSButton, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        Button button;
        TMP_Dropdown dropdown;
        [SerializeField] RawImage[] img_instances;

        public readonly ListListener<SguiWindow1> instances = new();

        public Type software_type;

        public Action<PointerEventData>
            onClickAction,
            onPointerDown,
            onPointerUp;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            button = GetComponent<Button>();
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            img_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            instances.AddListener2(this, list =>
            {
                for (int i = 0; i < img_instances.Length; i++)
                    img_instances[i].gameObject.SetActive(list.Count > i);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onClickAction?.Invoke(eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke(eventData);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(eventData);
        }
    }
}