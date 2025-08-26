using _ARK_;
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
        public CanvasGroup canvasGroup2D, canvasGroup3D;
        public GraphicRaycaster raycaster_3D, raycaster_2D;
        public RectTransform rT_2D, rT_3D;

        [SerializeField] RectTransform rT_scheduler;
        [SerializeField] TextMeshProUGUI txt_scheduler;

        public RectTransform vchat_icon_rT, vchat_bar_rT;

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

            raycaster_2D = transform.Find("Canvas2D").GetComponent<GraphicRaycaster>();
            raycaster_3D = transform.Find("CameraUI/Canvas3D").GetComponent<GraphicRaycaster>();

            canvas2D = transform.Find("Canvas2D").GetComponent<Canvas>();
            canvasGroup2D = canvas2D.GetComponent<CanvasGroup>();
            rT_2D = (RectTransform)canvas2D.transform.Find("rT");

            rT_scheduler = (RectTransform)canvas2D.transform.Find("scheduler");
            txt_scheduler = rT_scheduler.Find("text").GetComponent<TextMeshProUGUI>();

            cameraUI = transform.Find("CameraUI").GetComponent<Camera>();
            canvas3D = cameraUI.transform.Find("Canvas3D").GetComponent<Canvas>();
            canvasGroup3D = canvas3D.GetComponent<CanvasGroup>();
            rT_3D = (RectTransform)canvas3D.transform.Find("rT");

            vchat_icon_rT = (RectTransform)transform.Find("Canvas2D/rT/VChat");
            vchat_bar_rT = (RectTransform)transform.Find("Canvas2D/rT/VChat/icon/bar");

            AwakeButtons();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            StartButtons();

            OSView.instance.users.AddListener1(this, not_empty =>
            {
                canvas2D.transform.Find("button-play").gameObject.SetActive(not_empty);
            });

            NUCLEOR.instance.sequencer.list.AddListener1(this, isNotEmpty =>
            {
                NUCLEOR.delegates.LateUpdate -= OnLateUpdateSchedulerInfos;
                if (isNotEmpty)
                    NUCLEOR.delegates.LateUpdate += OnLateUpdateSchedulerInfos;
                rT_scheduler.gameObject.SetActive(isNotEmpty);
            });

            IMGUI_global.instance.users_inputs.AddElement(OnImguiInputs, this);

            IMGUI_global.instance.users_ongui.AddListener1(this, isNotEmpty =>
            {
                canvasGroup2D.interactable = canvasGroup3D.interactable = !isNotEmpty;
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnLateUpdateSchedulerInfos()
        {
            Schedulable schedulable = NUCLEOR.instance.sequencer.list._collection[0];

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
            IMGUI_global.instance?.users_inputs.RemoveKey(OnImguiInputs);
            if (this == instance)
                instance = null;
        }
    }
}