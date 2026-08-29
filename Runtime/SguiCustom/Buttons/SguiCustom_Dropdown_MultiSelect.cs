using _ARK_;
using _SGUI_.context_click;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _SGUI_
{
    public class SguiCustom_Dropdown_MultiSelect : SguiCustom_Dropdown_StayOpen
    {
        [Serializable]
        public sealed class Option2
        {
            public readonly Traductions label;
            public bool _selected;

            //--------------------------------------------------------------------------------------------------------------

            public Option2(in Traductions label_trad, in bool selected = default)
            {
                label = label_trad;
                _selected = selected;
            }

            public Option2(in string label_string, in bool selected = default) : this(new Traductions(label_string), selected)
            {
            }

            internal bool SetFromUser(in bool selected)
            {
                if (_selected == selected)
                    return false;

                _selected = selected;
                return true;
            }
        }

        public Action<int, bool> onOptionChanged;

        public List<Option2> options2 = new();

        protected override SguiListTypes ListType => SguiListTypes.MultiSelect;
        protected override bool HasOptions => options2.Count > 0;

        //--------------------------------------------------------------------------------------------------------------

        public void SetupOptions2(in List<string> options) => SetupOptions2(options.Select(option => new Option2(option, false)).ToList());
        public void SetupOptions2(in List<Option2> options2)
        {
            Hide();
            this.options2 = options2 ?? new();
            _button.interactable = this.options2.Count > 0;
            RefreshLabel();
        }

        public override void ClearOptions()
        {
            Hide();
            options2 = new();
            _button.interactable = false;
            RefreshLabel();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnContextList(ContextList sguilist)
        {
            var buttons = new List<ContextListButton>();

            var bt_all = sguilist.AddButton_trad(new() { french = "Tout", english = "All", });
            sguilist.AddLine();
            var bt_none = sguilist.AddButton_trad(new() { french = "Rien", english = "None", });
            sguilist.AddLine();

            void RefreshAllNone()
            {
                int selected_count = options2.Count(option2 => option2._selected);
                bool has_options = options2.Count > 0;

                bt_all.toggle.Value = has_options && selected_count == options2.Count;
                bt_none.toggle.Value = has_options && selected_count == 0;
            }

            bt_all._button.onClick.AddListener(() =>
            {
                foreach (var button in buttons)
                    button.toggle.Value = true;

                RefreshAllNone();
            });

            bt_none._button.onClick.AddListener(() =>
            {
                foreach (var button in buttons)
                    button.toggle.Value = false;

                RefreshAllNone();
            });

            for (int i = 0; i < options2.Count; ++i)
            {
                int index = i;
                var option2 = options2[index];
                var button = sguilist.AddButton_trad(option2.label);

                buttons.Add(button);

                button.toggle.Value = option2._selected;

                button.toggle.AddListener(
                    action: value =>
                    {
                        OnOptionChangedFromUser(index, option2, value);
                        RefreshAllNone();
                    },
                    do_not_call_this_time: true
                );
            }

            RefreshAllNone();
        }

        protected override void RefreshLabel()
        {
            if (trad_button == null)
                return;

            int count = options2.Count(option2 => option2._selected);

            if (count == 0)
                trad_button.SetTraductions(new() { french = "Rien", english = "None", });
            else if (count == options2.Count)
                trad_button.SetTraductions(new() { french = "Tout", english = "All", });
            else
                trad_button.SetTraductions(new()
                {
                    french = options2.Where(option => option._selected).Select(option => option.label.french).Join(", "),
                    english = options2.Where(option => option._selected).Select(option => option.label.english).Join(", "),
                });
        }

        void OnOptionChangedFromUser(int index, Option2 option2, bool value)
        {
            if (!option2.SetFromUser(value))
                return;

            RefreshLabel();
            onOptionChanged?.Invoke(index, value);
        }
    }
}
