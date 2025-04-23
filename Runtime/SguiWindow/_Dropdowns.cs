using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow
    {
        enum HelpDropdowns : byte
        {
            Documentation,
            Youtube,
            Discord,
            Shitstorm,
            _last_,
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnPopulateDropdowns()
        {
            if (dropdown_help != null)
            {
                List<TMP_Dropdown.OptionData> options = new();
                OnPopulateDropdown_Help(options);
                dropdown_help.dropdown.AddOptions(options);
            }
        }

        protected virtual void OnPopulateDropdown_Help(List<TMP_Dropdown.OptionData> options)
        {
            for (HelpDropdowns code = 0; code < HelpDropdowns._last_; ++code)
                options.Add(new(code switch
                {
                    HelpDropdowns.Shitstorm => "shitstorm.ovh",
                    _ => code.ToString(),
                }));
        }

        public virtual void OnClick_HelpDropdown(string item)
        {
            if (Enum.TryParse(item, true, out HelpDropdowns code))
                switch (code)
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

                    default:
                        Debug.LogWarning($"{GetType().FullName} : unused {nameof(code)}: '{code}'");
                        break;
                }
            else
                Debug.LogWarning($"{GetType().FullName} : unknown {nameof(code)}: '{item}'");
        }
    }
}