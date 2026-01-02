using _ARK_;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.context_click
{
    public sealed class ContextListButton : MonoBehaviour, SguiContextHover.IUser
    {
        public ContextList plist;
        public RectTransform rt;
        public Button button;
        public Traductable trad;
        [SerializeField] RawImage arrow;
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            plist = GetComponentInParent<ContextList>();
            rt = (RectTransform)transform;
            button = GetComponentInChildren<Button>();
            trad = GetComponentInChildren<Traductable>();
            arrow = transform.Find("arrow").GetComponent<RawImage>();

            arrow.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetupSublist(Action<ContextList> onSublist)
        {
            button.onClick.RemoveAllListeners();
            arrow.gameObject.SetActive(true);

            button.onClick.AddListener(() =>
            {
                if (plist.sublist != null)
                    Destroy(plist.sublist.gameObject);

                var sublist = plist.sublist = Instantiate(SguiContextClick.instance.prefab_list, plist.transform);

                sublist.gameObject.SetActive(true);

                Util.GetWorldCorners(rt, out _, out Vector2 max);
                sublist.rt.position = max;
                sublist.rt.anchoredPosition += new Vector2(0, plist.vlayout.padding.top);

                onSublist(sublist);
            });
        }
    }
}