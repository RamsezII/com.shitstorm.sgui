using UnityEngine;

namespace _SGUI_
{
    public partial class SguiCustom : SguiWindow2
    {
        readonly SguiCustomButton[] prefabs = new SguiCustomButton[(int)SguiCustomButton.Codes._last_];
        SguiCustomButton this[in SguiCustomButton.Codes code] { get => prefabs[(int)code]; set => prefabs[(int)code] = value; }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            RectTransform rT = (RectTransform)transform.Find("rT/body/scroll_view/viewport/content_layout");

            this[SguiCustomButton.Codes.Slider] = rT.Find("sgui2-slider").GetComponent<SguiCustomButton>();
            this[SguiCustomButton.Codes.InputField] = rT.Find("sgui2-input").GetComponent<SguiCustomButton>();
            this[SguiCustomButton.Codes.Dropdown] = rT.Find("sgui2-dropdown").GetComponent<SguiCustomButton>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            for (SguiCustomButton.Codes code = 0; code < SguiCustomButton.Codes._last_; ++code)
                this[code].gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiCustomButton AddButton(in SguiCustomButton.Infos infos)
        {
            SguiCustomButton prefab = this[infos.code];
            SguiCustomButton clone = Instantiate(prefab, prefab.transform.parent);
            clone.gameObject.SetActive(true);
            clone.Init(infos);
            return clone;
        }
    }
}