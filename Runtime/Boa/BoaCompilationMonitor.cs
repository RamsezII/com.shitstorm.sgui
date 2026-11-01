using _ARK_;
using _BOA_;
using _UTIL_;
using UnityEngine;

namespace _SGUI_.osview
{
    sealed partial class BoaCompilationMonitor : MonoBehaviour
    {
        readonly TickTimer timer = new();
        [SerializeField] RectTransform rt_icon;
        int angle;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            gameObject.SetActive(false);

            BoaManager.isCompiling.AddListener(gameObject.SetActive);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            NUCLEOR.delegates.LateUpdate += OnTick;
        }

        private void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= OnTick;
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnTick()
        {
            if (!timer.Tick(Time.time, .1f))
                return;

            angle = ++angle % 8;
            rt_icon.localEulerAngles = new(0, 0, -angle * 45);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            BoaManager.isCompiling.RemoveListener(gameObject.SetActive);
        }
    }
}