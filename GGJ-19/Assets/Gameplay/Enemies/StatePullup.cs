using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePullup : State<EnemyAI>
{
    private static StatePullup instance;

    public float pullupTimeMax = 2;
    private float pullupTimeCurrent;
    private float pullupMoveSpeed = 3.5f;

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
        pullupTimeCurrent = pullupTimeMax;
        //Vector3 standPoint = a_entity.headPoint.transform.position + (a_entity.transform.forward * a_entity.enemyCapsuleCollider.radius * 4) + new Vector3(0, (a_entity.headPoint.transform.position.y - a_entity.climbPoint.transform.position.y), 0);
        //a_entity.transform.position = standPoint;
    }

    public override void Execute(EnemyAI a_entity)
    {
        pullupTimeCurrent -= Time.deltaTime;
        if (pullupTimeCurrent > 0)
        {
            a_entity.transform.position += a_entity.transform.forward * pullupMoveSpeed * Time.deltaTime;
        }
        else
        {
            a_entity.enemyStateMachine.ChangeState(StateAttacking.Instance);
        }
    }

    public override void Exit(EnemyAI a_entity)
    {
        Vector3 playerPos = EnemyAI.playerObject.transform.position;
        Vector3 enemyPos = a_entity.transform.position;
        enemyPos.y = playerPos.y;
        a_entity.transform.rotation = Quaternion.LookRotation(-(enemyPos - playerPos).normalized, Vector3.up);
    }
}