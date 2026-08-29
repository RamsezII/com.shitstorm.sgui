using _ARK_;
using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Dropdown_Normal : SguiCustom_Abstract
    {
        public Button _button;
        [SerializeField] protected Traductable trad_button;
        public Action<ContextList> onContextList;
        [SerializeField] ContextList currentList;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            _button = GetComponentInChildren<Button>(true);
            trad_button = _button.GetComponentInChildren<Traductable>(true);

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            _button.onClick.AddListener(() =>
            {
                currentList = SguiContextList.instance.InstantiateListHere(rT.position);

                switch (this)
                {
                    case SguiCustom_Dropdown_MultiSelect:
                        currentList.type = SguiListTypes.MultiSelect;
                        break;

                    case SguiCustom_Dropdown_StayOpen:
                        currentList.type = SguiListTypes.StayOpen;
                        currentList.last_button_toggled.AddListener((ContextListButton button) =>
                        {
                            if (button == null)
                                trad_button.SetText("None");
                            else
                                trad_button.SetTraductions(button.trad.traductions);
                        });
                        break;

                    default:
                        currentList.type = SguiListTypes.Normal;
                        break;
                }

                OnContextList(currentList);

                onContextList?.Invoke(currentList);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Hide()
        {
            if (currentList != null)
                Destroy(currentList.gameObject);
        }

        public void SelectButton(ContextListButton button)
        {
            if (currentList != null)
                currentList.SelectButton(button);

            if (button == null)
                trad_button.SetText("None");
            else
                trad_button.SetTraductions(button.trad.traductions);
        }

        protected virtual void OnContextList(ContextList sguilist)
        {
        }
    }
}