using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace NodeTree
{
    public class Tree<T> where T : Node<T>
    {
        public T entryNode;
        private HashSet<T> _nodes = new();
        private Dictionary<string, T> _nodeNames = new();
        private Dictionary<T, HashSet<T>> _children = new();
        private Dictionary<T, T> _parents = new();

        private Dictionary<(T, T), bool> _ancestryCache = new();
        private Dictionary<(T, T), T> _lcaCache = new();
        public T GetNodeByName(string nameToFind)
        {
            if (_nodeNames.TryGetValue(nameToFind, out T node))
            {
                return node;
            }
            return default;
        }

        public void SetEntryNode(T node)
        {
            if (node == null)
            {
                Debug.LogError($"Entry node cannot be null.");
                return;
            }
            _nodes.Add(node);
            _nodeNames[node.name] = node;
            node.SetTree(this);
            entryNode = node;
        }

        public virtual bool AddChild(T parent, T child)
        {
            if (parent == null || child == null)
            {
                Debug.LogError($"Parent and child cannot be null.");
                return false;
            }
            if (!_nodes.Contains(parent))
            {
                Debug.LogError($"Parent node {parent.name} is not in the tree.");
                return false;
            }
            if (_nodes.Contains(child))
            {
                Debug.LogError($"Child node {child.name} is already in the tree.");
                return false;
            }
            if (_nodeNames.ContainsKey(child.name))
            {
                Debug.LogError($"There is already a node with name {child.name} in the tree.");
                return false;
            }

            // Add child to the tree
            _parents[child] = parent;
            T currentAncestor = parent;
            while (currentAncestor != null)
            {
                _ancestryCache[(currentAncestor, child)] = true;
                currentAncestor = GetParent(currentAncestor);
            }
            _lcaCache[OrderPair(parent, child)] = parent;

            if (!_children.ContainsKey(parent))
            {
                _children[parent] = new HashSet<T>();
            }
            _children[parent].Add(child);
            child.SetTree(this);

            _nodes.Add(child);
            _nodeNames[child.name] = child;
            return true;
        }

        private static (T, T) OrderPair(T node1, T node2)
        {
            return Comparer<int>.Default.Compare(node1.GetHashCode(), node2.GetHashCode()) <= 0 ? (node1, node2) : (node2, node1);
        }

        public HashSet<T> GetChildren(T parent)
        {
            if (_children.TryGetValue(parent, out HashSet<T> children))
            {
                return children;
            }
            return new HashSet<T>();
        }
        public T GetParent(T child)
        {
            if (_parents.TryGetValue(child, out T parent))
            {
                return parent;
            }
            return default;
        }

        public T GetLowestCommonAncestor(T node1, T node2)
        {
            if (node1 == null || node2 == null) return default;
            if (!_nodes.Contains(node1) || !_nodes.Contains(node2))
            {
                Debug.LogError($"Both nodes must be in the tree.");
                return default;
            }

            // Existing LCA cache check
            if (_lcaCache.TryGetValue(OrderPair(node1, node2), out T cachedLCA))
            {
                return cachedLCA;
            }

            // Get all ancestors of node1
            HashSet<T> ancestors1 = new();
            T current = node1;
            while (current != null)
            {
                ancestors1.Add(current);
                current = GetParent(current);
            }

            // Find the first ancestor of node2 that is also an ancestor of node1
            current = node2;
            while (current != null)
            {
                if (ancestors1.Contains(current))
                {
                    // Cache and return the result
                    _lcaCache[OrderPair(node1, node2)] = current;
                    return current;
                }
                current = GetParent(current);
            }

            return default;
        }

        public bool IsAncestorOf(T ancestor, T descendant)
        {
            if (ancestor == null || descendant == null) return false;
            if (!_nodes.Contains(ancestor) || !_nodes.Contains(descendant))
            {
                Debug.LogError($"Both nodes must be in the tree.");
                return false;
            }

            // Existing ancestry cache check
            if (_ancestryCache.TryGetValue((ancestor, descendant), out bool isAncestor))
            {
                return isAncestor;
            }

            // Traverse up from descendant to see if we reach ancestor
            T current = descendant;
            while (current != null)
            {
                if (current.Equals(ancestor))
                {
                    _ancestryCache[(ancestor, descendant)] = true;
                    return true;
                }
                current = GetParent(current);
            }

            _ancestryCache[(ancestor, descendant)] = false;
            return false;
        }
    }
}