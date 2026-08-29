using _ARK_;
using _SGUI_.context_click;
using _UTIL_;
using System.Linq;

namespace _SGUI_
{
    public class SguiCustom_Dropdown_MultiSelect : SguiCustom_Dropdown_StayOpen
    {
        public readonly struct Option
        {
            public readonly Traductions label;
            public readonly ValueNotifier<bool> toggle;

            //--------------------------------------------------------------------------------------------------------------

            public Option(in Traductions label, in bool toggle = default) : this(label, new ValueNotifier<bool>(toggle))
            {
            }

            public Option(in Traductions label, in ValueNotifier<bool> toggle)
            {
                this.label = label;
                this.toggle = toggle;
            }
        }

        Option[] options;

        //--------------------------------------------------------------------------------------------------------------

        public Option[] SetupOptions(params Option[] options)
        {
            ClearOptions();
            this.options = options;
            RefreshLabel();
            return options;
        }

        public void ClearOptions()
        {
            options = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshLabel()
        {
            int count = options.Count(option => option.toggle._value);
            if (count == 0)
                trad_button.SetTraductions(new() { french = "Rien", english = "None", });
            else if (count == options.Length)
                trad_button.SetTraductions(new() { french = "Tout", english = "All", });
            else
                trad_button.SetTraductions(new()
                {
                    french = options.Where(option => option.toggle._value).Select(option => option.label.french).Join(", "),
                    english = options.Where(option => option.toggle._value).Select(option => option.label.english).Join(", "),
                });
        }

        protected override void OnContextList(ContextList sguilist)
        {
            base.OnContextList(sguilist);

            foreach (var option in options)
            {
                var button = sguilist.AddButton_trad(option.label);
                button.toggle.Value = option.toggle._value;
                button.toggle.AddListener(option.toggle.Update);
                button.toggle.AddListener(RefreshLabel);
            }

            sguilist.buttons_toggled.AddListener2(list => RefreshLabel());
        }
    }
}