using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SoftwareButton : OSButton
    //public class SoftwareButton : OSButton, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        Button button;
        [SerializeField] RawImage[] img_instances;

        readonly HashSet<SguiWindow1> instances = new();

        public SguiWindow1 software_prefab;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            button = GetComponent<Button>();
            img_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            button.onClick.AddListener(OnClick);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnClick()
        {
        }

        //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        //{
        //    Debug.Log("click: " + eventData.button, this);
        //    button.OnPointerClick(eventData);
        //}

        //void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        //{
        //    Debug.Log("up: " + eventData.button, this);
        //    button.OnPointerUp(eventData);
        //}

        //void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        //{
        //    Debug.Log("down: " + eventData.button, this);
        //    button.OnPointerDown(eventData);
        //}
    }
}