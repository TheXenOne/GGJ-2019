using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject caravanToAttack;
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
    public StateMachine<EnemyAI> enemyStateMachine;

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
    static public GameObject player;

    [HideInInspector]
    public Enemy enemyComponent;

    void Awake()
    {
		isKnockedBack = false;
        enemyCapsuleCollider = GetComponent<CapsuleCollider>();
		characterController = GetComponent<CharacterController>();

        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
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
    }
}