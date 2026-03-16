using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeTree
{
    public class Tree<T> where T : Node<T>
    {
        public T entryNode;
        private Dictionary<(T, T), T> lcaCache = new();
        private Dictionary<(T, T), bool> ancestryCache = new();
        public T LowestCommonAncestor(T a, T b)
        {
            if (a == null || b == null) return null;
            (T, T) pair = MakeKey(a, b);
            T cachedLCA;
            if (lcaCache.TryGetValue(pair, out cachedLCA))
            {
                return cachedLCA;
            }

            HashSet<T> visited = new();

            while (a != null || b != null)
            {
                if (a != null)
                {
                    if (!visited.Add(a))
                    {
                        lcaCache[pair] = a; // Cache the result before returning
                        return a;
                    }
                    a = a.parent;
                }

                if (b != null)
                {
                    if (!visited.Add(b))
                    {
                        lcaCache[pair] = b; // Cache the result before returning
                        return b;
                    }
                    b = b.parent;
                }
            }

            return null;
        }
        private static (T, T) MakeKey(T a, T b)
        {
            int hashA = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(a);
            int hashB = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(b);

            if (hashA != hashB)
                return hashA < hashB ? (a, b) : (b, a);

            return ReferenceEquals(a, b) ? (a, b) : (a.GetHashCode() <= b.GetHashCode() ? (a, b) : (b, a));
        }

        public bool IsAncestorOf(T ancestor, T descendant)
        {
            if (ancestor == null || descendant == null) return false;
            bool cachedResult;
            if (ancestryCache.TryGetValue((ancestor, descendant), out cachedResult))
            {
                return cachedResult;
            }

            bool isDescendantParentAncestor = IsAncestorOf(ancestor, descendant.parent);
            if (isDescendantParentAncestor)
            {
                ancestryCache[(ancestor, descendant)] = true;
                return true;
            }

            return false;
        }
    }

}