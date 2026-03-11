using System.Collections.Generic;
using StateCharts;
using UnityEngine;

public class StateChartTest : MonoBehaviour
{
    private StateChart stateChart;
    void Start()
    {
        AtomicState stateA = new AtomicState("State A", stateChart: stateChart);
        AtomicState stateB = new AtomicState("State B", stateChart: stateChart);
        CompoundState rootState = new CompoundState("Root State", stateChart: stateChart, children: new HashSet<State> { stateA, stateB }, initialState: stateA);
        stateChart = new StateChart(rootState);
        stateChart.Activate();
    }

    void Update()
    {
        stateChart.Update(Time.deltaTime);
    }

}
