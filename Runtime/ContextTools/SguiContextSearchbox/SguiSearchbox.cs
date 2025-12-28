using _ARK_;
using _SGUI_.searchbox;
using _UTIL_;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiSearchbox : ArkComponent
    {
        public static SguiSearchbox instance;

        [SerializeField] CanvasGroup canvasGroup;
        RectTransform rt;
        public Traductable trad_title;
        ScrollRect scrollview;
        VerticalLayoutGroup vlayout;
        public TMP_InputField input_search;
        [SerializeField] SearchboxItem prefab_item;

        readonly List<SearchboxItem> clones = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            instance = this;

            base.Awake();

            canvasGroup = GetComponent<CanvasGroup>();
            rt = (RectTransform)transform.Find("rT");
            trad_title = transform.Find("rT/margin/title").GetComponent<Traductable>();
            scrollview = GetComponentInChildren<ScrollRect>(true);
            vlayout = scrollview.content.GetComponentInChildren<VerticalLayoutGroup>(true);
            input_search = GetComponentInChildren<TMP_InputField>(true);
            prefab_item = vlayout.GetComponentInChildren<SearchboxItem>(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            trad_title.SetTrad(GetType().FullName);
            canvasGroup.alpha = 0;
            NUCLEOR.delegates.LateUpdate += RefreshAlpha;
        }

        protected override void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= RefreshAlpha;
            base.OnDisable();
            input_search.text = string.Empty;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            prefab_item.button.onClick.AddListener(Close);
            prefab_item.gameObject.SetActive(false);

            transform.Find("background").GetComponent<PointerClickHandler>().onClick += eventData =>
            {
                switch (eventData.button)
                {
                    case PointerEventData.InputButton.Left:
                        Close();
                        break;

                    case PointerEventData.InputButton.Right:
                        rt.position = eventData.position;
                        break;
                }
            };

            transform.Find("rT/margin/button-close").GetComponent<Button>().onClick.AddListener(Close);

            input_search.transform.Find("clear-icon").GetComponent<Button>().onClick.AddListener(() => input_search.text = string.Empty);

            Close();
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshAlpha()
        {
            float a = Mathf.MoveTowards(canvasGroup.alpha, 1, 10 * Time.deltaTime);
            canvasGroup.alpha = a;
            if (a >= 1)
                NUCLEOR.delegates.LateUpdate -= RefreshAlpha;
        }

        public void AutoSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, _AutoSize_Immediate);
        internal void _AutoSize_Immediate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);
            float h = vlayout.preferredHeight;
            scrollview.content.sizeDelta = new Vector2(0, h);
        }

        public void Close() => Toggle(false);
        void Toggle(in bool toggle)
        {
            foreach (var item in clones)
                Destroy(item.gameObject);
            clones.Clear();
            gameObject.SetActive(toggle);
        }

        public void OpenHere(in Vector2 screenPosition)
        {
            Toggle(true);
            rt.position = screenPosition;
        }

        public SearchboxItem AddItem()
        {
            var clone = prefab_item.Clone(true);
            clones.Add(clone);
            AutoSize();
            return clone;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            NUCLEOR.delegates.LateUpdate -= RefreshAlpha;
            base.OnDestroy();
        }
    }
}