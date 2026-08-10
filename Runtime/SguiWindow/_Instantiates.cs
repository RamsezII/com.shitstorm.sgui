using _ARK_;
using System;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow
    {
        public static SguiCustom CreatePrompt() => ShowPrompt<SguiCustom>();

        public static T ShowPrompt<T>() where T : SguiPrompt => (T)ShowPrompt(typeof(T));
        public static SguiPrompt ShowPrompt(in Type type) => ShowPrompt((SguiPrompt)Util.LoadResourceByType(type));
        public static SguiPrompt ShowPrompt(in SguiPrompt prefab)
        {
            SguiPrompt clone = Instantiate(prefab, SguiGlobal.instance.rt_sgui_prompts);
            clone.OnAwake();
            return clone;
        }

        public static SguiCustom ShowProgressBar(out SguiCustom_Progress progress_bar, in bool no_label = false)
        {
            SguiCustom window = CreatePrompt();

            progress_bar = window.AddButton<SguiCustom_Progress>();

            window.button_confirm.transform.gameObject.SetActive(false);
            window.button_close.transform.parent.parent.gameObject.SetActive(false);
            window.trad_title.tmpro.alignment = TMPro.TextAlignmentOptions.MidlineLeft;

            if (no_label)
            {
                progress_bar.rT_label.gameObject.SetActive(false);
                RectTransform rt = (RectTransform)progress_bar.rT_fill.parent.parent;
                rt.anchorMin = new(0, .5f);
            }

            return window;
        }

        public static SguiCustom ShowAlert(in SguiDialogs type, out SguiCustom_Alert alert, in Traductions traductions)
        {
            SguiCustom sgui = CreatePrompt();

            switch (type)
            {
                case SguiDialogs.Info:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;

                case SguiDialogs.Dialog:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;

                case SguiDialogs.Error:
                    Debug.LogWarning($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;

                default:
                    Debug.LogError($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;
            }

            alert = sgui.AddButton<SguiCustom_Alert>();
            alert.SetType(type);
            alert.SetText(traductions);
            sgui.trad_title.SetTrad(type.ToString());

            if (type == SguiDialogs.Dialog)
            {
                sgui.trad_confirm.SetTrads(new() { french = "Oui", english = "Yes", });
                sgui.trad_cancel.SetTrads(new() { french = "Non", english = "No", });
            }
            else
            {
                sgui.button_cancel.gameObject.SetActive(false);
                sgui.trad_confirm.SetTrad("OK");
            }

            return sgui;
        }
    }
}