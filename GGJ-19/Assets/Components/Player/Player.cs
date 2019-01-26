using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gameplay;
using Assets.Components;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    void Awake()
    {
        Instance = this;
    }

    public void Respawn(GameObject wagonComponent)
    {
        Bounds bounds = wagonComponent.GetComponent<Collider>().bounds;

        transform.position = bounds.center + Vector3.up * bounds.size.y;
    }

    public void RespawnRandom()
    {
        var wagon = Gameplay.Caravan.GetRandomWagon();

        Respawn(wagon.GetComponent<CaravanWagon>().GetRandomActiveComponent());
    }
}
