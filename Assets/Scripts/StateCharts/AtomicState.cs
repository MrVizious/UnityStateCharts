using System;
using StateCharts;
using UnityEngine;

public class AtomicState : State
{
    #region Child Management
    public override bool AddChild(State newChild) => false;
    public override bool RemoveChild(State node) => false;
    #endregion

    public AtomicState(string name = null, State parent = null, Action onEnterAction = null, Action onExitAction = null, StateChart stateChart = null)
        : base(name, parent, onEnterAction: onEnterAction, onExitAction: onExitAction, stateChart: stateChart) { }

    #region Activation Methods
    public override void Activate()
    {
        if (isActive) return;
        isActive = true;
        stateChart?.AddActiveAtomicState(this);
        onEnterAction?.Invoke();
        entered.Invoke();
    }
    public override void Deactivate()
    {
        if (!isActive) return;
        isActive = false;
        stateChart?.RemoveActiveAtomicState(this);
        onExitAction?.Invoke();
        exited.Invoke();
    }

    public override void RequestActivationFromChild(State requestingState)
    {
        Debug.LogError($"Trying to request activation to {name} atomic state as if it were a parent state");
    }

    public override void Update(float deltaTime)
    {
    }
    #endregion
}
