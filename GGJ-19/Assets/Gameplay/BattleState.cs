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
        public override void Enter()
        {
            int spawnScore = Gameplay.Instance.m_battlesFought * 2 + 3;
            var spawnManager = Gameplay.Instance.GetComponent<SpawnManager>();

            spawnManager.Spawn(spawnScore, (obj) => m_enemies.Add(obj));
        }

        override public void Exit()
        {
        }
    }
}
