using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.context_click
{
    public enum SguiListTypes : byte
    {
        Normal,
        StayOpen,
        MultiSelect,
    }

    public sealed class ContextList : ArkComponent1
    {
        CanvasGroup canvasGroup;
        public RectTransform prt, rt;
        public ContextList sublist;
        public ScrollRect scrollview;
        public VerticalLayoutGroup vlayout;
        public Traductable target_trad;
        [SerializeField] ContextListButton prefab_button;
        [SerializeField] RectTransform prefab_line;
        public readonly List<ContextListButton> buttons_clones = new();
        public readonly ValueNotifier<ContextListButton> last_button_toggled = new();
        public readonly HashSetListener<ContextListButton> buttons_toggled = new();
        public SguiListTypes type;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            scrollview = GetComponentInChildren<ScrollRect>();
            rt = (RectTransform)scrollview.transform.parent;
            prt = (RectTransform)rt.parent;
            vlayout = GetComponentInChildren<VerticalLayoutGroup>();
            prefab_button = GetComponentInChildren<ContextListButton>();
            prefab_line = (RectTransform)transform.Find("rt/scroll-view/viewport/content/layout/trait");

            canvasGroup.alpha = 0;

            GetComponentInChildren<PointerClickHandler>().onClick += eventData =>
            {
                var raycaster = GetComponentInParent<GraphicRaycaster>();
                List<RaycastResult> rc_results = new();
                raycaster.Raycast(eventData, rc_results);

                if (rc_results.Count > 0)
                    for (int i = 0; i < rc_results.Count; i++)
                    {
                        ContextListButton button = rc_results[i].gameObject.GetComponentInParent<ContextListButton>();
                        if (button != null)
                            if (button.plist.sublist != null)
                            {
                                Destroy(button.plist.sublist.gameObject);
                                return;
                            }
                    }

                Destroy(SguiContextList.instance.scrollview_lastRootList.gameObject);
            };

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            if (buttons_clones.Count == 0)
                Destroy(gameObject);

            prefab_line.gameObject.SetActive(false);
            prefab_button.gameObject.SetActive(false);
            AutoSizeAndMove();

            canvasGroup.alpha = 0;

            Scheduler.Operation op = default;
            NUCLEOR.instance.scheduler_unscaled.AddOperation(op = new("lerp contextlist alpha", 0, true, () =>
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

        public void AddLine() => prefab_line.Clone(true);

        public ContextListButton AddButton_string(in string label) => AddButton_trad(new Traductions(label));
        public ContextListButton AddButton_trad(in Traductions label)
        {
            var clone = prefab_button.Clone(true);
            clone.index = buttons_clones.Count - 1;
            clone.trad.SetTraductions(label);
            buttons_clones.Add(clone);

            clone._button.onClick.AddListener(() =>
            {
                if (type == SguiListTypes.MultiSelect)
                {
                    clone.toggle.ToggleAuto();
                    if (target_trad != null)
                        if (buttons_toggled.IsEmpty)
                            target_trad.SetTraductions(new() { french = "Rien", english = "None", });
                        else if (buttons_toggled._collection.Count == buttons_clones.Count)
                            target_trad.SetTraductions(new() { french = "Tout", english = "All", });
                        else
                            target_trad.SetTraductions(new()
                            {
                                french = buttons_toggled._collection.Select(button => button.trad.traductions.french).Join(", "),
                                english = buttons_toggled._collection.Select(button => button.trad.traductions.english).Join(", "),
                            });
                }

                if (type == SguiListTypes.Normal)
                    Destroy(SguiContextList.instance.scrollview_lastRootList.gameObject);
            });

            if (didStart)
                AutoSizeAndMove();

            return clone;
        }

        public void AutoSizeAndMove() => Util.AddActionOnce(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, AutoSizeAndMove_now);
        public void AutoSizeAndMove_now()
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