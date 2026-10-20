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

        public FrameHeaderButton AddButton(in Traductions label, in bool arrow = false, in bool toggle = false)
        {
            var clone = Instantiate(prefab_button, parent: prefab_button.transform.parent);
            clone.gameObject.SetActive(true);
            clone.Initialize(show_arrow: arrow, use_toggle: toggle);
            clone.label.SetTraductions(label);
            return clone;
        }
    }
}