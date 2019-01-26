using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 100.0f;
    public float jumpStrength = 100.0f;
    public float gravity = 10.0f;
    public bool movementEnabled = true;

    private Vector2 axesInput;
    private bool jumpPressed;
    private bool controllerConnected;

    private Vector3 velocity;

    void ProcessInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        jumpPressed = Input.GetAxis("Jump") > 0.0f;
        axesInput = new Vector2(horizontal, vertical);
    }

    void UpdateMovement()
    {
        CharacterController controller = GetComponentInParent<CharacterController>();

        velocity.x = axesInput.x * movementSpeed;
        velocity.z = axesInput.y * movementSpeed;

        if(controller.isGrounded && jumpPressed)
        {
            velocity.y = jumpStrength;
        }

        velocity.y = velocity.y - (gravity * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
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
