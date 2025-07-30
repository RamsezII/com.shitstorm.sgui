using _ARK_;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiCustom : SguiWindow2
    {
        readonly Dictionary<Type, SguiCustom_Abstract> prefabs = new();

        public readonly List<SguiCustom_Abstract> clones = new();

        VerticalLayoutGroup content_layout;
        RectTransform content_layout_rT;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            content_layout_rT = (RectTransform)transform.Find("rT/body/scroll_view/viewport/content_layout");
            content_layout = content_layout_rT.GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < content_layout_rT.childCount; ++i)
                if (content_layout_rT.GetChild(i).TryGetComponent<SguiCustom_Abstract>(out var prefab))
                    prefabs[prefab.GetType()] = prefab;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            foreach (var pair in prefabs)
                pair.Value.gameObject.SetActive(false);

            if (clones.Count > 0)
                clones[^1].ToggleBottomLine(false);

            AutoSize();
        }

        //--------------------------------------------------------------------------------------------------------------

        public T AddButton<T>() where T : SguiCustom_Abstract => (T)AddButton(typeof(T));
        public SguiCustom_Abstract AddButton(in Type type)
        {
            SguiCustom_Abstract prefab = prefabs[type];
            SguiCustom_Abstract clone = Instantiate(prefab, prefab.transform.parent);
            clones.Add(clone);
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
                NUCLEOR.delegates.onApplicationFocus?.Invoke();
            });
        }

        public static SguiCustom ShowAlert(in SguiCustom_Alert.DialogTypes type, out SguiCustom_Alert alert, in Traductions traductions)
        {
            SguiCustom sgui = InstantiateWindow<SguiCustom>();
            alert = sgui.AddButton<SguiCustom_Alert>();

            alert.SetType(type);
            alert.SetText(traductions);
            sgui.trad_title.SetTrad(type.ToString());

            if (type != SguiCustom_Alert.DialogTypes.Dialog)
            {
                sgui.button_cancel.gameObject.SetActive(false);
                sgui.trad_confirm.SetTrad("OK");
            }

            return sgui;
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