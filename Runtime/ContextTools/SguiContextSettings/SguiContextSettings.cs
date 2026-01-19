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

            transform.Find("raycast").GetComponent<PointerClickHandler>().onClick += eventData =>
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

        public ContextSetting_item AddItem<T>(in Traductions trad) where T : ContextSetting_item => AddItem(typeof(T), trad);
        public ContextSetting_item AddItem(in Type type, in Traductions trad)
        {
            var clone = prefabs[type].Clone(true);
            clone.trad.SetTrads(trad);
            AutoSize();
            return clone;
        }

        public void AutoSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, _AutoSize_Immediate);
        public void _AutoSize_Immediate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);
            float height = vlayout.preferredHeight;
            rt.sizeDelta = new Vector2(init_size.x, Mathf.Min(init_size.y, height));
            Rect r = rt.rect;
            Vector2 displ = Vector2.zero;
            displ += Vector2.Max(r.min, Vector2.zero);
            displ -= Vector2.Min(SguiGlobal.instance.rt_screen.rect.max - r.max, Vector2.zero);
            rt.anchoredPosition += displ;
        }

        void DestroyChildren()
        {
            foreach (var clone in clones)
                Destroy(clone.gameObject);
            clones.Clear();
        }

        public void OpenHere(in Vector2 position, in Traductions title)
        {
            DestroyChildren();
            gameObject.SetActive(true);
            rt.position = position;
            AddItem<SettingsHeader>(title);
        }
    }
}