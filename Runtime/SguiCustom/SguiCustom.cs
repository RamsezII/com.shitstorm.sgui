using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiCustom : SguiWindow2
    {
        readonly Dictionary<Type, SguiCustomButton_Abstract> prefabs = new();

        public readonly List<SguiCustomButton_Abstract> clones = new();

        public Action onAction_confirm, onAction_cancel;

        [SerializeField] Button button_confirm, button_cancel;

        VerticalLayoutGroup content_layout;
        RectTransform content_layout_rT;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            RectTransform rT;

            rT = (RectTransform)transform.Find("rT/footer");

            button_cancel = rT.Find("button_cancel").GetComponent<Button>();
            button_confirm = rT.Find("button_confirm").GetComponent<Button>();

            button_confirm.onClick.AddListener(OnClick_Confirm);
            button_cancel.onClick.AddListener(OnClick_cancel);

            rT = (RectTransform)transform.Find("rT/body/scroll_view/viewport/content_layout");

            content_layout_rT = rT;
            content_layout = rT.GetComponent<VerticalLayoutGroup>();

            prefabs[typeof(SguiCustomButton_Slider)] = rT.Find("sgui2-slider").GetComponent<SguiCustomButton_Slider>();
            prefabs[typeof(SguiCustomButton_InputField)] = rT.Find("sgui2-input").GetComponent<SguiCustomButton_InputField>();
            prefabs[typeof(SguiCustomButton_Dropdown)] = rT.Find("sgui2-dropdown").GetComponent<SguiCustomButton_Dropdown>();
            prefabs[typeof(SguiCustomButton_Toggle)] = rT.Find("sgui2-toggle").GetComponent<SguiCustomButton_Toggle>();
            prefabs[typeof(SguiCustomButton_Button)] = rT.Find("sgui2-button").GetComponent<SguiCustomButton_Button>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            foreach (var pair in prefabs)
                pair.Value.gameObject.SetActive(false);

            if (clones.Count > 0)
                clones[^1].ToggleBottomLine(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public T AddButton<T>() where T : SguiCustomButton_Abstract => (T)AddButton(typeof(T));
        public SguiCustomButton_Abstract AddButton(in Type type)
        {
            SguiCustomButton_Abstract prefab = prefabs[type];
            SguiCustomButton_Abstract clone = Instantiate(prefab, prefab.transform.parent);
            clones.Add(clone);
            clone.gameObject.SetActive(true);
            return clone;
        }

        private void OnClick_Confirm()
        {
            if (!oblivionized)
                onAction_confirm?.Invoke();
            Oblivionize();
        }

        private void OnClick_cancel()
        {
            if (!oblivionized)
                onAction_cancel?.Invoke();
            Oblivionize();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnOblivion()
        {
            base.OnOblivion();
            for (int i = 0; i < clones.Count; ++i)
                clones[i].Dispose();
            clones.Clear();
        }
    }
}