using _ARK_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    internal class NucleorMonitor : MonoBehaviour
    {
        [SerializeField] RectTransform rt, rt_progress;
        [SerializeField] TextMeshProUGUI text, percent;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            rt = (RectTransform)transform.Find("rt");
            rt_progress = (RectTransform)rt.Find("progress");
            text = rt.Find("text").GetComponent<TextMeshProUGUI>();
            percent = rt.Find("percent").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            NUCLEOR.instance.monolith.sequencables.AddListener1(isNotEmpty =>
            {
                NUCLEOR.delegates.LateUpdate -= Refresh;
                if (isNotEmpty)
                    NUCLEOR.delegates.LateUpdate += Refresh;
                rt.gameObject.SetActive(isNotEmpty);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void Refresh()
        {
            Sequencable schedulable = NUCLEOR.instance.monolith.sequencables._collection[0];

            float progress = schedulable.Progress;

            percent.text = $"{Mathf.RoundToInt(100 * progress)}%";
            rt_progress.anchorMax = new Vector2(progress, 1);

            text.text = schedulable.description;
            rt.sizeDelta = new Vector2(0, text.GetPreferredValues(text.text, ArkUI.instance.rt_canvas.rect.width, float.PositiveInfinity).y);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            NUCLEOR.delegates.LateUpdate -= Refresh;
        }
    }
}
