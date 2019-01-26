using System.Collections;
using System.Collections.Generic;
using Assets.Gameplay.Spawning;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public EnemyType m_enemyType;

    public Slider slider;
    public GameObject camera;

    private void Start()
    {
        this.hitPoints = m_enemyType.m_hitpoints;
        maxHitPoint = hitPoints;
        slider.value = hitPoints / maxHitPoint;
    }

    private void Update()
    {
        slider.transform.rotation = Quaternion.LookRotation(camera.transform.position - transform.position);
    }

    public override void TakeDamage(float damage)
    {
        hitPoints -= damage;

        slider.value = hitPoints / maxHitPoint;

        if (hitPoints <= 0.0f)
        {
            EventDied();
        }
    }

    public override void EventDied()
    {
        Debug.Log("Enemy Died!");
        Destroy(this);
    }
}
