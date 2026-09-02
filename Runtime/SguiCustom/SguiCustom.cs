using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiCustom : SguiPrompt
    {
        readonly Dictionary<Type, SguiCustom_Abstract> button_prefabs = new();

        VerticalLayoutGroup content_layout;
        RectTransform content_layout_rT;

        public bool HasAnyButton => GetComponentInChildren<SguiCustom_Abstract>() != null;

        //--------------------------------------------------------------------------------------------------------------

        internal protected override void OnInitialize()
        {
            base.OnInitialize();

            content_layout_rT = (RectTransform)rt.Find("body/scroll_view/viewport/content_layout");
            content_layout = content_layout_rT.GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < content_layout_rT.childCount; ++i)
                if (content_layout_rT.GetChild(i).TryGetComponent<SguiCustom_Abstract>(out var prefab))
                    button_prefabs[prefab.GetType()] = prefab;

            EventSystem.current.SetSelectedGameObject(button_confirm.gameObject);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            AutoSizeAtEndOfFrame();
        }

        //--------------------------------------------------------------------------------------------------------------

        public T AddButton<T>() where T : SguiCustom_Abstract => (T)AddButton(typeof(T));
        public SguiCustom_Abstract AddButton(in Type type)
        {
            SguiCustom_Abstract prefab = button_prefabs[type];
            SguiCustom_Abstract clone = Instantiate(prefab, prefab.transform.parent);
            clone.gameObject.SetActive(true);

            return clone;
        }
    }
}