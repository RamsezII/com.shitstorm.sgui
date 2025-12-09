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
            NUCLEOR.instance.sequencer.schedulables.AddListener1(isNotEmpty =>
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
            Schedulable schedulable = NUCLEOR.instance.sequencer.schedulables._collection[0];

            float progress = schedulable.routine == null ? 0 : schedulable.routine.Current;

            percent.text = $"{Mathf.RoundToInt(100 * progress)}%";
            rt_progress.anchorMax = new Vector2(progress, 1);

            text.text = schedulable.description;
            rt.sizeDelta = new Vector2(0, text.GetPreferredValues(text.text, SguiGlobal.instance.rT_2D.rect.width, float.PositiveInfinity).y);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            NUCLEOR.delegates.LateUpdate -= Refresh;
        }
    }
}
