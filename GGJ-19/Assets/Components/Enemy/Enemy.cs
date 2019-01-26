using System.Collections;
using System.Collections.Generic;
using Assets.Gameplay.Spawning;
using UnityEngine;

public class Enemy : Character
{
    public EnemyType m_enemyType;

    private void Start()
    {
    }

    public override void EventDied()
    {
        Debug.Log("Enemy Died!");
        Destroy(this);
    }
}
