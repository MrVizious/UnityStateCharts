using NodeTree;
using System.Collections.Generic;

namespace StateCharts
{
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
        public StateChart(State entryState)
        {
            entryNode = entryState;
            if (entryNode == null) return;
            entryState.stateChart = this;
        }
        public void Activate()
        {
            entryNode.Activate();
        }
    }
}
