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
	public int maxHealth;
	public float knockbackStrength;
	public bool disableGravity;

	//TODO Move these enemy variables to the enemy file!
	bool isKnockedBack;
	Vector2 knockbackDirection;

    private Vector2 axesInput;
    private bool jumpPressed;
	private bool dashPressed;
    private bool controllerConnected;
	private bool isDashing;
	private float timeDashStarted;
	private int currentHealth;

	private Vector2 dashDirection;

    private Vector3 velocity;

    void ProcessInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        jumpPressed = Input.GetAxis("Jump") > 0.0f;
		dashPressed = Input.GetAxis("Fire1") > 0.0f;
        axesInput = new Vector2(horizontal, vertical);
    }

    void UpdateMovement()
    {
        CharacterController controller = GetComponentInParent<CharacterController>();

		//TODO the direction of the movement should be translated by the camera
		if (!isDashing)
		{
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
			//TODO dash direction directly corresponds to the direction of the stick in relation to the camera - not the direction.
			isDashing = true;
			dashDirection.x = Input.GetAxisRaw("Horizontal");
			dashDirection.y = Input.GetAxisRaw("Vertical");
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
    }


	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		
		//TODO check if it collides with enemy and knock them back
		if (isDashing)
		{
			int a;
			a = 7;
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
