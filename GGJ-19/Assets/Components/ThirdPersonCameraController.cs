using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public float sensitivityX = 4.0f;
    public float sensitivityY = 1.0f;
    public float minY = 0.0f;
    public float maxY = 50.0f;
    public float distance = 10.0f;
    //public bool invertRotationControl = false;

    public Transform lookAt;

    private Vector2 mouseInput = Vector2.zero;
    
    void Update()
    {
        mouseInput.x += Input.GetAxis("Mouse X") * sensitivityX;
        mouseInput.y += Input.GetAxis("Mouse Y") * sensitivityY;
        mouseInput.y = Mathf.Clamp(mouseInput.y, minY, maxY);       
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0.0f, 0.0f, -distance);
        Quaternion rot = Quaternion.Euler(mouseInput.y, mouseInput.x, 0.0f);

        transform.position = lookAt.position + rot * dir;
        transform.LookAt(lookAt.position);
    }
}
