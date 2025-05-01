using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiCustom : SguiWindow2
    {
        readonly Dictionary<Type, SguiCustomButton> prefabs = new();

        readonly List<SguiCustomButton> clones = new();

        public Action onButton_confirm, onButton_cancel;

        [SerializeField] Button button_confirm, button_cancel;

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

            prefabs[typeof(SguiCustomButton_Slider)] = rT.Find("sgui2-slider").GetComponent<SguiCustomButton_Slider>();
            prefabs[typeof(SguiCustomButton_InputField)] = rT.Find("sgui2-input").GetComponent<SguiCustomButton_InputField>();
            prefabs[typeof(SguiCustomButton_Dropdown)] = rT.Find("sgui2-dropdown").GetComponent<SguiCustomButton_Dropdown>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            foreach (var pair in prefabs)
                pair.Value.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiCustomButton AddButton(in Type type)
        {
            SguiCustomButton prefab = prefabs[type];
            SguiCustomButton clone = Instantiate(prefab, prefab.transform.parent);
            clones.Add(clone);
            clone.gameObject.SetActive(true);
            return clone;
        }

        private void OnClick_Confirm()
        {
            if (!oblivionized)
                onButton_confirm?.Invoke();
            Oblivionize();
        }

        private void OnClick_cancel()
        {
            if (!oblivionized)
                onButton_cancel?.Invoke();
            Oblivionize();
        }

        public List<object> GetResults()
        {
            List<object> results = new();
            for (int i = 0; i < clones.Count; ++i)
                results.Add(clones[i].GetResult());
            return results;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            for (int i = 0; i < clones.Count; ++i)
                clones[i].Dispose();
            clones.Clear();
        }
    }
}