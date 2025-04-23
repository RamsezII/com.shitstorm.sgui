using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiDialog : SguiWindow
    {
        protected Traductable trad_text;
        public Button button_no, button_yes;

        Vector2 initial_size;

        //--------------------------------------------------------------------------------------------------------------

        public static void ShowDialog<T>(in Traductions text, in string title = null, in string ok_button = null, in string cancel_button = null) where T : SguiDialog
        {
            T clone = Util.Instantiate<T>(SguiGlobal.instance.rT);

            clone.SetText(text);

            if (!string.IsNullOrWhiteSpace(title))
                clone.trad_title.SetTrad(title);

            NUCLEOR.instance.subScheduler.AddRoutine(clone.ERoutine());
        }

        public static T ShowDialog<T>(out IEnumerator<bool> routine) where T : SguiDialog
        {
            T clone = Util.Instantiate<T>(SguiGlobal.instance.rT);
            routine = clone.ERoutine();
            return clone;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            trad_text = transform.Find("rT/body/text").GetComponent<Traductable>();

            button_no = transform.Find("rT/body/buttons/button_a/Button")?.GetComponent<Button>();
            button_yes = transform.Find("rT/body/buttons/button_b/Button")?.GetComponent<Button>();

            initial_size = rT.rect.size;
            rT.anchoredPosition = .5f * (rT_parent.rect.size - initial_size);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetText(in Traductions trads)
        {
            trad_text.SetTrads(trads);
            Util.AddAction(ref NUCLEOR.delegates.onEndOfFrame_once, FitText);
        }

        public void FitText()
        {
            TextMeshProUGUI tmp = trad_text.AllTmps().First();
            float height = tmp.preferredHeight;
            rT.sizeDelta = initial_size + new Vector2(0, height);
        }

        IEnumerator<bool> ERoutine()
        {
            while (state_base == 0)
                yield return false;

            onState += (state, onEnter) =>
            {
                if (state == BaseStates.Default)
                    Destroy(gameObject);
            };

            bool waiting = true;
            bool value = false;

            button_no.onClick.AddListener(() =>
            {
                waiting = false;
            });

            button_yes.onClick.AddListener(() =>
            {
                waiting = false;
                value = true;
            });

            while (waiting)
                yield return default;

            button_no.interactable = false;
            button_yes.interactable = false;

            sgui_toggle_window.Update(false);

            yield return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            button_no.onClick.RemoveAllListeners();
            button_yes.onClick.RemoveAllListeners();
        }
    }
}