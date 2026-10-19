using _ARK_;
using UnityEngine;

namespace _SGUI_.composer
{
    public sealed partial class SguiFrameHeader : MonoBehaviour
    {
        [SerializeField] SguiFrameHeaderButton prefab_button;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            prefab_button.gameObject.SetActive(false);
        }

        //----------------------------------------------------------------------------------------------------------

        public SguiFrameHeaderButton AddButton(in Traductions label)
        {
            var clone = Instantiate(prefab_button, parent: prefab_button.transform.parent);
            clone.gameObject.SetActive(true);
            clone.label.SetTraductions(label);
            return clone;
        }
    }
}