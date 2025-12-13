using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed partial class SguiTerminal : SguiWindow1
    {
        public RectTransform rt_shellview;

        //--------------------------------------------------------------------------------------------------------------

        internal static void AddSoftwareButton()
        {
            var software_button = OSView.instance.AddSoftwareButton<SguiTerminal>(new("Terminal"));

            software_button.onClick_left_empty = eventData => OnClickLeft();

            software_button.onClick_left_notEmpty = eventData =>
            {
                OnClickLeft();
                return false;
            };

            software_button.onRightClickhandler += OnRightClick;

            void OnClickLeft()
            {

            }

            void OnRightClick(in PointerEventData eventData, ref bool enable_AddWindow, ref bool enable_CloseAll, in List<Action<SguiContextClick_List_Button>> addButtons)
            {
                enable_AddWindow = false;

                addButtons.Add(button =>
                {
                    button.trad.SetTrads(new()
                    {
                        french = $"Ajouter une fenêtre",
                        english = $"Add a window",
                    });

                    button.SetupSublist(sublist =>
                    {
                        sublist.AddButton().trad.SetTrad("BOA");
                        sublist.AddButton().trad.SetTrad("ZOA");
                        sublist.AddButton().trad.SetTrad("AST");
                    });
                });
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            rt_shellview = (RectTransform)transform.Find("rT/body/_SGUI_.ShellView");
            base.OnAwake();
            trad_title.SetTrad("SguiTerminal");
        }
    }
}