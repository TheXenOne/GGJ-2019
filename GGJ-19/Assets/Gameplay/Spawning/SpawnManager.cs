using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Gameplay.Spawning
{
    public class SpawnManager : MonoBehaviour
    {
        public EnemyType[] m_enemies;
        public float m_spawnHeight = -100.0f;

        public void Spawn(int spawnScore, Action<GameObject> f)
        {
            var wagons = Gameplay.Caravan.Wagons;

            while (spawnScore > 0)
            {
                var possible = m_enemies.Where(e => e.m_cost <= spawnScore).ToList();

                if (possible.Count == 0)
                {
                    // Nothing left to spawn
                    return;
                }

                var spawn = possible[Random.Range(0, possible.Count - 1)];
                spawnScore -= spawn.m_cost;

                var wagon = wagons[Random.Range(0, wagons.Count - 1)];
                var created = Instantiate(spawn.m_prefab, new Vector3(wagon.transform.position.x, m_spawnHeight, wagon.transform.position.z), Quaternion.identity);

                Debug.Log($"Spawned enemy of type {created.name}");

                f(created);
            }
        }
    }
}
