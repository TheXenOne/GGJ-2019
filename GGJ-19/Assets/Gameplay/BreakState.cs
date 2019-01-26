using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Gameplay
{
    /// <summary>
    /// The "idle" state between battles
    /// </summary>
    public class BreakState : ILoopState
    {
        List<GameObject> wagons = new List<GameObject>();

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}
