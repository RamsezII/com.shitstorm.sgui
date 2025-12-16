using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed class SguiContextClick_List : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        public RectTransform prt, rt;
        public SguiContextClick_List sublist;
        public ScrollRect scrollview;
        public VerticalLayoutGroup vlayout;
        [SerializeField] SguiContextClick_List_Button prefab_button;
        [SerializeField] GameObject prefab_line;
        public readonly List<SguiContextClick_List_Button> buttons_clones = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            scrollview = GetComponentInChildren<ScrollRect>();
            rt = (RectTransform)scrollview.transform.parent;
            prt = (RectTransform)rt.parent;
            vlayout = GetComponentInChildren<VerticalLayoutGroup>();
            prefab_button = GetComponentInChildren<SguiContextClick_List_Button>();
            prefab_line = transform.Find("rt/scroll-view/viewport/content/layout/trait").gameObject;

            canvasGroup.alpha = 0;

            GetComponentInChildren<PointerClickHandler>().onClick += eventData =>
            {
                var raycaster = GetComponentInParent<GraphicRaycaster>();
                List<RaycastResult> rc_results = new();
                raycaster.Raycast(eventData, rc_results);

                if (rc_results.Count > 0)
                    for (int i = 0; i < rc_results.Count; i++)
                    {
                        SguiContextClick_List_Button button = rc_results[i].gameObject.GetComponentInParent<SguiContextClick_List_Button>();
                        if (button != null)
                            if (button.plist.sublist != null)
                            {
                                Destroy(button.plist.sublist.gameObject);
                                return;
                            }
                    }

                Destroy(SguiContextClick.instance.scrollview_lastRootList.gameObject);
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            if (buttons_clones.Count == 0)
                Destroy(gameObject);

            prefab_line.gameObject.SetActive(false);
            prefab_button.gameObject.SetActive(false);
            AutoSizeAndMove();

            canvasGroup.alpha = 0;

            HeartBeat.Operation op = default;
            NUCLEOR.instance.heartbeat_unscaled.AddOperation(op = new(0, true, () =>
            {
                if (this == null || canvasGroup.alpha >= 1)
                {
                    op.Dispose();
                    return;
                }
                canvasGroup.alpha += 7.5f * Time.unscaledDeltaTime;
            }));
        }

        //--------------------------------------------------------------------------------------------------------------

        public void AddLine() => prefab_line.Clone().SetActive(true);

        public SguiContextClick_List_Button AddButton()
        {
            var clone = prefab_button.Clone();
            buttons_clones.Add(clone);

            clone.gameObject.SetActive(true);
            clone.button.onClick.AddListener(() => Destroy(SguiContextClick.instance.scrollview_lastRootList.gameObject));

            if (didStart)
                AutoSizeAndMove();

            return clone;
        }

        public void AutoSizeAndMove()
        {
            if (gameObject == null)
                return;

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);

            scrollview.content.sizeDelta = new(0, vlayout.preferredHeight);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, Mathf.Min(300, vlayout.preferredHeight));

            Util.GetWorldCorners(prt, out Vector2 pmin, out Vector2 pmax);
            Util.GetWorldCorners(rt, out Vector2 min, out Vector2 max);

            Vector2 pos = rt.position;

            for (int i = 0; i < 2; ++i)
            {
                if (min[i] < pmin[i])
                    pos[i] += pmin[i] - min[i];
                if (max[i] > pmax[i])
                    pos[i] += pmax[i] - max[i];
            }

            rt.position = pos;
        }
    }
}