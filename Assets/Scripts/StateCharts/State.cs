using System;
using System.Collections.Generic;
using NodeTree;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
namespace StateCharts
{
    [System.Serializable]
    public abstract class State : Node<State>
    {
        #region Properties
        [OdinSerialize]
        [ShowInInspector]
        private string _name;
        public override string name
        {
            get
            {
                if (_name == null)
                {
                    _name = GetType().Name + GetHashCode();
                }
                return _name;
            }
            protected set => _name = value;
        }
        [OdinSerialize]
        [ShowInInspector]
        public bool isActive { get; protected set; } = false;
        [OdinSerialize]
        protected Action onEnterAction = null;
        [OdinSerialize]
        protected Action onExitAction = null;
        [OdinSerialize]
        public UnityEvent entered = new();
        [OdinSerialize]
        public UnityEvent exited = new();
        [OdinSerialize]
        [ShowInInspector]
        public StateChart stateChart => tree as StateChart;


        #endregion

        #region Constructors
        public State(string? name = null, Action onEnterAction = null, Action onExitAction = null)
        {
            if (name != null) this.name = name;
            this.onEnterAction = onEnterAction;
            this.onExitAction = onExitAction;
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
        protected virtual void OnEnter()
        {
            onEnterAction?.Invoke();
        }
        protected virtual void OnExit()
        {
            onExitAction?.Invoke();
        }
        #endregion

        #region Activation Methods
        public abstract bool ActivateState(State stateToActivate);
        public abstract void Activate();
        public abstract void Deactivate();
        #endregion

        #region Child Methods
        public virtual bool CanAddChild(State child) => true;

        #endregion
    }
}
