using _ARK_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class SGUI_global : MonoBehaviour
    {
        public static SGUI_global instance;

        public Canvas canvas;
        public RectTransform rT;

        [SerializeField] RectTransform rT_scheduler;
        [SerializeField] TextMeshProUGUI txt_scheduler;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            Util.InstantiateOrCreateIfAbsent<SGUI_global>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            canvas = GetComponent<Canvas>();
            rT = (RectTransform)transform.Find("rT");

            rT_scheduler = (RectTransform)transform.Find("rT_scheduler");
            txt_scheduler = rT_scheduler.Find("text").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            NUCLEOR.instance.scheduler.list.AddListener1(isNotEmpty =>
            {
                NUCLEOR.delegates.onLateUpdate -= OnLateUpdateSchedulerInfos;
                if (isNotEmpty)
                    NUCLEOR.delegates.onLateUpdate += OnLateUpdateSchedulerInfos;
                rT_scheduler.gameObject.SetActive(isNotEmpty);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnLateUpdateSchedulerInfos()
        {
            Schedulable schedulable = NUCLEOR.instance.scheduler.list._list[0];

            string progressBar = schedulable.progressBar;
            string text = $"{typeof(NUCLEOR).FullName}({Util_ark.GetRotator()})\n{schedulable.description}\n\n{progressBar}";

            txt_scheduler.text = text;
            rT_scheduler.sizeDelta = new(0, txt_scheduler.GetPreferredValues(txt_scheduler.text, rT.rect.width, float.PositiveInfinity).y);
        }
    }
}