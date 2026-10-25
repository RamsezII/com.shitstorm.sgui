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
            public readonly ItemNode parent;
            public readonly string label;
            public Action action;
            public Traductions hover_infos;
            public readonly List<ItemNode> children = new();
            public ItemNode(in ItemNode parent, in string label)
            {
                this.parent = parent;
                this.label = label;
            }
        }

        public delegate ItemNode OnAddSearchboxItems(Action onClick, params string[] path);

        //--------------------------------------------------------------------------------------------------------------

        public void OpenHere(in Vector2 screenPosition, in Traductions title, Action<OnAddSearchboxItems> onCreateItems, Camera eventCamera = null)
        {
            Toggle(true);
            trad_title.SetTraductions(title);
            ArkUI.instance.SetScreenPosition(rt_box, screenPosition, eventCamera);

            var root = new ItemNode(parent: null, label: null);
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
                        child = new ItemNode(node, branch);
                        node.children.Add(child);
                    }
                    node = child;
                }

                node.action += onClick;
                return node;
            }
        }

        void ShowChildren(ItemNode node)
        {
            ClearClones();

            button_category.onClick.RemoveAllListeners();
            button_category.gameObject.SetActive(node.parent != null);
            text_category.text = node.label;

            if (node.parent != null)
                button_category.onClick.AddListener(() => ShowChildren(node.parent));

            foreach (var child in node.children)
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
