using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gameplay;
using Assets.Components;
using UnityEngine.UI;

public class Player : Character
{
    public Gameplay gameplay;
    public static Player Instance;

	private Slider healthSlider;
    
    void Awake()
    {
        Instance = this;
		healthSlider.maxValue = hitPoints;
		healthSlider.value = hitPoints;
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

    public override void EventDied()
    {
        Debug.Log("Player died! Respawning...");
        RespawnRandom();
        
    }

	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
		healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
		healthSlider.value = hitPoints;
	}
}
