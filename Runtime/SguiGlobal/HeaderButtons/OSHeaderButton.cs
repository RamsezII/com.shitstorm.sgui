using _UTIL_;
using UnityEngine.UI;

namespace _SGUI_
{
    public class OSHeaderButton : OSHeaderItem
    {
        public Button button;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            button = transform.Find("button").GetComponent<Button>();
            label = button.transform.Find("label").GetComponent<Traductable>();
            base.Awake();
        }
    }
}