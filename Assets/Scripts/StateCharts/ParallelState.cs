using System;
using System.Collections.Generic;
using StateCharts;

public class ParallelState : State
{
    #region Update Methods
    public override void FixedUpdate(float fixedDeltaTime)
    {
        if (!isActive) return;
        foreach (var state in children)
        {
            state.FixedUpdate(fixedDeltaTime);
        }
    }
    public override void Update(float deltaTime)
    {
        if (!isActive) return;
        foreach (var state in children)
        {
            state.Update(deltaTime);
        }
    }
    public override void LateUpdate(float deltaTime)
    {
        if (!isActive) return;
        foreach (var state in children)
        {
            state.LateUpdate(deltaTime);
        }
    }
    #endregion

    #region Constructors
    public ParallelState() { }
    public ParallelState(string? name = null, State parent = null, HashSet<State> children = null, Action onEnterAction = null, Action onExitAction = null, StateChart stateChart = null)
        : base(name, parent, children, onEnterAction, onExitAction, stateChart) { }
    #endregion

    #region Activation Methods
    public override void Activate()
    {
        if (isActive) return;
        onEnterAction?.Invoke();
        entered.Invoke();
        isActive = true;
        foreach (var state in children)
        {
            state.Activate();
        }
    }
    public override void Deactivate()
    {
        if (!isActive) return;
        isActive = false;
        foreach (var state in children)
        {
            state.Deactivate();
        }
        onExitAction?.Invoke();
        exited.Invoke();
    }
    #endregion
}