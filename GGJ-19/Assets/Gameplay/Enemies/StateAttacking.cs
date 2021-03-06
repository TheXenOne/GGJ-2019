﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttacking : State<EnemyAI>
{
    private static StateAttacking instance;

    private StateAttacking() { }

    public static StateAttacking Instance {
        get {
            if (instance == null)
            {
                instance = new StateAttacking();
            }
            return instance;
        }
    }

    public override void Enter(EnemyAI a_entity)
    {
        Debug.Log("Enemy entered StateAttacking.");
        a_entity.characterController.enabled = true;
        a_entity.enemyCapsuleCollider.enabled = true;
        a_entity.useGravity = true;
		//TODO Check if I didn't delete this code unnecessarily
		//a_entity.enemyRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public override void Execute(EnemyAI a_entity)
    {
        Vector3 distToPlayer = EnemyAI.playerObject.transform.position - a_entity.transform.position;
        Color color = Color.green;
		
		Vector3 currentPosition = a_entity.transform.position;
		if ((a_entity.previousPosition - currentPosition).magnitude >= 0.1f)
		{
			a_entity.animator.SetBool("isMovingHorizontally", true);
		}
		else
		{
			a_entity.animator.SetBool("isMovingHorizontally", false);
		}
		a_entity.previousPosition = currentPosition;

        if (a_entity.caravanToAttack.containsPlayer)
        {

            if (distToPlayer.magnitude > a_entity.attackRange)
            {
                Vector3 temp;
                temp.x = distToPlayer.x;
                temp.y = 0.0f;
                temp.z = distToPlayer.z;
                distToPlayer = temp;
                a_entity.velocity += distToPlayer.normalized * a_entity.accelMoveSpeed * Time.deltaTime;

                color = Color.red;
            }
            else
            {
                if (a_entity.attackCooldownCurrent <= 0.01f)
                {
                    a_entity.attackCooldownCurrent = a_entity.attackCooldown;
                    a_entity.enemyCharacter.Attack(EnemyAI.player, 0);
                }
            }

            if (a_entity.attackCooldownCurrent > 0)
            {
                a_entity.attackCooldownCurrent -= Time.deltaTime;
            }

            float angle = Vector3.SignedAngle(a_entity.transform.forward, distToPlayer.normalized, Vector3.up);
            if (Mathf.Abs(angle) > 3)
            {
                a_entity.transform.Rotate(new Vector3(0, angle * a_entity.turnSpeed, 0));
            }
        }

        Debug.DrawRay(a_entity.transform.position, a_entity.transform.TransformDirection(Vector3.forward) * distToPlayer.magnitude, color);
    }

    public override void Exit(EnemyAI a_entity)
    {
    }
}