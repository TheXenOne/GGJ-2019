﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Assets.Components.CaravanWagon caravanToAttack;
    public GameObject climbPoint;
    public GameObject headPoint;
    public GameObject feetPoint;
    public AudioSource footstepSource;
    public float climbSpeed;
    public float maxClimbDistance;
    public float accelMoveSpeed;
    public float moveSpeedMax;
    public float attackRange;
    public float turnSpeed;
    public float attackCooldown;
    public StateMachine<EnemyAI> enemyStateMachine;

	[HideInInspector]
	public Animator animator;

	[HideInInspector]
	public Vector3 previousPosition;

	public float deceleration;
	public float gravity;
	public float maxFallingSpeed;
	[HideInInspector]
	public bool isKnockedBack;
	[HideInInspector]
	public bool useGravity;
	[HideInInspector]
	public Vector3 velocity;
    [HideInInspector]
    public CharacterController characterController;
    [HideInInspector]
    public CapsuleCollider enemyCapsuleCollider;
    [HideInInspector]
    public bool isClimbing;
    [HideInInspector]
    public float attackCooldownCurrent;
    [HideInInspector]
    public Enemy enemyCharacter;

    [HideInInspector]
    static public GameObject playerObject;
    [HideInInspector]
    static public Player player;

    [HideInInspector]
    public Enemy enemyComponent;

    void Awake()
    {
        if (attackCooldown == 0)
        {
            attackCooldown = 1.5f;
        }

		animator = GetComponentInChildren<Animator>();
		previousPosition = transform.position;
        attackCooldownCurrent = 0;

        enemyCharacter = GetComponent<Enemy>();

		isKnockedBack = false;
        enemyCapsuleCollider = GetComponent<CapsuleCollider>();
		characterController = GetComponent<CharacterController>();

        if (playerObject == null)
        {
            playerObject = FindObjectOfType<PlayerMovement>().gameObject;
            if (playerObject != null)
            {
                player = playerObject.GetComponent<Player>();
            }
        }

        enemyStateMachine = new StateMachine<EnemyAI>(this, StateClimb.Instance);
    }

    void Update()
    {
        enemyStateMachine.UpdateStateMachine();

		//Move 
		if (isKnockedBack)
		{
			Vector2 temp;
			temp.x = velocity.x;
			temp.y = velocity.z;
			if (temp.magnitude > moveSpeedMax*10)
			{
				temp.Normalize();
				temp = temp * moveSpeedMax*10;
				velocity.x = temp.x;
				velocity.z = temp.y;
			}

			if ((deceleration * Time.deltaTime) < temp.magnitude)
			{
				if (temp.x > 0.0f)
				{
					temp.x -= deceleration * Time.deltaTime;
				}
				else
				{
					temp.x += deceleration * Time.deltaTime;
				}

				if (temp.y > 0.0f)
				{
					temp.y -= deceleration * Time.deltaTime;
				}
				else
				{
					temp.y += deceleration * Time.deltaTime;
				}
			}
			else
			{
				temp.x = 0.0f;
				temp.y = 0.0f;
				isKnockedBack = false;
			}
			velocity.x = temp.x;
			velocity.z = temp.y;
			characterController.Move(velocity * Time.deltaTime);
		}
			else if (useGravity)
		{
			if (velocity.y > (maxFallingSpeed - (2 * maxFallingSpeed)))
			{
				velocity.y -= gravity * Time.deltaTime;
			}
			Vector2 temp;
			temp.x = velocity.x;
			temp.y = velocity.z;
			if (temp.magnitude > moveSpeedMax)
			{
				temp.Normalize();
				temp = temp * moveSpeedMax;
				velocity.x = temp.x;
				velocity.z = temp.y;
			}

			if ((deceleration * Time.deltaTime) < temp.magnitude)
			{
				if (temp.x > 0.0f)
				{
					temp.x -= deceleration * Time.deltaTime;
				}
				else
				{
					temp.x += deceleration * Time.deltaTime;
				}

				if (temp.y > 0.0f)
				{
					temp.y -= deceleration * Time.deltaTime;
				}
				else
				{
					temp.y += deceleration * Time.deltaTime;
				}
			}
			else
			{
				temp.x = 0.0f;
				temp.y = 0.0f;
			}

			velocity.x = temp.x;
			velocity.z = temp.y;
            characterController.Move(velocity * Time.deltaTime);

            Vector3 horVel = velocity;
            horVel.y = 0;

            if (horVel.magnitude > 2 && !isClimbing)
            {
                if (!footstepSource.isPlaying)
                {
                    //footstepSource.Play();
                }
            }
            else
            {
                //footstepSource.Stop();
            }
        }
		if(GetComponent<CharacterController>().isGrounded)
		{
			animator.SetBool("isOnGround", true);
		}
		else
		{
			animator.SetBool("isOnGround", false);
		}
    }
}