using System.Collections.Generic;
using UnityEngine;

namespace NodeTree
{
    public abstract class Node<T> where T : Node<T>
    {
        public virtual string name { get; protected set; }
        public T parent { get; protected set; }
        public virtual HashSet<T> children { get; protected set; } = new();
        protected Dictionary<T, bool> isAncestorCache = new();
        public bool SetParent(T newParent, bool forceChange = false)
        {
            if (parent == null || forceChange)
            {
                parent = newParent;
                return true;
            }
            return false;
        }

        public virtual bool AddChild(T newChild)
        {
            if (newChild == null)
            {
                return false;
            }
            if (newChild == this)
            {
                return false;
            }
            if (this.IsAncestorOf(newChild))
            {
                return false;
            }
            if (newChild.IsAncestorOf((T)this))
            {
                return false;
            }
            if (newChild.SetParent((T)this))
            {
                children.Add(newChild);
                return true;
            }
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
                return true;
            }
            return false;
        }

        public virtual bool IsAncestorOf(T node)
        {
            foreach (T child in children)
            {
                if (child == node)
                {
                    return true;
                }
                if (child.IsAncestorOf(node))
                {
                    return true;
                }
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