using _ARK_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed class SguiGlobal : MonoBehaviour
    {
        public static SguiGlobal instance;

        public Canvas canvas2D, canvas3D;
        public RectTransform rT_2D;

        [SerializeField] RectTransform rT_scheduler;
        [SerializeField] TextMeshProUGUI txt_scheduler;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            Util.InstantiateOrCreateIfAbsent<SguiGlobal>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            canvas2D = transform.Find("Canvas2D").GetComponent<Canvas>();
            rT_2D = (RectTransform)canvas2D.transform.Find("rT");

            rT_scheduler = (RectTransform)canvas2D.transform.Find("rT_scheduler");
            txt_scheduler = rT_scheduler.Find("text").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            NUCLEOR.instance.scheduler.list.AddListener1(this, isNotEmpty =>
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

            float progress = schedulable.routine == null ? 0 : schedulable.routine.Current;

            float body_width = rT_scheduler.rect.width;
            float char_width = txt_scheduler.GetPreferredValues("_", body_width, float.PositiveInfinity).x;
            int max_chars = (int)(body_width / char_width);

            int bar_count = max_chars - 9;
            int count = (int)(Mathf.Clamp01(progress) * bar_count);

            txt_scheduler.text = $"{schedulable.description}\n({Util_ark.GetRotator()}) {new string('▓', count)}{new string('░', bar_count - count)} {Mathf.RoundToInt(100 * progress),3}%";
            rT_scheduler.sizeDelta = new(0, txt_scheduler.GetPreferredValues(txt_scheduler.text, rT_2D.rect.width, float.PositiveInfinity).y);
        }
    }
}