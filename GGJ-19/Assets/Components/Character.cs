using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxHitPoint;
    public float hitPoints;
    public AttackType[] availableAttacks;

    public abstract void EventDied();

    public virtual void TakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0.0f)
        {
            EventDied();
        }
    }

    public void Attack(Character other, int attackID)
    {
        var attack = availableAttacks[attackID];

        other.TakeDamage(attack.Damage);
        // TODO: Animation code here
    }
}
