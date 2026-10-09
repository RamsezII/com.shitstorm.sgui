using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed class ContextListHandler : MonoBehaviour, SguiContextList.IUser
    {
        public bool leftClick = false;
        public bool rightClick = true;
        bool SguiContextList.IUser.AcceptsLeftClick => leftClick;
        bool SguiContextList.IUser.AcceptsRightClick => rightClick;
        public Action<PointerEventData, ContextList> callback;
        void SguiContextList.IUser.OnSguiContextClick(PointerEventData eventData, ContextList list) => callback?.Invoke(eventData, list);
    }
}