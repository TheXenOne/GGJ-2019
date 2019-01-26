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
    /// The battle state
    /// </summary>
    public class BattleState : LoopState
    {
        List<GameObject> m_enemies = new List<GameObject>();

        public override void Enter()
        {
            // TODO: Spawn enemies
        }

        public void Update()
        {
            //m_enemies.RemoveAll(e => e.hitpoints <= 0);

            if (m_enemies.Count == 0)
            {
                // All enemies are dead, transition to break
                Gameplay.Instance.ChangeStateWithTravelTo<BreakState>();
            }
        }

        public override void Exit()
        {
            // Cleanup corpses, gore, etc
        }
    }
}
