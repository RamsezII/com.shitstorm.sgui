using _ARK_;
using _UTIL_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public sealed class SguiProgressBar : SguiWindow2
    {
        public RectTransform rT_fill;
        [SerializeField] TextMeshProUGUI tmp_percentage;
        public Traductable trad_infos;
        TextMeshProUGUI tmp_timer;

        float initsize_tmp;

        HeartBeat.Operation op_timer;
        float start_time;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            rT_fill = (RectTransform)transform.Find("rT/progress/mask/fill");
            tmp_percentage = transform.Find("rT/progress/text").GetComponent<TextMeshProUGUI>();
            tmp_timer = transform.Find("rT/header/timer").GetComponent<TextMeshProUGUI>();
            trad_infos = transform.Find("rT/text").GetComponent<Traductable>();

            base.OnAwake();

            initsize_tmp = trad_infos.tmpro.preferredHeight;

            start_time = Time.unscaledTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            AutoSize();

            tmp_timer.text = "0s";
            NUCLEOR.instance.heartbeat_unscaled.operations.Add(op_timer = new(.45f, true, () =>
            {
                string time = (Time.unscaledTime - start_time).TimeLog();
                tmp_timer.text = time;
            }));
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetProgress(in float progress)
        {
            rT_fill.anchorMax = new(progress, 1);
            tmp_percentage.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }

        public void AutoSize()
        {
            if (!tmp_percentage.gameObject.activeInHierarchy || string.IsNullOrWhiteSpace(trad_infos.tmpro.text))
            {
                rt.sizeDelta = rect_current.size;
                return;
            }

            float preferred_height = trad_infos.tmpro.preferredHeight;
            rt.sizeDelta = new(rect_current.size.x, rect_current.size.y + preferred_height - initsize_tmp);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnOblivion()
        {
            base.OnOblivion();
            op_timer.Dispose();
        }
    }
}