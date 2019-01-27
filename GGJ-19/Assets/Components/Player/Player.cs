using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gameplay;
using Assets.Components;
using UnityEngine.UI;

public class Player : Character
{
    public static Player Instance;

	private Slider healthSlider;
    
    new void Awake()
    {
        base.Awake();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen

        Instance = this;
    }

    public void Start()
    {
        healthSlider.maxValue = hitPoints;
        healthSlider.value = hitPoints;
    }

    public void Respawn(GameObject wagonComponent)
    {
        Bounds bounds = wagonComponent.GetComponent<Collider>().bounds;

        var controller = GetComponent<CharacterController>();
        controller.enabled = false;
        transform.localPosition = bounds.center + Vector3.up * (bounds.size.y + 10.0f);
        controller.enabled = true;
        hitPoints = maxHitPoint;
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
