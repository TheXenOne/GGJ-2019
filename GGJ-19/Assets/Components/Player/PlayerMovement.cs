using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 100.0f;
    public float jumpStrength = 100.0f;

    void ProcessInput()
    {
    }

    void ResolveInput()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        ResolveInput();
    }
}
