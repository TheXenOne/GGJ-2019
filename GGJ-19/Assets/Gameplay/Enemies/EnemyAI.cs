using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public StateMachine<EnemyAI> enemyStateMachine;

    void Awake()
    {
        enemyStateMachine = new StateMachine<EnemyAI>(this, StateClimb.Instance);
    }

    void Update()
    {
        enemyStateMachine.UpdateStateMachine();
    }
}
