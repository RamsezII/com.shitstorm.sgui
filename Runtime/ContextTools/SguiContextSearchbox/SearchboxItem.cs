using _ARK_;
using _SGUI_.context_click;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.searchbox
{
    public sealed class SearchboxItem : MonoBehaviour, SguiContextHover.IUser, SguiContextList.IUser
    {
        [NonSerialized] public SguiSearchbox pbox;

        [SerializeField] internal Button button;
        public TextMeshProUGUI label;
        [SerializeField] Graphic arrow;

        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        public Action<ContextList> onContextList;
        void SguiContextList.IUser.OnSguiContextClick(PointerEventData eventData, ContextList list) => onContextList?.Invoke(list);

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            pbox = GetComponentInParent<SguiSearchbox>(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void ShowArrow()
        {
            arrow.gameObject.SetActive(true);
            label.margin = new(5, 0, 10, 0);
        }
    }
}