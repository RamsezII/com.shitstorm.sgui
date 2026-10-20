using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.console
{
    public sealed partial class SguiConsoleMessage : MonoBehaviour, IPointerClickHandler
    {
        public SguiConsole console;
        [SerializeField] internal SguiConsole.LogMessage message;
        [SerializeField] internal TMP_Text tmptext;
        [SerializeField] Graphic graphic_bg;
        [SerializeField] RawImage rimg_icon;
        [SerializeField] internal Graphic graphic_select;
        [AutoStaticsCleanup] static bool pair;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize(in SguiConsole.LogMessage message)
        {
            console = GetComponentInParent<SguiConsole>(includeInactive: true);

            this.message = message;

            rimg_icon.texture = message.type switch
            {
                LogType.Error or LogType.Assert or LogType.Exception => console.icon_err,
                LogType.Warning => console.icon_warn,
                LogType.Log => console.icon_log,
                _ => rimg_icon.texture,
            };

            graphic_bg.color = (pair = !pair) ? new(1, 1, 1, .01f) : new(0, 0, 0, .02f);

            tmptext.text = $"{message.stacktrace}\n{message.text}";
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            console.OnSelectEntry(this);
        }
    }
}