using _ARK_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public abstract class SguiCustom_Abstract : ArkComponent1
    {
        public SguiCustom window;
        [HideInInspector] public RectTransform rT, rT_parent, rT_label;
        [HideInInspector] public TextMeshProUGUI tmp_label;
        [HideInInspector] public Traductable trad_label;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            window = GetComponentInParent<SguiCustom>();
            rT = (RectTransform)transform;
            rT_parent = (RectTransform)transform.parent;
            rT_label = (RectTransform)transform.Find("label");
            tmp_label = rT_label.GetComponent<TextMeshProUGUI>();
            trad_label = rT_label.GetComponent<Traductable>();

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();
            window.AutoSizeAtEndOfFrame();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            window.AutoSizeAtEndOfFrame();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleBottomLine(in bool value) => transform.Find("line_bottom").gameObject.SetActive(value);
    }
}