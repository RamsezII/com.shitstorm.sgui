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
        [SerializeField] RectTransform rt_box;
        [SerializeField] Button button_close, button_clear, button_category;
        [SerializeField] Traductable trad_title;
        [SerializeField] internal TextMeshProUGUI text_category;
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
            input_search.text = string.Empty;

            base.OnDisable();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            prefab_item.gameObject.SetActive(false);
            button_category.gameObject.SetActive(false);

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

        void ClearClones()
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

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            NUCLEOR.delegates.LateUpdate -= RefreshAlpha;
            base.OnDestroy();
        }
    }
}
