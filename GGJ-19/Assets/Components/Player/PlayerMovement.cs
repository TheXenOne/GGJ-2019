using System.Collections;
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
	public float maxFallingSpeed;
	public bool movementEnabled;
	public float knockbackStrength;
	public bool disableGravity;
    public float attackSphereWidth = 1.0f;

    public float attackReach;

	//TODO Move these enemy variables to the enemy file!
	bool isKnockedBack;
	Vector2 knockbackDirection;

    private Vector2 axesInput;
    private bool jumpPressed;
	private bool dashPressed;
    private bool primaryAttackPressed;
    private bool secondaryAttackPressed;
    private bool controllerConnected;
	private bool isDashing;
	private float timeDashStarted;
	private Camera mainCamera;

	private Vector2 dashDirection;

    private Vector3 velocity;

    void ProcessInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        jumpPressed = Input.GetAxis("Jump") > 0.0f;
		dashPressed = Input.GetAxis("Fire3") > 0.0f;
        primaryAttackPressed = Input.GetAxisRaw("Fire1") > 0.0f;
        secondaryAttackPressed = Input.GetAxisRaw("Fire2") > 0.0f;

        axesInput = new Vector2(horizontal, vertical);
    }

    void UpdateMovement()
    {
        CharacterController controller = GetComponentInParent<CharacterController>();

		//Direction of the movement is translated by the camera. This is done the simple way, so it's not recognized by the character if it should walk slowly or not.
		if (!isDashing)
		{
			Vector3 transformedAxesInput;
			transformedAxesInput.x = axesInput.x;
			transformedAxesInput.y = 0;
			transformedAxesInput.z = axesInput.y;
			
			transformedAxesInput =  mainCamera.transform.rotation* transformedAxesInput;

			axesInput.x = transformedAxesInput.x;
			axesInput.y = transformedAxesInput.z;
			axesInput.Normalize();
			velocity.x = axesInput.x * movementSpeed;
			velocity.z = axesInput.y * movementSpeed;

			if (controller.isGrounded && jumpPressed)
			{
				velocity.y = jumpStrength;
			}

			
			if (velocity.y > (maxFallingSpeed - 2 * maxFallingSpeed))
			{
				velocity.y = velocity.y - (gravity * Time.deltaTime);
			}
		}

		if (controller.isGrounded && dashPressed && (!isDashing) && (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f))
		{
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
			
			dashDirection.Normalize();
			timeDashStarted = Time.time;
		}

		if(isDashing)
		{
			velocity.x = dashDirection.x* dashSpeed;
			velocity.z = dashDirection.y* dashSpeed;
			velocity.y = 0.0f;


            if ((Time.time - timeDashStarted) > dashDuration)
            {
                isDashing = false;
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

        if(primaryAttackPressed)
        {
            PerformAttack(0);
        }

        if (secondaryAttackPressed)
        {
            PerformAttack(1);
        }
    }

    void PerformAttack(int attackID)
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
				Physics.IgnoreCollision(GetComponent<CharacterController>(), hit.gameObject.GetComponent<CharacterController>(), true);
				
			}
		}
	}
	
	// Start is called before the first frame update
	void Start()
    {
        controllerConnected = Input.GetJoystickNames().Length > 0;
		mainCamera = Camera.main;
	}

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        if (movementEnabled)
        {
            UpdateMovement();
        }
    }
}
