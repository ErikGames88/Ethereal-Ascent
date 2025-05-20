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
    [SerializeField] private bool isGrounded;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    private bool isJumping;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeed;
    private Vector3 colliderCenter;
    private float colliderHeight;
    private bool isCrouching;
    private CapsuleCollider _collider;
    [SerializeField] private Transform _camera;
    private Vector3 originalCameraPosition = new Vector3(0f, 1.8f, 0f);


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        colliderCenter = _collider.center;
        colliderHeight = _collider.height;
        _camera.transform.localPosition = originalCameraPosition;
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

        PlayerCrouch();
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

        float yVelocity = _rigidbody.velocity.y;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool constrainDirections = vertical < 0 || horizontal != 0;
        bool playerQuiet = horizontal == 0 && vertical == 0;
        float modifier = 0.5f;

        Vector3 movement = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (!playerQuiet)
        {
            if (!constrainDirections && !isSprinting)
            {
                currentSpeed = speed;
            }
            else if (!constrainDirections && isSprinting)
            {
                currentSpeed = sprintSpeed;
            }
            else if (constrainDirections && !isSprinting)
            {
                currentSpeed = speed * modifier;
            }
            else
            {
                currentSpeed = sprintSpeed * modifier;
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

    void PlayerCrouch()
    {
        float crouchColliderHeight = 1f;
        bool crouchInput = Input.GetKey(KeyCode.LeftControl);
        Vector3 crouchCameraPosition = new Vector3(0f, 0.6f, 0f);

        if (crouchInput)
        {
            isCrouching = true;

            _collider.center = new Vector3(0f, 0.6f, 0f);
            _collider.height = crouchColliderHeight;
            _camera.transform.localPosition = crouchCameraPosition;

        }
        else
        {
            isCrouching = false;

            _collider.center = colliderCenter;
            _collider.height = colliderHeight;
            _camera.transform.localPosition = originalCameraPosition;
        }
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
