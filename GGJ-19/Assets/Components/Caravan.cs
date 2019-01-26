using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Assets.Components
{
    public class Hex
    {
        public int positionX;
        public int positionY;
        public GameObject wagon;
    }

    public class Caravan : MonoBehaviour
    {
        public int m_wagons = 0;
        public int m_people = 0;
        public int m_currency = 0;
        public float m_fleetScaleX = 16.0f;
        public float m_fleetScaleZ = 16.0f;
        public GameObject m_wagon;

        List<GameObject> m_wagonObjects = new List<GameObject>();
        List<Hex> m_hexes = new List<Hex>();

        public void Awake()
        {
            Debug.Log("Initial caravan creation");

            // Caravan is being created, spawn initial wagons
            while (m_wagonObjects.Count < m_wagons)
            {
                AddWagon();
            }
        }

        public void Update()
        {
            
        }

        public Hex GetHex(int positionX, int positionY)
        {
            return m_hexes.FirstOrDefault(h => h.positionX == positionX && h.positionY == positionY);
        }

        public Hex AssignHex(int positionX, int positionY, GameObject wagon)
        {
            var hex = GetHex(positionX, positionY);
            Debug.Assert(hex == null);

            hex = new Hex()
            {
                positionX = positionX,
                positionY = positionY,
                wagon = wagon
            };

            m_hexes.Add(hex);

            return hex;
        }

        public GameObject GetWagon(int positionX, int positionY)
        {
            return GetHex(positionX, positionY)?.wagon;
        }

        public GameObject GetRandomWagon()
        {
            if(m_wagonObjects.Count == 0)
            {
                return null;
            }

            return m_wagonObjects[Random.Range(0, m_wagonObjects.Count - 1)];
        }

        public void AddWagon()
        {
            Debug.Log("Adding new wagon");

            var newWagon = GameObject.Instantiate(m_wagon);
            newWagon.transform.SetParent(transform);
            m_wagonObjects.Add(newWagon);

            AssignToEmptySpot(newWagon);

            foreach (var wagonObject in m_wagonObjects)
            {
                var wagon = wagonObject.GetComponent<CaravanWagon>();
            }
        }

        public void AssignToEmptySpot(GameObject wagonObject)
        {
            // Find empty hexagon closest to the center 0,0 hexagon
            // This is a really dumb loop that checks the same wagons multiple times and could do with some randomization
            for (int distance = 0; ; distance++)
            {
                for (int x = -distance; x <= distance; x++)
                {
                    for (int y = -distance; y <= distance; y++)
                    {
                        var occupier = GetWagon(x, y);

                        if (occupier == null)
                        {
                            // Found an empty spot, occupy it
                            SetWagonPosition(wagonObject, x, y);

                            Debug.Log($"Added new wagon at position {x};{y}");

                            return;
                        }
                    }
                }
            }
        }

        public void SetWagonPosition(GameObject wagonObject, int positionX, int positionZ)
        {
            AssignHex(positionX, positionZ, wagonObject);

            var wagon = wagonObject.GetComponent<CaravanWagon>();
            wagon.m_fleetPositionX = positionX;
            wagon.m_fleetPositionZ = positionZ;

            // Hexagons, so every odd X needs to have shift 0.5Z up
            wagonObject.transform.SetPositionAndRotation(new Vector3(positionX * m_fleetScaleX, 0.0f,
                positionZ * m_fleetScaleZ - (positionX % 2) * m_fleetScaleZ * 0.5f), Quaternion.identity);
        }
    }
}
