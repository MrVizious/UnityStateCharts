using System;
using UnityEngine;

namespace StateCharts
{
    public struct TransitionGuard
    {
        public Func<bool> canActivate;
    }
}
