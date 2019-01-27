using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaravanPlayerTrigger : MonoBehaviour
{
    private Assets.Components.CaravanWagon caravan;

    private void Awake()
    {
        caravan = GetComponentInParent<Assets.Components.CaravanWagon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (caravan != null)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                caravan.containsPlayer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (caravan != null)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                caravan.containsPlayer = false;
            }
        }
    }
}
