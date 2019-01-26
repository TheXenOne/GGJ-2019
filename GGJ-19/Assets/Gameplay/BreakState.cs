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
    public class BreakState : LoopState
    {
        public int m_secondsToSkip = 30;

        int m_skipTime;

        public override void Enter()
        {
            m_skipTime = (int)Time.time + m_secondsToSkip;
        }

        public void Update()
        {
            if (Time.time > m_skipTime)
            {
                Gameplay.Instance.ChangeStateWithTravelTo<BattleState>();
                return;
            }
        }

        public override void Exit()
        {
            // TODO
        }
    }
}
