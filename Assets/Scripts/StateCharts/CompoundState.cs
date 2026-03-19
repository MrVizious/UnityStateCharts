using System;
using System.Collections.Generic;
using StateCharts;
using Sirenix.Serialization;
using UnityEngine;

public class CompoundState : State
{
    [OdinSerialize]
    public State initialState;
    [OdinSerialize]
    public State activeState;

    public CompoundState(string? name, Action onEnterAction = null, Action onExitAction = null) : base(name, onEnterAction, onExitAction) { }
    public void AddInitialState(State initialState)
    {
        Debug.Log($"Adding new state {initialState.name}");
        if (tree != null)
        {
            bool success = tree.AddChild(this, initialState);
            Debug.Log($"Has succeeded? {success}");
            SetInitialState(initialState);
        }
    }
    public void SetInitialState(State state)
    {
        if (tree == null)
        {
            Debug.LogError($"Tree is null, cannot set initial state");
            return;
        }

        var children = tree.GetChildren(this);
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
        if (tree == null) return;
        foreach (var state in tree.GetChildren(this))
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
        if (tree == null) return;
        foreach (var state in tree.GetChildren(this))
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
        ActivateState(initialState);
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
        activeState = null;
        onEnterAction?.Invoke();
        exited.Invoke();
    }

    public override bool ActivateState(State stateToActivate)
    {
        if (tree == null) return false;

        bool stateActivated = false;
        foreach (State child in tree.GetChildren(this))
        {
            if (child.ActivateState(stateToActivate))
            {
                stateActivated = true;
                activeState = child;
                continue;
            }
            child.Deactivate();
        }
        return stateActivated;
    }

    #endregion
}