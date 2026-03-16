using System;
using System.Collections.Generic;
using StateCharts;
using UnityEngine;

public class CompoundState : State
{
    public State initialState;
    public State activeState;

    #region Constructors
    public CompoundState(string name = null, State parent = null, State initialState = null, HashSet<State> children = null, Action onEnterAction = null, Action onExitAction = null, StateChart stateChart = null)
        : base(name, parent, children, onEnterAction, onExitAction, stateChart)
    {
        if (initialState != null)
        {
            AddChild(initialState);
            SetInitialState(initialState);
        }
    }
    #endregion

    public void AddInitialState(State initialState)
    {
        Debug.Log($"Adding new state {initialState.name}");
        bool success = AddChild(initialState);
        Debug.Log($"Has succeded? {success}");
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
        activeState?.Update(deltaTime);
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
        ActivateChild(initialState);
    }
    public override void Deactivate()
    {
        if (!isActive) return;
        isActive = false;
        foreach (var state in children)
        {
            state.Deactivate();
        }
        activeState = null;
        onEnterAction?.Invoke();
        exited.Invoke();
    }

    public override void RequestActivationFromChild(State requestingState)
    {
        if (!stateChart.IsAncestorOf(this, requestingState))
        {
            Debug.LogError($"Requesting activation from {requestingState.name} to {name} compound state, but the requesting state is not an ancestor of this compound state.");
            return;
        }
        if (isActive)
        {
            ActivateChild(requestingState);
            return;
        }
        parent.RequestActivationFromChild(this);
    }

    private void ActivateChild(State childToActivate)
    {
        foreach (State child in children)
        {
            if (child == childToActivate)
            {
                child.Activate();
                activeState = child;
            }
            else child.Deactivate();
        }
    }

    #endregion
}