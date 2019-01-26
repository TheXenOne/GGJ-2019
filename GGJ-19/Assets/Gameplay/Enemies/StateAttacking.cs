using System.Collections;
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
        a_entity.enemyRigidbody.useGravity = true;
        a_entity.enemyRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public override void Execute(EnemyAI a_entity)
    {
        Vector3 distToPlayer = EnemyAI.player.transform.position - a_entity.transform.position;
        Color color = Color.green;

        if (distToPlayer.magnitude > a_entity.attackRange)
        {
            a_entity.enemyRigidbody.AddForce(distToPlayer.normalized * a_entity.accelMoveSpeed * Time.deltaTime);

            float angle = Vector3.SignedAngle(a_entity.transform.forward, distToPlayer.normalized, Vector3.up);
            if (Mathf.Abs(angle) > 3)
            {
                a_entity.transform.Rotate(new Vector3(0, angle * a_entity.turnSpeed, 0));
            }

            color = Color.red;
        }

        Debug.DrawRay(a_entity.transform.position, a_entity.transform.TransformDirection(Vector3.forward) * distToPlayer.magnitude, color);
    }

    public override void Exit(EnemyAI a_entity)
    {
    }
}