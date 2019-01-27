using Assets.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxHitPoint;
    public float hitPoints;
    public AttackType[] availableAttacks;

	private Animator animator;

    protected void Awake()
    {
        hitPoints = maxHitPoint;
		animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if (transform.position.y < Gameplay.Instance.m_bottom)
        {
            TakeDamage(1000);
        }
    }

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
        if (other)
        {
            var attack = availableAttacks[attackID];

            other.TakeDamage(attack.Damage);
        }
		// TODO: Animation code here
		animator.SetInteger("attackId", attackID);
		animator.SetTrigger("isAttacking");

        Debug.Log("Attacking: Attack ID " + attackID.ToString(), this);
    }
}
