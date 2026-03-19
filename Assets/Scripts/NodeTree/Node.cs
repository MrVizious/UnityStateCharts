using UnityEngine;

namespace NodeTree
{
    public abstract class Node<T> where T : Node<T>
    {
        public Tree<T> tree { get; protected set; }
        public void SetTree(Tree<T> newTree)
        {
            tree = newTree;
        }
        public virtual string name { get; protected set; }
    }
}