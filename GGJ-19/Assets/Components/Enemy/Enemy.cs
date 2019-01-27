using System.Collections;
using System.Collections.Generic;
using Assets.Gameplay.Spawning;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public EnemyType m_enemyType;

    public Slider slider;

    private void Start()
    {
        base.Awake();
        this.hitPoints = m_enemyType.m_hitpoints;
        maxHitPoint = hitPoints;
        slider.value = hitPoints / maxHitPoint;
    }

    private new void Update()
    {
        base.Update();

        slider.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
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
        //Destroy(this);
    }
}
