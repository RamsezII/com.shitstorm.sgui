using UnityEngine;

namespace _SGUI_
{
    public abstract partial class SguiWindow1 : SguiWindow
    {

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            AwakeUI();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                CheckBounds();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            OnPopulateDropdowns();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Oblivionize()
        {
            if (oblivionized)
                return;
            oblivionized = true;
            sgui_toggle_window.Update(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            onDestroy?.Invoke();
            instances.RemoveElement(this);
            Debug.Log($"destroyed {GetType().FullName} ({transform.GetPath(true)})".ToSubLog());
        }
    }
}