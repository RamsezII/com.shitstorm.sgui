using _ARK_;
using _SGUI_.context_click;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Dropdown_Normal : SguiCustom_Abstract
    {
        [Serializable]
        public readonly struct Option1
        {
            public readonly Traductions label;

            //--------------------------------------------------------------------------------------------------------------

            public Option1(in Traductions label)
            {
                this.label = label;
            }

            public Option1(in string label) : this(new Traductions(label))
            {
            }
        }

        public Button _button;
        [SerializeField] protected Traductable trad_button;

        public Action<int> onValueChanged;

        ContextList currentList;
        public List<Option1> options1 = new();
        public int selectedIndex = -1;
        public Option1 GetSelectedOption() => options1[selectedIndex];
        protected virtual SguiListTypes ListType => SguiListTypes.Normal;
        protected virtual bool HasOptions => options1.Count > 0;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            _button = GetComponentInChildren<Button>(true);
            trad_button = _button.GetComponentInChildren<Traductable>(true);

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            _button.onClick.AddListener(Show);
        }

        protected override void OnDestroy()
        {
            Hide();
            onValueChanged = null;
            base.OnDestroy();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetupOptions(in int selectedIndex, in List<string> options) => SetupOptions(selectedIndex, options?.Select(option => new Option1(option)).ToList());
        public void SetupOptions(in int selectedIndex, in List<Option1> options1)
        {
            Hide();
            this.options1 = options1 ?? new();
            _button.interactable = this.options1.Count > 0;
            SetValueWithoutNotify(selectedIndex);
        }

        public virtual void ClearOptions()
        {
            Hide();
            options1 = new();
            selectedIndex = -1;
            _button.interactable = false;
            RefreshLabel();
        }

        public void SetValueWithoutNotify(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex >= options1.Count)
                selectedIndex = -1;

            this.selectedIndex = selectedIndex;

            if (currentList != null)
                currentList.SelectButton(selectedIndex);

            RefreshLabel();
        }

        public bool TryGetSelectedOption(out Option1 option)
        {
            if (selectedIndex < 0 || selectedIndex >= options1.Count)
            {
                option = default;
                return false;
            }

            option = options1[selectedIndex];
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Show()
        {
            if (!HasOptions)
                return;

            currentList = SguiContextList.instance.InstantiateListHere(rT.position + new Vector3(0, -.5f * rT.rect.height));
            currentList.rt.pivot = new Vector2(0, 1);
            currentList.type = ListType;

            OnContextList(currentList);
        }

        public void Hide()
        {
            if (currentList != null)
                Destroy(currentList.gameObject);

            currentList = null;
        }

        protected virtual void OnContextList(ContextList sguilist)
        {
            for (int i = 0; i < options1.Count; ++i)
            {
                int index = i;
                ContextListButton button = sguilist.AddButton_trad(options1[index].label);
                button.toggle.Value = index == selectedIndex;
                button._button.onClick.AddListener(() => SelectFromUser(index));
            }
        }

        protected virtual void RefreshLabel()
        {
            if (trad_button == null)
                return;

            if (selectedIndex < 0 || selectedIndex >= options1.Count)
                trad_button.SetText("None");
            else
                trad_button.SetTraductions(options1[selectedIndex].label);
        }

        void SelectFromUser(int index)
        {
            if (index < 0 || index >= options1.Count || index == selectedIndex)
                return;

            SetValueWithoutNotify(index);
            onValueChanged?.Invoke(index);
        }
    }
}
