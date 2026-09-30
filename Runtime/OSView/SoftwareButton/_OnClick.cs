using _SGUI_.context_click;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SoftwareButton : IPointerClickHandler
    {
        public Action<PointerEventData> onClick_left_empty, onClick_middle;
        public Func<PointerEventData, bool> onClick_left_notEmpty, onClick_right;
        public delegate void OnRightClickhandler(in PointerEventData eventData, ref bool enable_AddWindow, ref bool enable_CloseAll, in List<Action<ContextListButton>> addButtons);
        public OnRightClickhandler onRightClickhandler;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            SguiContextHover.instance.UnassignUser(this);
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (users.IsEmpty)
                    {
                        if (onClick_left_empty == null) InstantiateSoftware();
                        else onClick_left_empty(eventData);
                    }
                    else if (onClick_left_notEmpty == null || onClick_left_notEmpty(eventData))
                    {
                        int focused = users._collection.FindIndex(frame => frame.isFocused._value);
                        if (users._collection.Count == 1 && focused == 0)
                        {
                            var composer = users._collection[0].composer;
                            composer.SetScalePivot(this);
                            composer.toggle.Value = false;
                        }
                        else users._collection[(focused + 1) % users._collection.Count].TakeFocus();
                    }
                    break;
                case PointerEventData.InputButton.Middle:
                    onClick_middle?.Invoke(eventData);
                    break;
                case PointerEventData.InputButton.Right:
                    if (onClick_right != null && !onClick_right(eventData)) return;
                    bool enable_AddWindow = true, enable_CloseAll = true;
                    var list = SguiContextList.instance.InstantiateListAtScreenPoint(eventData.position, eventData.pressEventCamera);
                    List<Action<ContextListButton>> onButtons = new();
                    onRightClickhandler?.Invoke(eventData, ref enable_AddWindow, ref enable_CloseAll, onButtons);
                    if (enable_AddWindow)
                    {
                        var tab = list.AddButton_trad(new() { french = "Ouvrir un nouvel onglet", english = "Open new tab" });
                        tab._button.onClick.AddListener(() => InstantiateSoftware());
                        var window = list.AddButton_trad(new() { french = "Ouvrir dans une nouvelle fenêtre", english = "Open in new window" });
                        window._button.onClick.AddListener(() => InstantiateSoftware(true));
                    }
                    foreach (var onButton in onButtons) onButton(list.AddButton_trad(default));
                    foreach (var frame in users._collection)
                    {
                        var button = list.AddButton_trad(frame.tab.trad_title.traductions);
                        button._button.onClick.AddListener(frame.TakeFocus);
                    }
                    if (enable_CloseAll)
                    {
                        var close = list.AddButton_trad(new() { french = "Fermer tous les onglets de ce logiciel", english = "Close all tabs of this software" });
                        close._button.onClick.AddListener(() =>
                        {
                            foreach (var frame in users._collection.ToArray()) if (frame != null) frame.RequestClose();
                        });
                    }
                    break;
            }
        }
    }
}
