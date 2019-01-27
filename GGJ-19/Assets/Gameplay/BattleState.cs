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
        List<Enemy> m_enemies = new List<Enemy>();
        int m_killScore = 0;

        public override void Enter()
        {
            int spawnScore = Gameplay.Instance.m_battlesFought * 2 + 3;
            var spawnManager = Gameplay.Instance.GetComponent<SpawnManager>();

            m_killScore = spawnScore;

            spawnManager.Spawn(spawnScore, (obj) => m_enemies.Add(obj.GetComponent<Enemy>()));
        }

        public void Update()
        {
            var dead = m_enemies.Where(e => e.hitPoints <= 0).ToList();

            foreach (var enemy in dead)
            {
                var enemyAI = enemy.GetComponent<EnemyAI>();
                enemyAI.enabled = false;
            }

            if (m_enemies.Count == dead.Count)
            {
                Gameplay.Instance.ChangeStateWithTravelTo<BattleState>();
                return;
            }
        }

        override public void Exit()
        {
            Gameplay.Caravan.m_currency += m_killScore;
            Gameplay.Instance.m_battlesFought++;

            foreach (var enemy in m_enemies)
            {
                Destroy(enemy.gameObject);
            }

            m_enemies.Clear();
        }
    }
}
