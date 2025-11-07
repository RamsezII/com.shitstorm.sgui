using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public RectTransform
            rT_2D, rt_windows1, rt_windows2,
            rT_3D;

        [SerializeField] RectTransform rT_scheduler;
        [SerializeField] TextMeshProUGUI txt_scheduler, text_framerate;

        public RectTransform vchat_icon_rT, vchat_bar_rT;

        HeartBeat.Operation op_framerate;

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
            rt_windows1 = (RectTransform)rT_2D.Find("windows1");
            rt_windows2 = (RectTransform)rT_2D.Find("windows2");

            rT_scheduler = (RectTransform)canvas2D.transform.Find("scheduler");
            txt_scheduler = rT_scheduler.Find("text").GetComponent<TextMeshProUGUI>();

            text_framerate = transform.Find("Canvas2D/rT/Framerate/text").GetComponent<TextMeshProUGUI>();

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

            NUCLEOR.instance.sequencer.schedulables.AddListener1(this, isNotEmpty =>
            {
                NUCLEOR.delegates.LateUpdate -= OnLateUpdateSchedulerInfos;
                if (isNotEmpty)
                    NUCLEOR.delegates.LateUpdate += OnLateUpdateSchedulerInfos;
                rT_scheduler.gameObject.SetActive(isNotEmpty);
            });

            IMGUI_global.instance.users_inputs.AddElement(OnImguiInputs, this);

            UsageManager.usages[(int)UsageGroups.IMGUI].AddListener1(this, isNotEmpty =>
            {
                canvasGroup2D.interactable = canvasGroup3D.interactable = !isNotEmpty;
                canvasGroup2D.blocksRaycasts = canvasGroup3D.blocksRaycasts = !isNotEmpty;
            });

            NUCLEOR.delegates.LateUpdate += CheckClick;

            NUCLEOR.instance.heartbeat_unscaled.operations.Add(op_framerate = new HeartBeat.Operation(1, true, () =>
            {
                float framerate = 1 / NUCLEOR.instance.averageUnscaledDeltatime;
                text_framerate.text = $"{Mathf.RoundToInt(framerate)}";

                if (framerate >= 28)
                    text_framerate.color = Color.white;
                else if (framerate >= 18)
                    text_framerate.color = Color.orange;
                else
                    text_framerate.color = Color.red;
            }));
        }

        //--------------------------------------------------------------------------------------------------------------

        void CheckClick()
        {
            if (!UsageManager.AllAreEmpty(UsageGroups.GameMouse))
                if (Input.GetMouseButtonDown(0))
                {
                    PointerEventData e = new(EventSystem.current)
                    {
                        position = Input.mousePosition,
                    };

                    List<RaycastResult> results = new();

                    if (Raycast(raycaster_2D))
                        return;

                    results.Clear();
                    if (Raycast(raycaster_3D))
                        return;

                    foreach (var raycaster in FindObjectsByType<GraphicRaycaster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
                        if (raycaster != raycaster_2D && raycaster != raycaster_3D)
                        {
                            results.Clear();
                            if (Raycast(raycaster))
                                return;
                        }

                    bool Raycast(in GraphicRaycaster raycaster)
                    {
                        raycaster.Raycast(e, results);

                        for (int i = 0; i < results.Count; i++)
                        {
                            IClickable clickable = results[i].gameObject.GetComponentInParent<IClickable>();
                            if (clickable != null)
                            {
                                clickable.OnPointerClick(e);
                                return true;
                            }
                        }

                        return false;
                    }
                }
        }

        void OnLateUpdateSchedulerInfos()
        {
            Schedulable schedulable = NUCLEOR.instance.sequencer.schedulables._collection[0];

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
            NUCLEOR.delegates.LateUpdate -= CheckClick;
            IMGUI_global.instance?.users_inputs.RemoveKey(OnImguiInputs);
            op_framerate.Dispose();
        }
    }
}