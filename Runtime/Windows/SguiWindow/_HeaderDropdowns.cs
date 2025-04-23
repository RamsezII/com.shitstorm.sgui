using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiWindow
    {
        protected enum HelpDropdowns : byte
        {
            Documentation,
            Youtube,
            Discord,
            Shitstorm,
            _last_,
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void OnRebuildDropdowns()
        {
            if (dropdown_help != null)
            {
                List<TMP_Dropdown.OptionData> options = new();
                OnRebuildDropdown_Help(options);
                dropdown_help.dropdown.AddOptions(options);
            }
        }

        protected virtual void OnRebuildDropdown_Help(List<TMP_Dropdown.OptionData> options)
        {
            for (HelpDropdowns code = 0; code < HelpDropdowns._last_; ++code)
                options.Add(new(code switch
                {
                    HelpDropdowns.Shitstorm => "shitstorm.ovh",
                    _ => code.ToString(),
                }));
        }

        public virtual void OnClick_HelpDropdown()
        {
            switch ((HelpDropdowns)dropdown_help.dropdown.value)
            {
                case HelpDropdowns.Documentation:
                    SguiDialog.ShowDialog<SguiAlert>(new("no documentation yet"));
                    break;

                case HelpDropdowns.Youtube:
                    Application.OpenURL("https://www.youtube.com/@_SHITSTORM_");
                    break;

                case HelpDropdowns.Discord:
                    Application.OpenURL("https://discord.gg/MWhZSh2Pn8");
                    break;

                case HelpDropdowns.Shitstorm:
                    Application.OpenURL("https://shitstorm.ovh");
                    break;
            }
        }
    }
}