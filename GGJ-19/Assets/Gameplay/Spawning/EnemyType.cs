using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Gameplay.Spawning
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "GGJ/Create Enemy Type", order = 1)]
    public class EnemyType : ScriptableObject
    {
        public GameObject m_prefab;
        public int m_difficulty;
        public int m_cost;
        public int m_hitpoints;
    }
}
