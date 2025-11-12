using _ARK_;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiCustom : SguiWindow2
    {
        readonly Dictionary<Type, SguiCustom_Abstract> prefabs = new();

        VerticalLayoutGroup content_layout;
        RectTransform content_layout_rT;

        public SguiCustom_Abstract[] GetButtons(in bool includeInactive = false) => GetComponentsInChildren<SguiCustom_Abstract>(includeInactive);

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            content_layout_rT = (RectTransform)transform.Find("rT/body/scroll_view/viewport/content_layout");
            content_layout = content_layout_rT.GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < content_layout_rT.childCount; ++i)
                if (content_layout_rT.GetChild(i).TryGetComponent<SguiCustom_Abstract>(out var prefab))
                    prefabs[prefab.GetType()] = prefab;

            EventSystem.current.SetSelectedGameObject(button_confirm.gameObject);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            foreach (var pair in prefabs)
                pair.Value.gameObject.SetActive(false);

            AutoSize();
        }

        //--------------------------------------------------------------------------------------------------------------

        public T AddButton<T>() where T : SguiCustom_Abstract => (T)AddButton(typeof(T));
        public SguiCustom_Abstract AddButton(in Type type)
        {
            SguiCustom_Abstract prefab = prefabs[type];
            SguiCustom_Abstract clone = Instantiate(prefab, prefab.transform.parent);
            clone.gameObject.SetActive(true);
            return clone;
        }

        public void EditJSon(string file_path, Type type)
        {
            object obj = JsonUtility.FromJson(File.ReadAllText(file_path), type);
            JSon json = (JSon)obj;

            ReflectionEditor(json, result =>
            {
                obj = JsonUtility.FromJson(JsonUtility.ToJson(result), type);
                json = (JSon)obj;
                json.Save(file_path, true);
                NUCLEOR.delegates.OnApplicationFocus?.Invoke();
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnOblivion()
        {
            base.OnOblivion();

            var buttons = GetButtons(true);
            for (int i = 0; i < buttons.Length; ++i)
                buttons[i].Dispose();
        }
    }
}