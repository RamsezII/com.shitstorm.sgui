using _ARK_;
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
        Vector2 initial_size;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnInitialize()
        {
            base.OnInitialize();

            var rt = transform.AsRTfm();
            initial_size = rt.rect.size;
            rt.anchoredPosition = .5f * (transform.parent.AsRTfm().rect.size - initial_size);
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
            trad_label.SetTraductions(trads);
            Util.AddActionOnce(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, FitText);
        }

        public void FitText()
        {
            float height = trad_label.tmpro.preferredHeight;
            transform.AsRTfm().sizeDelta = initial_size + new Vector2(0, height);
        }
    }
}
