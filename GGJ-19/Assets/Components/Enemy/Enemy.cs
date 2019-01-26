using System.Collections;
using System.Collections.Generic;
using Assets.Gameplay.Spawning;
using UnityEngine;

public class Enemy : Character
{
    public EnemyType m_enemyType;

    private void Start()
    {
        this.hitPoints = m_enemyType.m_hitpoints;
    }

    public override void EventDied()
    {
        Debug.Log("Enemy Died!");
        Destroy(this);
    }
}
