using System;
using System.Collections.Generic;
using StateCharts;
using UnityEngine;

public class ParallelState : State
{
    #region Update Methods
    public override void FixedUpdate(float fixedDeltaTime)
    {
        if (!isActive) return;
        if (tree == null) return;
        foreach (var state in tree.GetChildren(this))
        {
            state.FixedUpdate(fixedDeltaTime);
        }
    }
    public override void Update(float deltaTime)
    {
        if (!isActive) return;
        if (tree == null) return;
        foreach (var state in tree.GetChildren(this))
        {
            state.Update(deltaTime);
        }
    }
    public override void LateUpdate(float deltaTime)
    {
        if (!isActive) return;
        if (tree == null) return;
        foreach (var state in tree.GetChildren(this))
        {
            state.LateUpdate(deltaTime);
        }
    }
    #endregion

    #region Constructors
    public ParallelState(string? name, Action onEnterAction = null, Action onExitAction = null) : base(name, onEnterAction, onExitAction) { }
    #endregion

    #region Activation Methods
    public override void Activate()
    {
        if (isActive) return;
        isActive = true;
        onEnterAction?.Invoke();
        entered.Invoke();
        if (tree != null)
        {
            foreach (var state in tree.GetChildren(this))
            {
                state.Activate();
            }
        }
    }

    public override bool ActivateState(State stateToActivate)
    {
        if (!stateChart.IsAncestorOf(this, stateToActivate)) return false;
        isActive = true;
        foreach (var child in tree.GetChildren(this))
        {
            if (tree.IsAncestorOf(child, stateToActivate))
            {
                child.ActivateState(stateToActivate);
            }
            else
            {
                child.Activate();
            }
        }
        return true;
    }

    public override void Deactivate()
    {
        if (!isActive) return;
        isActive = false;
        if (tree != null)
        {
            foreach (var state in tree.GetChildren(this))
            {
                state.Deactivate();
            }
        }
        onExitAction?.Invoke();
        exited.Invoke();
    }
    #endregion
}