using UnityEngine;

namespace _SGUI_
{
    public enum SguiCancelTypes : byte
    {
        Off,
        Cancel,
        No,
        Back,
    }

    public enum SguiConfirmTypes : byte
    {
        Off,
        Ok,
        Yes,
        Confirm,
        Save,
        Apply,
    }

    partial class SguiCustom
    {
        //--------------------------------------------------------------------------------------------------------------

        public void SetDialogButtons(in SguiCancelTypes cancel, in SguiConfirmTypes confirm)
        {
            SetCancelButton(cancel);
            SetConfirmButton(confirm);
        }

        public void SetCancelButton(in SguiCancelTypes type)
        {
            switch (type)
            {
                case SguiCancelTypes.Off:
                    button_cancel.gameObject.SetActive(false);
                    break;

                case SguiCancelTypes.Cancel:
                    trad_cancel.SetTraductions(new() { french = "Annuler", english = "Cancel", });
                    break;

                case SguiCancelTypes.No:
                    trad_cancel.SetTraductions(new() { french = "Non", english = "No", });
                    break;

                case SguiCancelTypes.Back:
                    trad_cancel.SetTraductions(new() { french = "Retour", english = "Back", });
                    break;

                default:
                    Debug.LogError($"Wrong {typeof(SguiCancelTypes)}: '{type}'");
                    break;
            }
        }

        public void SetConfirmButton(in SguiConfirmTypes type)
        {
            switch (type)
            {
                case SguiConfirmTypes.Off:
                    button_confirm.gameObject.SetActive(false);
                    break;

                case SguiConfirmTypes.Ok:
                    trad_confirm.SetText("Ok");
                    break;

                case SguiConfirmTypes.Yes:
                    trad_confirm.SetTraductions(new() { french = "Oui", english = "Yes", });
                    break;

                case SguiConfirmTypes.Confirm:
                    trad_confirm.SetTraductions(new() { french = "Confirmer", english = "Confirm", });
                    break;

                case SguiConfirmTypes.Save:
                    trad_confirm.SetTraductions(new() { french = "Sauvegarder", english = "Save", });
                    break;

                case SguiConfirmTypes.Apply:
                    trad_confirm.SetTraductions(new() { french = "Appliquer", english = "Apply", });
                    break;

                default:
                    Debug.LogError($"Wrong {typeof(SguiConfirmTypes)}: '{type}'");
                    break;
            }
        }
    }
}