using _ARK_;
using _UTIL_;
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

        public Func<bool> onFunc_confirm, onFunc_cancel;
        public Action onAction_confirm, onAction_cancel;
        public Traductable trad_cancel, trad_confirm;

        public Button button_confirm, button_cancel;

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

            trad_cancel = button_cancel.transform.Find("label").GetComponent<Traductable>();
            trad_confirm = button_confirm.transform.Find("label").GetComponent<Traductable>();

            button_confirm.onClick.AddListener(() =>
            {
                if (!oblivionized)
                    if (onFunc_confirm != null && !onFunc_confirm())
                        return;
                onAction_confirm?.Invoke();
                Oblivionize();
            });

            button_cancel.onClick.AddListener(() =>
            {
                if (!oblivionized)
                    if (onFunc_cancel != null && !onFunc_cancel())
                        return;
                onAction_cancel?.Invoke();
                Oblivionize();
            });

            rT = (RectTransform)transform.Find("rT/body/scroll_view/viewport/content_layout");

            content_layout_rT = rT;
            content_layout = rT.GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < rT.childCount; ++i)
                if (rT.GetChild(i).TryGetComponent<SguiCustom_Abstract>(out var prefab))
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
            NUCLEOR.instance.subScheduler.AddRoutine(Util.EWaitForFrames(1, AutoSize));
            NUCLEOR.instance.subScheduler.AddRoutine(Util.EWaitForFrames(2, AutoSize));
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

        public void EditArkJSon(in string file_path, Type type)
        {
            StaticJSon arkjson = (StaticJSon)JsonUtility.FromJson(File.ReadAllText(file_path), type);
            ReflectionEditor(arkjson, result =>
            {
                StaticJSon arkjson = (StaticJSon)JsonUtility.FromJson(JsonUtility.ToJson(result), type);
                arkjson.SaveStaticJSon(true);
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