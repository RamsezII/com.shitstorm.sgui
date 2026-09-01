using _ARK_;
using _UTIL_;
using System.Text;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    partial class ArkMonitor : MonoBehaviour
    {
        public static ArkMonitor instance;

        [SerializeField] RectTransform rt;
        [SerializeField] TMP_Text text;

        Scheduler.Operation op_refresh;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            rt = (RectTransform)transform.Find("rt");
            text = rt.Find("text").GetComponent<TMP_Text>();
        }

        //----------------------------------------------------------------------------------------------------------

        private void Start()
        {
            op_refresh = NUCLEOR.instance.scheduler_unscaled.AddOperation(new(GetType().FullName, .1f, true, () =>
            {
                var sb = new StringBuilder();

                NUCLEOR.instance.monolith.GetStatus(sb);
                NUCLEOR.instance.routinizer.GetStatus(sb);

                if (sb.Length > 0)
                {
                    gameObject.SetActive(true);
                    text.text = sb.ToString().TrimEnd('\n');
                    rt.sizeDelta = text.GetPreferredValues(text.text);
                }
                else
                {
                    text.text = string.Empty;
                    gameObject.SetActive(false);
                }
            }));
        }

        //----------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (this == instance)
                instance = null;
            op_refresh.Dispose();
        }
    }
}