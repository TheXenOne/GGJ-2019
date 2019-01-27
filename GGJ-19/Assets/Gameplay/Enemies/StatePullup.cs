using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePullup : State<EnemyAI>
{
    private static StatePullup instance;

    private StatePullup() { }

    public static StatePullup Instance {
        get {
            if (instance == null)
            {
                instance = new StatePullup();
            }
            return instance;
        }
    }

    public override void Enter(EnemyAI a_entity)
    {
        Debug.Log("Enemy entered StatePullup.");
        a_entity.characterController.enabled = true;
        a_entity.enemyCapsuleCollider.enabled = true;
    }

    public override void Execute(EnemyAI a_entity)
    {
        Vector3 standPoint = a_entity.headPoint.transform.position + (a_entity.transform.forward * a_entity.enemyCapsuleCollider.radius * 4) + new Vector3(0, (a_entity.headPoint.transform.position.y - a_entity.climbPoint.transform.position.y), 0);
        a_entity.transform.position = standPoint;
        a_entity.enemyStateMachine.ChangeState(StateAttacking.Instance);
    }

    public override void Exit(EnemyAI a_entity)
    {
    }
}