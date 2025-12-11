using _ARK_;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    class SguiMonitor_PageButton : MonoBehaviour
    {
        [SerializeField] internal Button button;
        [SerializeField] internal Image img_selected;
        [SerializeField] TextMeshProUGUI text;
        float max_width;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            button = GetComponent<Button>();
            img_selected = transform.Find("selected").GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            max_width = text.rectTransform.sizeDelta.x;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            Traductable.language.AddListener(OnLangage);
        }

        private void OnDisable()
        {
            Traductable.language.RemoveListener(OnLangage);
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnLangage(Languages value)
        {
            NUCLEOR.delegates.Update_OnStartOfFrame_once += () =>
            {
                float w = text.GetPreferredValues(text.text, max_width, float.PositiveInfinity).x;
                w = Mathf.Min(max_width, w);
                text.rectTransform.sizeDelta = new(w, 0);
            };
        }
    }
}