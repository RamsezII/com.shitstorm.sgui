using _ARK_;
using _SGUI_.context_tools.settings;
using _UTIL_;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.context_tools
{
    public class SguiContextSettings : MonoBehaviour
    {
        public interface IUser
        {
        }

        public static SguiContextSettings instance;

        public RectTransform rt;
        [SerializeField] VerticalLayoutGroup vlayout;

        readonly Dictionary<Type, ContextSetting_item> prefabs = new();
        internal readonly List<ContextSetting_item> clones = new();

        [SerializeField] Vector2 init_size;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            rt = (RectTransform)transform.Find("rt");
            vlayout = GetComponentInChildren<VerticalLayoutGroup>(true);

            transform.Find("raycast").GetComponent<PointerClickHandler>().onPointerDown += eventData =>
            {
                gameObject.SetActive(false);
            };

            foreach (var item in GetComponentsInChildren<ContextSetting_item>(true))
                prefabs.Add(item.GetType(), item);

            init_size = rt.sizeDelta;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDisable()
        {
            DestroyChildren();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            foreach (var item in prefabs.Values)
                item.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void OpenHere(in Vector2 position, in Vector2 pivot, in Traductions title)
        {
            DestroyChildren();
            gameObject.SetActive(true);
            rt.pivot = pivot;
            rt.position = position;
            AddItem<SettingsHeader>(title);
        }

        void DestroyChildren()
        {
            foreach (var clone in clones)
                Destroy(clone.gameObject);
            clones.Clear();
        }

        public T AddItem<T>(in Traductions trad) where T : ContextSetting_item => (T)AddItem(typeof(T), trad);
        public ContextSetting_item AddItem(in Type type, in Traductions trad)
        {
            var clone = prefabs[type].Clone(true);
            clone.label_trad.SetTrads(trad);
            AutoSize();
            return clone;
        }

        public void AutoSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, _AutoSize_Immediate);
        public void _AutoSize_Immediate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);
            float height = vlayout.preferredHeight;
            rt.sizeDelta = new Vector2(init_size.x, Mathf.Min(init_size.y, height));

            if (Util.GetStayInsideCorrection(rt, SguiGlobal.instance.rt_screen, 5 * Vector2.one, out Vector2 correction))
                rt.position += (Vector3)correction;
        }
    }
}