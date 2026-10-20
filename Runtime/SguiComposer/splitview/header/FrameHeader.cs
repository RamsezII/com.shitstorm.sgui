using _ARK_;
using UnityEngine;

namespace _SGUI_.composer
{
    public sealed partial class FrameHeader : MonoBehaviour
    {
        [SerializeField] FrameHeaderButton prefab_button;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            prefab_button.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public FrameHeaderButton AddButton(in Traductions label, in bool show_arrow = false, in bool use_toggle = false)
        {
            var clone = Instantiate(prefab_button, parent: prefab_button.transform.parent);
            clone.gameObject.SetActive(true);
            clone.Initialize(show_arrow: show_arrow, use_toggle: use_toggle);
            clone.label.SetTraductions(label);
            return clone;
        }
    }
}