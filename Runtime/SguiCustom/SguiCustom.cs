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

        public Func<bool> onAction_confirm, onAction_cancel;

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

            button_confirm.onClick.AddListener(OnClick_Confirm);
            button_cancel.onClick.AddListener(OnClick_Cancel);

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

        private void OnClick_Confirm()
        {
            if (!oblivionized)
                if (onAction_confirm == null || onAction_confirm())
                    Oblivionize();
        }

        private void OnClick_Cancel()
        {
            if (!oblivionized)
                if (onAction_cancel == null || onAction_cancel())
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