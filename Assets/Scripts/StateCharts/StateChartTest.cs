using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using StateCharts;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateChartTest : MonoBehaviour
{
    [OdinSerialize, ShowInInspector]
    private StateChart stateChart;
    CompoundState rootState = new("Root State");
    CompoundState stateA = new("State A");
    AtomicState stateE = new("State E");
    AtomicState stateF = new("State F");
    ParallelState stateB = new("State B");
    AtomicState stateC = new("State C");
    AtomicState stateD = new("State D");
    void Start()
    {
        stateChart = new StateChart(rootState);

        // Children of Root
        stateChart.AddChild(rootState, stateA);
        stateChart.AddChild(rootState, stateB);
        rootState.SetInitialState(stateA);

        // Children of A
        stateChart.AddChild(stateA, stateE);
        stateChart.AddChild(stateA, stateF);
        stateA.SetInitialState(stateE);

        // Children of B
        stateChart.AddChild(stateB, stateC);
        stateChart.AddChild(stateB, stateD);

        stateChart.Activate();
    }

    void Update()
    {
        stateChart.Update(Time.deltaTime);
    }

    public void ChangeToE(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            stateChart.entryNode.ActivateState(stateE);
        }
    }
    public void ChangeToF(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            stateChart.entryNode.ActivateState(stateF);
        }
    }
    public void ChangeToD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            stateChart.entryNode.ActivateState(stateD);
        }
    }

}
