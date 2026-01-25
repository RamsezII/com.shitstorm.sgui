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
        public CanvasGroup canvasGroup2D, canvasGroup3D;
        public GraphicRaycaster raycaster_3D, raycaster_2D;

        public RectTransform
            rt_screen,
            rT_2D, rt_windows1, rt_windows2,
            rT_3D;

        [SerializeField] TextMeshProUGUI text_framerate;

        public RectTransform vchat_icon_rT, vchat_bar_rT;

        HeartBeat.Operation op_framerate;

#if UNITY_EDITOR
        [SerializeField] internal SguiWindow _FOCUSED_WINDOW;
#endif

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
            canvasGroup2D = canvas2D.GetComponent<CanvasGroup>();
            rT_2D = (RectTransform)canvas2D.transform.Find("rT");
            rt_windows1 = (RectTransform)rT_2D.Find("windows1");
            rt_windows2 = (RectTransform)rT_2D.Find("windows2");

            rt_screen = (RectTransform)canvas2D.transform.Find("screen_transform");

            text_framerate = transform.Find("Canvas2D/rT/Framerate/text").GetComponent<TextMeshProUGUI>();

            cameraUI = transform.Find("WorldCameraUI").GetComponent<Camera>();
            canvas3D = cameraUI.transform.Find("Canvas3D").GetComponent<Canvas>();
            canvasGroup3D = canvas3D.GetComponent<CanvasGroup>();
            rT_3D = (RectTransform)canvas3D.transform.Find("rT");

            raycaster_2D = canvas2D.GetComponent<GraphicRaycaster>();
            raycaster_3D = canvas3D.GetComponent<GraphicRaycaster>();

            vchat_icon_rT = (RectTransform)transform.Find("Canvas2D/rT/VChat");
            vchat_bar_rT = (RectTransform)transform.Find("Canvas2D/rT/VChat/icon/bar");

            AwakeButtons();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            StartButtons();

            IMGUI_global.instance.inputs_users.AddElement(OnImguiInputs);

            UsageManager.usages[(int)UsageGroups.IMGUI].AddListener1(isNotEmpty =>
            {
                if (this == null)
                {
                    Debug.LogWarning($"{this} is null", this);
                    return;
                }

                canvasGroup2D.interactable = canvasGroup3D.interactable = !isNotEmpty;
                canvasGroup2D.blocksRaycasts = canvasGroup3D.blocksRaycasts = !isNotEmpty;
            });

            NUCLEOR.instance.heartbeat_unscaled.AddOperation(op_framerate = new HeartBeat.Operation("refresh framerate monitor", 1, true, () =>
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

        public bool ScreenPointToWorldPoint(in Vector2 screenPoint, out Vector3 worldPoint) => RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rect: rt_screen,
            screenPoint: screenPoint,
            cam: null,
            out worldPoint
        );

        public bool ScreenPointToLocalPoint(in Vector2 screenPoint, out Vector2 localPoint) => RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect: rt_screen,
            screenPoint: screenPoint,
            cam: null,
            localPoint: out localPoint
        );

        public static Vector3 InverseTransformPoint(in Camera camera, in Vector3 point)
        {
            Vector3 lpos = camera.WorldToViewportPoint(point);
            Rect r = instance.rt_screen.rect;
            return new Vector3(
                Mathf.LerpUnclamped(r.xMin, r.xMax, lpos.x),
                Mathf.LerpUnclamped(r.yMin, r.yMax, lpos.y),
                lpos.z
            );
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);
            op_framerate.Dispose();
        }
    }
}