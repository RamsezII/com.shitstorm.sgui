using _ARK_;
using _SGUI_.context_click;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    public sealed partial class FrameHeaderButton : MonoBehaviour, SguiContextList.IUser
    {
        public Button button;
        public Traductable label;
        [SerializeField] GameObject go_arrow;
        [SerializeField] TextMeshProUGUI tmpro;
        public Action<ContextList> onList;
        bool SguiContextList.IUser.AcceptsLeftClick => true;
        bool SguiContextList.IUser.AcceptsRightClick => false;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize(bool arrow)
        {
            tmpro.margin = new Vector4(10, 0, arrow ? 15 : 10, 0);
            go_arrow.SetActive(arrow);
        }

        void SguiContextList.IUser.OnSguiContextClick(PointerEventData eventData, ContextList list)
        {
            onList?.Invoke(list);
        }
    }
}