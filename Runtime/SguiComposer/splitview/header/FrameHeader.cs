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

        public FrameHeaderButton AddButton(in Traductions label, in bool arrow)
        {
            var clone = Instantiate(prefab_button, parent: prefab_button.transform.parent);
            clone.gameObject.SetActive(true);
            clone.Initialize(arrow: arrow);
            clone.label.SetTraductions(label);
            return clone;
        }
    }
}