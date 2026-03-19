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
    ParallelState rootState = new ParallelState("Root State");
    CompoundState stateA = new CompoundState("State A");
    AtomicState stateE = new AtomicState("State E");
    AtomicState stateF = new AtomicState("State F");
    ParallelState stateB = new ParallelState("State B");
    AtomicState stateC = new AtomicState("State C");
    AtomicState stateD = new AtomicState("State D");
    void Start()
    {
        stateChart = new StateChart(rootState);

        // Children of Root
        stateChart.AddChild(rootState, stateA);
        stateChart.AddChild(rootState, stateB);

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

    public void ChangeToF(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"Activating state F", this);
            stateChart.entryNode.ActivateState(stateF);
        }
    }
    public void ChangeToD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"Activating state D", this);
            stateChart.entryNode.ActivateState(stateD);
        }
    }

}
