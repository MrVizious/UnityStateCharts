using NodeTree;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateCharts
{
    [System.Serializable]
    public class StateChart : Tree<State>
    {
        protected HashSet<AtomicState> activeAtomicStates = new();
        public virtual void Update(float deltaTime)
        {
            entryNode.Update(deltaTime);
        }
        public virtual void LateUpdate(float deltaTime)
        {
            entryNode.LateUpdate(deltaTime);
        }
        public virtual void FixedUpdate(float fixedDeltaTime)
        {
            entryNode.FixedUpdate(fixedDeltaTime);
        }
        public bool AddActiveAtomicState(AtomicState state)
        {
            return activeAtomicStates.Add(state);
        }
        public bool RemoveActiveAtomicState(AtomicState state)
        {
            return activeAtomicStates.Remove(state);
        }
        public StateChart(State entryState = null)
        {
            if (entryState != null) SetEntryNode(entryState);
        }
        public void Activate()
        {
            entryNode.Activate();
        }
        public override bool AddChild(State parent, State child)
        {
            if (parent == null || child == null)
            {
                Debug.LogError($"Cannot add child to state chart. Parent or child is null.");
                return false;
            }
            if (!parent.CanAddChild(child)) return false;
            return base.AddChild(parent, child);
        }
    }
}
