using System.Collections.Generic;
using StateCharts;
using UnityEngine;

public class StateChartTest : MonoBehaviour
{
    [SerializeField]
    private StateChart stateChart;
    void Start()
    {
        CompoundState rootState = new CompoundState("Root State");
        AtomicState stateA = new AtomicState("State A");
        ParallelState stateB = new ParallelState("State B");
        AtomicState stateC = new AtomicState("State C");
        AtomicState stateD = new AtomicState("State D");
        rootState.AddChild(stateA);
        rootState.AddChild(stateB);
        rootState.SetInitialState(stateB);
        stateB.AddChild(stateC);
        stateB.AddChild(stateD);
        stateChart = new StateChart(rootState);
        stateChart.Activate();
    }

    void Update()
    {
        stateChart.Update(Time.deltaTime);
    }

}
