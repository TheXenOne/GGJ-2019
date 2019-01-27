using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Components
{
    public class CaravanWagon : MonoBehaviour
    {
        public GameObject m_wagonBase;
        public GameObject[] m_wagonUpgrades;

        public bool containsPlayer;

        public int m_fleetPositionX;
        public int m_fleetPositionZ;

        private List<GameObject> m_activeComponents = new List<GameObject>();

        // Start is called before the first frame update
        void Awake()
        {
            containsPlayer = false;
            m_activeComponents.Add(m_wagonBase);
        }

        public GameObject GetRandomActiveComponent()
        {
            return m_activeComponents[Random.Range(0, m_activeComponents.Count - 1)];
        }
    }
}