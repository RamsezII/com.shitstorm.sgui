using UnityEngine;

namespace _SGUI_
{
    partial class SguiCustom
    {
        public enum CancelTypes : byte
        {
            Off,
            Cancel,
            No,
            Back,
        }

        public enum ConfirmTypes : byte
        {
            Off,
            Ok,
            Yes,
            Confirm,
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetCancelButton(in CancelTypes type)
        {
            switch (type)
            {
                case CancelTypes.Off:
                    button_cancel.gameObject.SetActive(false);
                    break;

                case CancelTypes.Cancel:
                    trad_cancel.SetTraductions(new() { french = "Annuler", english = "Cancel", });
                    break;

                case CancelTypes.No:
                    trad_cancel.SetTraductions(new() { french = "Non", english = "No", });
                    break;

                case CancelTypes.Back:
                    trad_cancel.SetTraductions(new() { french = "Retour", english = "Back", });
                    break;

                default:
                    Debug.LogError($"Wrong {typeof(CancelTypes)}: '{type}'");
                    break;
            }
        }

        public void SetConfirmButton(in ConfirmTypes type)
        {
            switch (type)
            {
                case ConfirmTypes.Off:
                    button_confirm.gameObject.SetActive(false);
                    break;

                case ConfirmTypes.Ok:
                    trad_confirm.SetText("Ok");
                    break;

                case ConfirmTypes.Yes:
                    trad_confirm.SetTraductions(new() { french = "Oui", english = "Yes", });
                    break;

                case ConfirmTypes.Confirm:
                    trad_confirm.SetTraductions(new() { french = "Confirmer", english = "Confirm", });
                    break;

                default:
                    Debug.LogError($"Wrong {typeof(ConfirmTypes)}: '{type}'");
                    break;
            }
        }
    }
}