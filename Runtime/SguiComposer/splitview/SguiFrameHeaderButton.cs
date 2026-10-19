using _ARK_;
using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_.composer
{
    public sealed partial class SguiFrameHeaderButton : MonoBehaviour, SguiContextList.IUser
    {
        public Traductable label;
        public Action<PointerEventData, ContextList> onContextList;
        bool SguiContextList.IUser.AcceptsLeftClick => true;
        bool SguiContextList.IUser.AcceptsRightClick => false;

        //----------------------------------------------------------------------------------------------------------

        void SguiContextList.IUser.OnSguiContextClick(PointerEventData eventData, ContextList list)
        {
            onContextList?.Invoke(eventData, list);
        }
    }
}