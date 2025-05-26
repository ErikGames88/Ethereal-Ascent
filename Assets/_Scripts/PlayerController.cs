using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float horizontal;
    private float vertical;
    private float currentSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] GameObject _checkGround;
    private Rigidbody _rigidbody;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundMask;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    private bool isJumping;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeed;
    private Vector3 colliderCenter;
    private Vector3 cameraPosition;
    private float colliderHeight;
    private bool isCrouched;
    private Vector3 originalColliderCenter;
    private float originalColliderHeight;
    private Vector3 originalCameraPosition;
    private CapsuleCollider _collider;
    [SerializeField] private Transform _camera;

    [Header("Dodge")]
    [SerializeField] private float dodgeForce;
    private bool isDodging;
    private bool canDodge;
    private Vector3 dodgeDirection;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        colliderCenter = _collider.center;
        colliderHeight = _collider.height;
        cameraPosition = _camera.transform.localPosition;
    }

    void Start()
    {
        originalColliderCenter = colliderCenter;
        originalColliderHeight = colliderHeight;
        originalCameraPosition = cameraPosition;
    }


    void Update()
    {
        bool inputJump = Input.GetKeyDown(KeyCode.Space);

        if (isGrounded && inputJump && !isCrouched)
        {
            isJumping = true;
        }

        PlayerCrouch();

        bool leftInput = Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftAlt);
        bool rightInput = Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftAlt);
        bool backInput = Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftAlt);

        if (isGrounded && !isJumping && !isCrouched && leftInput)
        {
            isDodging = true;
            dodgeDirection = -transform.right;
        }
        else if (isGrounded && !isJumping && !isCrouched && rightInput)
        {
            isDodging = true;
            dodgeDirection = transform.right;
        }
        else if (isGrounded && !isJumping && !isCrouched && backInput)
        {
            isDodging = true;
            dodgeDirection = -transform.forward;
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        PlayerMovement();

        if (isJumping)
        {
            PlayerJump();
            isJumping = false;
        }

        if (isDodging)
        {
            PlayerDodge();
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

            if (!constrainDirections && !isSprinting && !isCrouched)
            {
                currentSpeed = speed;
            }
            else if (!constrainDirections && isSprinting && !isCrouched)
            {
                currentSpeed = sprintSpeed;
            }
            else if (constrainDirections && !isSprinting && !isCrouched)
            {
                currentSpeed = speed * modifier;
            }
            else if (constrainDirections && isSprinting && !isCrouched)
            {
                currentSpeed = sprintSpeed * modifier;
            }
            else if (!constrainDirections && isCrouched)
            {
                currentSpeed = crouchSpeed;
            }
            else
            {
                currentSpeed = crouchSpeed * modifier;
            }
        
        }
        else
        {
            currentSpeed = 0f;
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
        Vector3 crouchColliderCenter = new Vector3(0f, 0.6f, 0f);
        float crouchColliderHeight = 1f;
        Vector3 crouchCameraPosition = new Vector3(0f, 0.6f, 0f);

        Vector3 globalColliderCenter = transform.position + _collider.center;
        float halfHeight = _collider.height / 2 - _collider.radius;

        Vector3 lowPoint = globalColliderCenter - Vector3.up * halfHeight;
        Vector3 topPoint = globalColliderCenter + Vector3.up * halfHeight;

        bool crouchCeiling = Physics.CapsuleCast(lowPoint, topPoint, _collider.radius, Vector3.up, 0.5f,
        LayerMask.GetMask("CrouchCeiling"));

        bool crouchInput = Input.GetKey(KeyCode.LeftControl);

        if (isGrounded && crouchInput)
        {
            isCrouched = true;
            isJumping = false;

            _collider.center = crouchColliderCenter;
            _collider.height = crouchColliderHeight;
            _camera.transform.localPosition = crouchCameraPosition;
        }

        if (!crouchInput && isGrounded && isCrouched && !crouchCeiling)
        {
            isCrouched = false;

            _collider.center = originalColliderCenter;
            _collider.height = originalColliderHeight;
            _camera.transform.localPosition = originalCameraPosition;

        }
    }

    void PlayerDodge()
    {
        _rigidbody.AddForce(dodgeDirection * dodgeForce, ForceMode.Impulse);
        isDodging = false;
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(_checkGround.transform.position, -Vector3.up, 0.2f, groundMask);

        Debug.DrawRay(_checkGround.transform.position, -Vector3.up * 0.5f, Color.red);
    }
    
}
