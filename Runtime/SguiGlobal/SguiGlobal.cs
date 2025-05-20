using _ARK_;
using _UTIL_;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiGlobal : MonoBehaviour
    {
        public static SguiGlobal instance;

        public Camera cameraUI;
        public Canvas canvas2D, canvas3D;
        public RectTransform rT_2D, rT_3D;

        [SerializeField] RectTransform rT_header, rT_footer, rT_scheduler;
        [SerializeField] TextMeshProUGUI txt_scheduler;

        public readonly ListListener osview_users = new();

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

            cameraUI = transform.Find("CameraUI").GetComponent<Camera>();
            canvas3D = cameraUI.transform.Find("Canvas3D").GetComponent<Canvas>();
            rT_3D = (RectTransform)canvas3D.transform.Find("rT");

            canvas2D = transform.Find("Canvas2D").GetComponent<Canvas>();
            rT_2D = (RectTransform)canvas2D.transform.Find("rT");

            rT_header = (RectTransform)canvas2D.transform.Find("_SGUI_.OSView/header");
            rT_footer = (RectTransform)canvas2D.transform.Find("_SGUI_.OSView/task-bar");

            rT_scheduler = (RectTransform)canvas2D.transform.Find("scheduler");
            txt_scheduler = rT_scheduler.Find("text").GetComponent<TextMeshProUGUI>();

            AwakeButtons();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            rT_footer.Find("main-button").GetComponent<Button>().onClick.AddListener(OSMainMenu.instance.Toggle);

            StartButtons();

            osview_users.AddListener1(this, not_empty =>
            {
                rT_header.gameObject.SetActive(not_empty);
                rT_footer.gameObject.SetActive(not_empty);
                canvas2D.transform.Find("button-play").gameObject.SetActive(not_empty);
            });

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

        //--------------------------------------------------------------------------------------------------------------

        void OnDestroy()
        {
            if (this == instance)
                instance = null;
        }
    }
}