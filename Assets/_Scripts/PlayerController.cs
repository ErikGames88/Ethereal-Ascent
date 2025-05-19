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

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    bool isJumping;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        bool inputJump = Input.GetKeyDown(KeyCode.Space);

        if (isGrounded && inputJump)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }
    void FixedUpdate()
    {
        PlayerMovement();

        if (isJumping)
        {
            PlayerJump();
        }
    }

    void PlayerMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float yVelocity = _rigidbody.velocity.y;
        bool constrainDirections = vertical < 0 || horizontal != 0;
        bool playerQuiet = horizontal == 0 && vertical == 0;
        float modifier = 0.5f;

        Vector3 movement = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (!playerQuiet)
        {
            if (constrainDirections)
            {
                currentSpeed = currentSpeed * modifier;
            }
            else
            {
                if (!isSprinting)
                {
                    currentSpeed = speed;
                }
                else
                {
                    currentSpeed = sprintSpeed;
                }
                // if (!isSprinting)
                // {
                //     currentSpeed = speed * modifier;
                // }
                // else
                // {
                //     currentSpeed = sprintSpeed * modifier;
                // }
                
            }
        }

        if (isGrounded)
        {
            _rigidbody.velocity = new Vector3(movement.x * currentSpeed, yVelocity, movement.z * currentSpeed);
            Debug.Log($"Current Speed: {currentSpeed}");
        }

    }

    void PlayerJump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

}
