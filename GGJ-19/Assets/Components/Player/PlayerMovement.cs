﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpStrength;
	public float gravity;
	public float dashSpeed;
	public float dashDuration;
    public float dashCooldown;
    public float dashCooldownCurrent;
    public float attackCooldown;
    public float attackCooldownCurrent;
	public float maxFallingSpeed;
	public bool movementEnabled;
	public float knockbackStrength;
	public bool disableGravity;
    public float attackSphereWidth = 1.0f;
    public AudioSource footstepSource;
    public float attackReach;

	//TODO Move these enemy variables to the enemy file!
	bool isKnockedBack;
	Vector2 knockbackDirection;

    private Vector2 axesInput;
    private bool jumpPressed;
	private bool dashPressed;
    private bool primaryAttackPressed;
    private bool secondaryAttackPressed;
    private bool attacking = false;
    private bool controllerConnected;
	private bool isDashing;
	private float timeDashStarted;
	private Camera mainCamera;
	private Animator animator;

	private Vector2 dashDirection;

    private Vector3 velocity;

    void ProcessInput()
    {
		//Changed to raw due to floaty movement (smoothing which was normalized afterwards)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        jumpPressed = Input.GetAxis("Jump") > 0.0f;
		dashPressed = Input.GetAxis("Fire3") > 0.0f;
        primaryAttackPressed = Input.GetAxisRaw("Fire1") > 0.0f;
        secondaryAttackPressed = Input.GetAxisRaw("Fire2") > 0.0f;

        axesInput = new Vector2(horizontal, vertical);
    }

    void UpdateMovement()
    {
        CharacterController controller = GetComponentInParent<CharacterController>();
		if(controller.isGrounded)
		{
			animator.SetBool("isOnGround", true);
		}
		else
		{
			animator.SetBool("isOnGround", false);
		}
		//Direction of the movement is translated by the camera. This is done the simple way, so it's not recognized by the character if it should walk slowly or not.
		if (!isDashing)
		{
			Vector3 transformedAxesInput;
			transformedAxesInput.x = axesInput.x;
			transformedAxesInput.y = 0;
			transformedAxesInput.z = axesInput.y;
			
			if(transformedAxesInput.magnitude>=0.05f)
			{
				animator.SetBool("isMovingHorizontally", true);
			}
			else
			{
				animator.SetBool("isMovingHorizontally", false);
			}
			transformedAxesInput =  mainCamera.transform.rotation* transformedAxesInput;

			axesInput.x = transformedAxesInput.x;
			axesInput.y = transformedAxesInput.z;
			axesInput.Normalize();
			velocity.x = axesInput.x * movementSpeed;
			velocity.z = axesInput.y * movementSpeed;

			if (controller.isGrounded && jumpPressed)
			{
				velocity.y = jumpStrength;
				animator.SetTrigger("isJumping");
			}

			
			if (velocity.y > (maxFallingSpeed - 2 * maxFallingSpeed))
			{
				velocity.y = velocity.y - (gravity * Time.deltaTime);
			}
		}

		if (controller.isGrounded && dashPressed && (!isDashing) && (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f))
		{
            if (dashCooldownCurrent <= 0.1)
            {
                dashCooldownCurrent = dashCooldown;
                // dash direction directly corresponds to the direction of the stick in relation to the camera - not the direction.
                dashDirection.x = Input.GetAxisRaw("Horizontal");
                dashDirection.y = Input.GetAxisRaw("Vertical");
                Vector3 transformedAxesInput;
                transformedAxesInput.x = dashDirection.x;
                transformedAxesInput.y = 0;
                transformedAxesInput.z = dashDirection.y;

                transformedAxesInput = mainCamera.transform.rotation * transformedAxesInput;

                dashDirection.x = transformedAxesInput.x;
                dashDirection.y = transformedAxesInput.z;
                dashDirection.Normalize();

                isDashing = true;
                animator.SetBool("isDashing", true);

                dashDirection.Normalize();
                timeDashStarted = Time.time;
            }
		}

		if(isDashing)
		{
			velocity.x = dashDirection.x* dashSpeed;
			velocity.z = dashDirection.y* dashSpeed;
			velocity.y = 0.0f;


            if ((Time.time - timeDashStarted) > dashDuration)
            {
                isDashing = false;
				animator.SetBool("isDashing", false);
			}
		}

		if(disableGravity)
		{
			velocity.y = 0;
		}

        controller.Move(velocity * Time.deltaTime);

        if (velocity.x != 0.0f || velocity.z != 0.0f)
        {
            controller.transform.rotation = Quaternion.Euler(0.0f, mainCamera.transform.rotation.eulerAngles.y, 0.0f);
        }

        if (attackCooldownCurrent <= 0.1)
        {
            if (primaryAttackPressed && !attacking)
            {
                attackCooldownCurrent = attackCooldown;
                StartCoroutine(PerformAttack(0));
            }

            if (secondaryAttackPressed && !attacking)
            {
                attackCooldownCurrent = attackCooldown;
                StartCoroutine(PerformAttack(1));
            }
        }

        Vector3 transformAxesInput;
        transformAxesInput.x = axesInput.x;
        transformAxesInput.y = 0;
        transformAxesInput.z = axesInput.y;
        if (controller.isGrounded && transformAxesInput.magnitude >= 0.05f)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }

    IEnumerator PerformAttack(int attackID)
    {
        RaycastHit hitInfo;
        Vector3 direction = mainCamera.transform.forward;

        bool hit = Physics.SphereCast(
            transform.position,
            attackSphereWidth,
            mainCamera.transform.forward,
            out hitInfo,
            attackReach
        );

        Character hitEnemy = null;

        attacking = true;
        if (hit)
        {
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                Character charScript = hitInfo.collider.gameObject.GetComponent<Character>();

                hitEnemy = charScript;
                Debug.DrawRay(transform.position, direction * attackReach, Color.green);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, direction * attackReach, Color.red);
        }

        gameObject.GetComponent<Player>().Attack(hitEnemy, attackID);

        float cooldown = Player.Instance.availableAttacks[attackID].Cooldown;

        yield return new WaitForSecondsRealtime(cooldown);
        attacking = false;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		
		//TODO check if it collides with enemy and knock them back
		if (isDashing)
		{
			if (hit.gameObject.tag == "Enemy")
			{
				Vector3 knockbackDirection = hit.transform.position - transform.position;
				knockbackDirection.y = 0.0f;
				knockbackDirection.Normalize();
				hit.gameObject.GetComponent<EnemyAI>().velocity = knockbackStrength * knockbackDirection;
				hit.gameObject.GetComponent<EnemyAI>().isKnockedBack = true;


							velocity.x = 0.0f;
				velocity.y = 0.0f;
				velocity.z = 0.0f;
				isDashing = false;
				animator.SetBool("isDashing", false);
				Physics.IgnoreCollision(GetComponent<CharacterController>(), hit.gameObject.GetComponent<CharacterController>(), true);
				
			}
		}
	}
	
	// Start is called before the first frame update
	void Start()
    {
        if (dashCooldown == 0)
        {
            dashCooldown = 2;
        }
        dashCooldownCurrent = 0;

        if (attackCooldown == 0)
        {
            attackCooldown = 1.3f;
        }
        attackCooldownCurrent = 0;

        controllerConnected = Input.GetJoystickNames().Length > 0;
		mainCamera = Camera.main;
		animator = GetComponentInChildren<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if (dashCooldownCurrent > 0)
        {
            dashCooldownCurrent -= Time.deltaTime;
        }

        if (attackCooldownCurrent > 0)
        {
            attackCooldownCurrent -= Time.deltaTime;
        }

        ProcessInput();
        if (movementEnabled)
        {
            UpdateMovement();
        }
    }
}
