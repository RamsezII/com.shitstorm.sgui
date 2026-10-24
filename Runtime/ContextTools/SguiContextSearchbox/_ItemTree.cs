using _ARK_;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiSearchbox
    {
        public sealed class ItemNode
        {
            public readonly string label;
            public Action action;
            public Traductions hover_infos;
            public readonly List<ItemNode> children = new();
            public ItemNode(in string label) => this.label = label;
        }

        public delegate ItemNode OnAddSearchboxItems(Action onClick, params string[] path);

        //--------------------------------------------------------------------------------------------------------------

        public void OpenHere(in Vector2 screenPosition, in Traductions title, Action<OnAddSearchboxItems> onCreateItems, Camera eventCamera = null)
        {
            Toggle(true);
            trad_title.SetTraductions(title);
            ArkUI.instance.SetScreenPosition(transform.AsRTfm(), screenPosition, eventCamera);

            var root = new ItemNode(default);
            onCreateItems(OnAddItem);
            ShowChildren(root);

            ItemNode OnAddItem(Action onClick, params string[] path)
            {
                ItemNode node = root;

                foreach (string branch in path)
                {
                    var child = node.children.Find(item => item.label.Equals(branch, StringComparison.OrdinalIgnoreCase));
                    if (child == null)
                    {
                        child = new ItemNode(branch);
                        node.children.Add(child);
                    }
                    node = child;
                }

                node.action = onClick;
                return node;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        void ShowChildren(ItemNode parent)
        {
            ClearClones();

            foreach (var child in parent.children)
            {
                var clone = Instantiate(prefab_item, parent: prefab_item.transform.parent);
                clone.label.text = child.label;
                clone.hover_infos = child.hover_infos;

                if (child.children.Count > 0)
                {
                    clone.ShowArrow();
                    clone.button.onClick.AddListener(() => ShowChildren(child));
                }
                else
                {
                    clone.button.onClick.AddListener(() =>
                    {
                        Close();
                        child.action();
                    });
                }

                clone.gameObject.SetActive(true);
                clones.Add(clone);
            }

            AutoSize();
        }
    }
}
