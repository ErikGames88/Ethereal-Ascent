using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float horizontal;
    private float vertical;
    private float currentSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    private Rigidbody _rigidbody;
    bool isGrounded;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float yVelocity = _rigidbody.velocity.y;
        Vector3 movement = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (!isSprinting)
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = sprintSpeed;
        }

        if (isGrounded)
        {
            _rigidbody.velocity = new Vector3(movement.x * currentSpeed, yVelocity, movement.z * currentSpeed);
        }

        

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
