using _ARK_;
using _SGUI_.searchbox;
using _UTIL_;
using System.Collections.Generic;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiSearchbox : ArkComponent1
    {
        [AutoStaticsCleanup] public static SguiSearchbox instance;

        CanvasGroup canvasGroup;
        [SerializeField] Button button_close, button_clear;
        [SerializeField] Traductable trad_title;
        [SerializeField] ScrollRect scrollview;
        [SerializeField] VerticalLayoutGroup vlayout;
        [SerializeField] TMP_InputField input_search;
        [SerializeField] SearchboxItem prefab_item;

        readonly List<SearchboxItem> clones = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            instance = this;

            canvasGroup = GetComponent<CanvasGroup>();

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            trad_title.SetText(GetType().FullName);
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

            prefab_item.gameObject.SetActive(false);

            transform.Find("background").GetComponent<PointerClickHandler>().onClick += eventData =>
            {
                switch (eventData.button)
                {
                    case PointerEventData.InputButton.Left:
                        Close();
                        break;

                    case PointerEventData.InputButton.Right:
                        ArkUI.instance.SetScreenPosition(transform.AsRTfm(), eventData.position, eventData.pressEventCamera);
                        break;
                }
            };

            button_close.onClick.AddListener(Close);

            button_clear.onClick.AddListener(() => input_search.text = string.Empty);

            Close();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ClearClones()
        {
            foreach (var item in clones)
                Destroy(item.gameObject);
            clones.Clear();
        }

        void RefreshAlpha()
        {
            float a = Mathf.MoveTowards(canvasGroup.alpha, 1, 10 * Time.deltaTime);
            canvasGroup.alpha = a;
            if (a >= 1)
                NUCLEOR.delegates.LateUpdate -= RefreshAlpha;
        }

        public void AutoSize() => Util.AddActionOnce(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, _AutoSize_Immediate);
        internal void _AutoSize_Immediate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);
            float h = vlayout.preferredHeight;
            scrollview.content.sizeDelta = new Vector2(0, h);
        }

        public void Close() => Toggle(false);
        void Toggle(in bool toggle)
        {
            ClearClones();
            gameObject.SetActive(toggle);
        }

        public void OpenAtScreenPoint(in Vector2 screenPosition, in Traductions title, Camera eventCamera = null)
        {
            Toggle(true);
            trad_title.SetTraductions(title);
            ArkUI.instance.SetScreenPosition(transform.AsRTfm(), screenPosition, eventCamera);
        }

        public SearchboxItem AddItem()
        {
            var clone = prefab_item.Clone(true);
            clone.button.onClick.AddListener(Close);
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
