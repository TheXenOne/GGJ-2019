using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject caravanToAttack;
    public GameObject climbPoint;
    public GameObject headPoint;
    public float climbSpeed;
    public float maxClimbDistance;
    public float accelMoveSpeed;
    public float moveSpeedMax;
    public float attackRange;
    public float turnSpeed;
    public StateMachine<EnemyAI> enemyStateMachine;

    [HideInInspector]
    public Rigidbody enemyRigidbody;
    [HideInInspector]
    public CapsuleCollider enemyCapsuleCollider;
    [HideInInspector]
    public bool isClimbing;

    [HideInInspector]
    static public GameObject player;

    void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyCapsuleCollider = GetComponent<CapsuleCollider>();

        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        enemyStateMachine = new StateMachine<EnemyAI>(this, StateClimb.Instance);
    }

    void Update()
    {
        enemyStateMachine.UpdateStateMachine();
    }
}