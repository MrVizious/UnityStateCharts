using System;
using System.Collections.Generic;
using StateCharts;
using UnityEngine;

public class CompoundState : State
{
    public State initialState;

    #region Constructors
    public CompoundState(string name = null, State parent = null, State initialState = null, HashSet<State> children = null, Action onEnterAction = null, Action onExitAction = null, StateChart stateChart = null)
        : base(name, parent, children, onEnterAction, onExitAction, stateChart)
    {
        SetInitialState(initialState);
    }
    #endregion

    public void AddInitialState(State initialState)
    {
        AddChild(initialState);
        SetInitialState(initialState);
    }
    public void SetInitialState(State state)
    {
        if (state == null || !children.Contains(state))
        {
            Debug.LogError($"Initial state must be a child of the compound state.");
            return;
        }
        initialState = state;
    }

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

    #region Activation Methods
    public override void Activate()
    {
        if (isActive) return;
        isActive = true;
        onEnterAction?.Invoke();
        entered.Invoke();
        initialState.Activate();
    }
    public override void Deactivate()
    {
        if (!isActive) return;
        isActive = false;
        foreach (var state in children)
        {
            state.Deactivate();
        }
        onEnterAction?.Invoke();
        exited.Invoke();
    }
    #endregion
}
