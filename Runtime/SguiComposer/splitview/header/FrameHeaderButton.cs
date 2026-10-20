using _ARK_;
using _SGUI_.context_click;
using _UTIL_;
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
        [SerializeField] Graphic graphic_arrow, graphic_toggle;
        [SerializeField] TextMeshProUGUI tmpro;
        public readonly ValueNotifier<bool> toggle = new();
        public Action<ContextList> onList;
        bool SguiContextList.IUser.AcceptsLeftClick => true;
        bool SguiContextList.IUser.AcceptsRightClick => false;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize(in bool show_arrow, in bool use_toggle)
        {
            tmpro.margin = new Vector4(10, 0, show_arrow ? 15 : 10, 0);
            graphic_arrow.gameObject.SetActive(show_arrow);

            if (use_toggle)
            {
                toggle.AddListener(graphic_toggle.gameObject.SetActive);
                button.onClick.AddListener(toggle.ToggleAuto);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        void SguiContextList.IUser.OnSguiContextClick(PointerEventData eventData, ContextList list)
        {
            onList?.Invoke(list);
        }
    }
}