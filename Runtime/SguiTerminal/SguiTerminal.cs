using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed partial class SguiTerminal : SguiWindow1
    {
        public ShellView shellView;
        public RectTransform prt_shellView, rt_shellview;
        public static Action<SguiContextClick_List> onSoftwareButtonAddWindowList;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            onSoftwareButtonAddWindowList = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        internal static void AddSoftwareButton()
        {
            var software_button = OSView.instance.AddSoftwareButton<SguiTerminal>(new("Terminal"));

            software_button.onClick_left_empty = eventData => OnLeftClick(eventData.position);

            software_button.onRightClickhandler += OnRightClick;

            void OnLeftClick(in Vector2 mousePos)
            {
                var list = SguiContextClick.instance.RightClickHere(mousePos);
                onSoftwareButtonAddWindowList(list);
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

                    button.SetupSublist(onSoftwareButtonAddWindowList);
                });
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            shellView = GetComponentInChildren<ShellView>(true);
            prt_shellView = (RectTransform)transform.Find("rT/body");
            rt_shellview = (RectTransform)transform.Find("rT/body/_SGUI_.ShellView");
            base.OnAwake();
            trad_title.SetTrad("SguiTerminal");
        }
    }
}