using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed class ContextListHandler : MonoBehaviour, SguiContextList.IUser
    {
        public Action<ContextList> callback;
        void SguiContextList.IUser.OnSguiContextClick(PointerEventData eventData, ContextList list) => callback?.Invoke(list);
    }
}