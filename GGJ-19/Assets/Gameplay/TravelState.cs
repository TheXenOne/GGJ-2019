using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Gameplay
{
    /// <summary>
    /// The state between the break and and battle states, for transitioning
    /// </summary>
    public class TravelState : LoopState
    {
        public LoopState m_nextState;
        public int m_timeToTransit = 4;

        int m_skipTime;

        public override void Enter()
        {
            m_skipTime = (int)Time.time + m_timeToTransit;
        }

        public void Update()
        {
            if (Time.time > m_skipTime)
            {
                Gameplay.Instance.ChangeState(m_nextState);
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
