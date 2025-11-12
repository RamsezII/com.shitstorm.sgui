using _ARK_;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public enum SguiDialogs : byte
    {
        Info = 1,
        Dialog = 2,
        Error = 3,
    }

    public class SguiCustom_Alert : SguiCustom_Abstract
    {
        public Traductable trad_text;

        Vector2 initial_size;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            trad_text = transform.Find("label").GetComponent<Traductable>();

            base.Awake();

            initial_size = rT.rect.size;
            rT.anchoredPosition = .5f * (rT_parent.rect.size - initial_size);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetType(in SguiDialogs type)
        {
            transform.Find("icon-info").GetComponent<RawImage>().gameObject.SetActive(type == SguiDialogs.Info);
            transform.Find("icon-question").GetComponent<RawImage>().gameObject.SetActive(type == SguiDialogs.Dialog);
            transform.Find("icon-error").GetComponent<RawImage>().gameObject.SetActive(type == SguiDialogs.Error);
        }

        public void SetText(in Traductions trads)
        {
            trad_text.SetTrads(trads);
            Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, FitText);
        }

        public void FitText()
        {
            TextMeshProUGUI tmp = trad_text.AllTmps().First();
            float height = tmp.preferredHeight;
            rT.sizeDelta = initial_size + new Vector2(0, height);
        }
    }
}
