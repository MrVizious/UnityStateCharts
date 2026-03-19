using System;
using StateCharts;
using UnityEngine;

public class AtomicState : State
{
    public AtomicState(string? name, Action onEnterAction = null, Action onExitAction = null) : base(name, onEnterAction, onExitAction) { }
    #region Activation Methods
    public override void Activate()
    {
        if (isActive) return;
        isActive = true;
        stateChart?.AddActiveAtomicState(this);
        onEnterAction?.Invoke();
        entered.Invoke();
    }
    public override bool ActivateState(State childToActivate)
    {
        if (childToActivate != this) return false;
        Activate();
        return true;
    }

    public override bool CanAddChild(State child) => false;

    public override void Deactivate()
    {
        if (!isActive) return;
        isActive = false;
        stateChart?.RemoveActiveAtomicState(this);
        onExitAction?.Invoke();
        exited.Invoke();
    }

    public override void Update(float deltaTime)
    {
        if (!isActive) return;
        Debug.Log($"Updating {name}");
    }
    #endregion
}
