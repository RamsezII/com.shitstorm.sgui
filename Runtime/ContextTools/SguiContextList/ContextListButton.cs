using _ARK_;
using _UTIL_;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.context_click
{
    public sealed class ContextListButton : MonoBehaviour, SguiContextHover.IUser
    {
        public ContextList plist;
        public int index;
        public Button _button;
        public Traductable label_left, label_right;
        public readonly ValueNotifier<bool> toggle = new();
        [SerializeField] internal RawImage arrow, checkmark;
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            plist = GetComponentInParent<ContextList>();

            arrow.gameObject.SetActive(false);

            toggle.AddListener(value =>
            {
                checkmark.gameObject.SetActive(value);

                plist.buttons_toggled.ToggleElement(this, value);

                if (value)
                    plist.last_button_toggled.Value = this;
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetupSublist(Action<ContextList> onSublist)
        {
            _button.onClick.RemoveAllListeners();
            arrow.gameObject.SetActive(true);

            _button.onClick.AddListener(() =>
            {
                if (plist.sublist != null)
                    Destroy(plist.sublist.gameObject);

                var sublist = plist.sublist = Instantiate(SguiContextList.instance.prefab_list, plist.transform);

                sublist.gameObject.SetActive(true);

                Util.GetWorldCorners(transform.AsRTfm(), out _, out Vector3 max);
                sublist.rt.position = max;
                sublist.rt.anchoredPosition += new Vector2(0, plist.vlayout.padding.top);

                onSublist(sublist);
            });
        }
    }
}
