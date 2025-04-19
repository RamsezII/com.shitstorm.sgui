using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiDialog : SguiWindow
    {
        public Traductable trad_text;
        public Button button_no, button_yes;

        //--------------------------------------------------------------------------------------------------------------

        public static SguiDialog ShowDialog(out IEnumerator<bool> routine)
        {
            SguiDialog clone = Util.Instantiate<SguiDialog>(SGUI_global.instance.rT);
            routine = clone.ERoutine();
            return clone;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            trad_text = transform.Find("rT/body/text/tmp").GetComponent<Traductable>();

            button_no = transform.Find("rT/body/buttons/button_a/Button").GetComponent<Button>();
            button_yes = transform.Find("rT/body/buttons/button_b/Button").GetComponent<Button>();

            button_hide.gameObject.SetActive(false);
            button_fullscreen.gameObject.SetActive(false);
            button_close.gameObject.SetActive(false);

            Vector2 size = rT.rect.size;
            Vector2 psize = rT_parent.rect.size;
            rT.anchoredPosition = .5f * (psize - size);
        }

        //--------------------------------------------------------------------------------------------------------------


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