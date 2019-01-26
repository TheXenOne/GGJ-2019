using Assets.Components;
using Assets.Gameplay.Spawning;
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
            int spawnScore = Gameplay.Instance.m_battlesFought * 2 + 3;
            var spawnManager = Gameplay.Instance.GetComponent<SpawnManager>();

            spawnManager.Spawn(spawnScore, (obj) => m_enemies.Add(obj));
        }

        public void Update()
        {
            //m_enemies.RemoveAll(e => e.hp <= 0);

            if (m_enemies.Count == 0)
            {
                Gameplay.Instance.ChangeStateWithTravelTo<BattleState>();
                return;
            }
        }

        override public void Exit()
        {
            // TODO: Clean up corpses etc
        }
    }
}
