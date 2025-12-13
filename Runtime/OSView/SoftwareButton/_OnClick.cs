using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SoftwareButton : IPointerClickHandler
    {
        public Action<PointerEventData> onClick_left_empty, onClick_middle;
        public Func<PointerEventData, bool> onClick_left_notEmpty, onClick_right;
        public delegate void OnRightClickhandler(in PointerEventData eventData, ref bool enable_AddWindow, ref bool enable_CloseAll, in List<Action<SguiContextClick_List_Button>> addButtons);
        public OnRightClickhandler onRightClickhandler;

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            SguiContextHover.instance.UnassignUser(this);

            if (software_prefab == null)
            {
                LoggerOverlay.Log($"{nameof(software_prefab)} is null ({software_prefab})", this, logLevel: LoggerOverlay.LogLevel.Warning);
                return;
            }

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (software_instances.IsEmpty)
                    {
                        if (onClick_left_empty == null)
                            InstantiateSoftware();
                        else
                            onClick_left_empty(eventData);
                    }
                    else
                    {
                        if (onClick_left_notEmpty == null || onClick_left_notEmpty(eventData))
                            if (software_instances._collection.Count == 1)
                            {
                                SguiWindow instance = software_instances._collection[0];
                                instance.SetScalePivot(this);

                                if (instance.HasFocus())
                                    instance.ToggleWindow();
                                else
                                    instance.TakeFocus();
                            }
                    }
                    break;

                case PointerEventData.InputButton.Middle:
                    onClick_middle?.Invoke(eventData);
                    break;

                case PointerEventData.InputButton.Right:
                    {
                        bool enable_AddWindow = true, enable_CloseAll = true;
                        var list = SguiContextClick.instance.RightClickHere(eventData.position);
                        List<Action<SguiContextClick_List_Button>> onButtons = new();
                        onRightClickhandler?.Invoke(eventData, ref enable_AddWindow, ref enable_CloseAll, onButtons);

                        if (enable_AddWindow)
                        {
                            var button = list.AddButton();

                            button.trad.SetTrads(new()
                            {
                                french = $"Ouvrir une nouvelle fenêtre",
                                english = $"Open new window",
                            });

                            button.button.onClick.AddListener(() =>
                            {
                                InstantiateSoftware();
                            });
                        }

                        foreach (var onButton in onButtons)
                            onButton(list.AddButton());

                        for (int i = 0; i < software_instances._collection.Count; i++)
                        {
                            SguiWindow window = software_instances._collection[i];
                            if (window.trad_title == null)
                                LoggerOverlay.Log($"error trad: {window}", window, logLevel: LoggerOverlay.LogLevel.Warning);
                            else
                            {
                                var button = list.AddButton();
                                button.trad.SetTrads(window.sgui_description._value);
                                button.button.onClick.AddListener(() =>
                                {
                                    window.SetScalePivot(this);
                                    window.ToggleWindow(true);
                                    window.TakeFocus();
                                });
                            }
                        }

                        if (enable_CloseAll)
                        {
                            var button = list.AddButton();

                            button.trad.SetTrads(new()
                            {
                                french = "Fermer toutes les fenêtres",
                                english = "Close all windows",
                            });

                            button.button.onClick.AddListener(() =>
                            {
                                for (int i = software_instances._collection.Count - 1; i >= 0; --i)
                                {
                                    SguiWindow window = software_instances._collection[i];
                                    window.SetScalePivot(null);
                                    window.Oblivionize();
                                }
                            });
                        }
                    }
                    break;
            }
        }
    }
}