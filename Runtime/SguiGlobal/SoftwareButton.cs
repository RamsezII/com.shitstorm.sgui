using System;
using System.Collections.Generic;
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

        readonly HashSet<SguiWindow1> instances = new();

        public Type software_type;

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
            button.onClick.AddListener(OnClick);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnClick()
        {
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("click: " + eventData.button, this);

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right:
                    dropdown.Show();
                    break;

                case PointerEventData.InputButton.Left:
                    {
                        SguiWindow1 instance = (SguiWindow1)SguiWindow.InstantiateWindow(software_type);
                        instance.gameObject.SetActive(true);
                        instances.Add(instance);
                    }
                    break;
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("up: " + eventData.button, this);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("down: " + eventData.button, this);
        }
    }
}