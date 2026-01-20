using _SGUI_.context_click;
using System;
using UnityEngine;

namespace _SGUI_
{
    public sealed class ContextListHandler : MonoBehaviour, SguiContextClick.IUser
    {
        public Action<ContextList> callback;
        void SguiContextClick.IUser.OnSguiContextClick(ContextList list) => callback?.Invoke(list);
    }
}