using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiCustom : SguiWindow2
    {
        readonly Dictionary<Type, SguiCustomButton> prefabs = new();

        readonly List<SguiCustomButton> clones = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            RectTransform rT = (RectTransform)transform.Find("rT/body/scroll_view/viewport/content_layout");

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

        public SguiCustomButton AddButton(in SguiCustomButton.Infos infos)
        {
            SguiCustomButton prefab = prefabs[infos.type];
            SguiCustomButton clone = Instantiate(prefab, prefab.transform.parent);
            clones.Add(clone);
            clone.gameObject.SetActive(true);
            clone.Init(infos);
            return clone;
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