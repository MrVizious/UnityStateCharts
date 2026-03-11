using System;
using System.Collections.Generic;
using NodeTree;
using UnityEngine;
using UnityEngine.Events;
namespace StateCharts
{
    public abstract class State : Node<State>
    {
        #region Properties
        private string _name;
        public override string name
        {
            get
            {
                if (_name == null)
                {
                    _name = GetType().Name;
                }
                return _name;
            }
            protected set => _name = value;
        }
        public bool isActive { get; protected set; } = false;
        protected Action onEnterAction = null;
        protected Action onExitAction = null;
        public UnityEvent entered = new();
        public UnityEvent exited = new();
        private StateChart _stateChart;
        public StateChart stateChart
        {
            get
            {
                if (_stateChart != null) return _stateChart;
                if (parent != null) _stateChart = parent.stateChart;
                return _stateChart;
            }
            set => _stateChart = value;
        }

        #endregion

        #region Constructors
        public State() { }
        public State(string? name = null, State parent = null, HashSet<State> children = null, Action onEnterAction = null, Action onExitAction = null, StateChart stateChart = null)
        {
            if (name != null) this.name = name;
            if (parent != null) SetParent(parent);
            if (children != null) AddChildren(children);
            this.onEnterAction = onEnterAction;
            this.onExitAction = onExitAction;
            this.stateChart = stateChart;
        }
        #endregion

        #region Update Methods
        public virtual void FixedUpdate(float fixedDeltaTime)
        {
            if (!isActive) return;
            Debug.Log($"FixedUpdate from {name}");
        }
        public virtual void Update(float deltaTime)
        {
            if (!isActive) return;
            Debug.Log($"Update from {name}");
        }
        public virtual void LateUpdate(float deltaTime)
        {
            if (!isActive) return;
            Debug.Log($"LateUpdate from {name}");
        }
        #endregion

        #region Enter and Exit Methods
        public virtual void OnEnter()
        {
            onEnterAction?.Invoke();
        }
        public virtual void OnExit()
        {
            onExitAction?.Invoke();
        }
        #endregion

        #region Activation Methods
        public abstract void Activate();
        public abstract void Deactivate();
        #endregion
    }
}
