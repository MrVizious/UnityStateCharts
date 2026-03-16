using System.Collections.Generic;
using UnityEngine;


namespace NodeTree
{
    [System.Serializable]
    public abstract class Node<T> where T : Node<T>
    {
        public virtual string name { get; protected set; }
        public T parent { get; protected set; }
        public Tree<T> tree { get; protected set; }
        public virtual HashSet<T> children { get; protected set; } = new();
        protected Dictionary<T, bool> isAncestorCache = new();
        public virtual void SetTree(Tree<T> newTree)
        {
            tree = newTree;
            foreach (T child in children)
            {
                child.SetTree(newTree);
            }
        }
        public bool SetParent(T newParent, bool forceChange = false)
        {
            if (parent != null && !forceChange) return false;

            parent = newParent;
            if (parent != null)
            {
                SetTree(newParent.tree);
            }
            else
            {
                SetTree(null);
            }
            return true;
        }
        public virtual bool AddChild(T newChild)
        {
            Debug.Log($"Trying to add child {newChild.name} to {name}");
            if (newChild == null)
            {
                Debug.Log($"Child {newChild.name} NOT added because it is null");
                return false;
            }
            if (newChild == this)
            {
                Debug.Log($"Child {newChild.name} NOT added because it is the same as the parent");
                return false;
            }
            if (tree != null && tree.IsAncestorOf((T)this, newChild))
            {
                Debug.Log($"Child {newChild.name} NOT added because it is an descendant of the parent {name}");
                return false;
            }
            if (tree != null && tree.IsAncestorOf(newChild, (T)this))
            {
                Debug.Log($"Child {newChild.name} NOT added because it is an ancestor of the parent {name}");
                return false;
            }
            bool parentSet = newChild.SetParent((T)this);
            Debug.Log($"Could set parent? {parentSet}");
            if (parentSet)
            {
                children.Add(newChild);
                Debug.Log($"Added state {newChild.name}");
                return true;
            }
            Debug.Log($"Child {newChild.name} NOT added because ???");
            return false;
        }
        public virtual void AddChildren(IEnumerable<T> newChildren)
        {
            foreach (T child in newChildren)
            {
                AddChild(child);
            }
        }

        public virtual bool RemoveChild(T childToRemove)
        {
            if (children.Remove(childToRemove))
            {
                childToRemove.SetParent(null, true);
                childToRemove.SetTree(null);
                return true;
            }
            return false;
        }

        public string GetTreeString(string prefix = "", bool isLast = true)
        {
            string result = prefix;

            if (parent != null)
            {
                result += isLast ? "\\_>" : "|_>";
            }

            result += name + "\n";

            int i = 0;
            foreach (T child in children)
            {
                bool lastChild = i == children.Count - 1;

                string newPrefix = prefix;
                if (parent != null)
                {
                    newPrefix += isLast ? "    " : "|   ";
                }

                result += child.GetTreeString(newPrefix, lastChild);
                i++;
            }

            return result;
        }
    }

}