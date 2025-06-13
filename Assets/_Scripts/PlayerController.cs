using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerStamina))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
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
    
    [Header("Dodge")]
    [SerializeField] private float dodgeForce;
    [SerializeField] private float dodgeJumpForce;
    private bool isDodging;
    private bool canDodge;
    private Vector3 dodgeDirection;

    [Header("Mud")]
    [SerializeField] private float mudSpeed;
    private bool isOnMud;
    public bool IsOnMud { get => isOnMud; set => isOnMud = value; }

    [Header("Ice")]
    [SerializeField] private float iceForce;
    [SerializeField] private float iceBrakeFactor;
    Vector3 lastMoveDirection;
    private bool isOnIce;
    private bool isSlidingOnIce;
    public bool IsOnIce { get => isOnIce; set => isOnIce = value; }
    public bool IsSlidingOnIce { get => isSlidingOnIce; set => IsSlidingOnIce = value; }

    [Header("Dependencies")]
    [SerializeField] private Transform _camera;
    private PlayerStamina _playerStamina;

    [Header("Inputs")]
    private float horizontal;
    private float vertical;
    private bool constrainDirections;
    private bool playerQuiet;
    private bool isSprinting;

    public bool IsGrounded { get => isGrounded; }
    public bool PlayerQuiet { get => playerQuiet; }
    public bool IsSprinting { get => isSprinting; }
    public bool ConstrainDirections { get => constrainDirections; }


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _playerStamina = GetComponent<PlayerStamina>();

        colliderCenter = _collider.center;
        colliderHeight = _collider.height;
        cameraPosition = _camera.transform.localPosition;
    }

    void Start()
    {
        canDodge = true;
        
        originalColliderCenter = colliderCenter;
        originalColliderHeight = colliderHeight;
        originalCameraPosition = cameraPosition;
    }


    void Update()
    {
        bool inputJump = Input.GetKeyDown(KeyCode.Space);

        if (isOnMud || isOnIce)
        {
            isSprinting = false;
            _playerStamina.CanSprint = false;
        }

        _playerStamina.SetSprintState(isSprinting);

        if (isGrounded && inputJump && !isCrouched && !isOnMud)
        {
            isJumping = true;
        }

        if (!isOnMud)
        {
            PlayerCrouch();
        }

        bool leftInput = Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftAlt);
        bool rightInput = Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftAlt);
        bool backInput = Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftAlt);

        if (leftInput && isGrounded && !isJumping && !isCrouched)
        {
            isDodging = true;
            dodgeDirection = -transform.right;
        }
        else if (rightInput && isGrounded && !isJumping && !isCrouched)
        {
            isDodging = true;
            dodgeDirection = transform.right;
        }
        else if (backInput && isGrounded && !isJumping && !isCrouched)
        {
            isDodging = true;
            dodgeDirection = -transform.forward;
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        PlayerMovement();

        bool checkJumpCost = _playerStamina.CurrentStamina >= _playerStamina.JumpStaminaCost;
        if (isJumping && checkJumpCost)
        {
            PlayerJump();
            _playerStamina.JumpStamina();
            isJumping = false;
        }

        bool checkDodgeCost = _playerStamina.CurrentStamina >= _playerStamina.DodgeStaminaCost;
        if (isGrounded && isDodging && canDodge && !isOnMud && !isOnIce && !isSprinting && checkDodgeCost)
        {
            PlayerDodge();
            _playerStamina.DodgeStamina();
        }
    }

    /// <summary>
    /// Handles all player movement logic: walking, sprinting (only if allowed),
    /// crouching, mud slowdown, and ice sliding. Automatically adjusts current speed
    /// based on terrain conditions and input state.
    /// </summary>
    void PlayerMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        constrainDirections = vertical < 0 || horizontal != 0;
        playerQuiet = horizontal == 0 && vertical == 0;

        float yVelocity = _rigidbody.velocity.y;
        float modifier = 0.5f;

        Vector3 movement = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (movement != Vector3.zero)
        {
            lastMoveDirection = movement;
        }

        if (!playerQuiet)
        {
            if (isOnMud)
            {
                currentSpeed = mudSpeed;
            }
            else if (!constrainDirections && !isSprinting && !isCrouched)
            {
                currentSpeed = speed;
            }
            else if (!constrainDirections && _playerStamina.SprintActive && !isCrouched && _playerStamina.CanSprint) //&& !isOnMud)
            {
                currentSpeed = sprintSpeed;
            }
            else if (!constrainDirections && !_playerStamina.CanSprint)
            {
                currentSpeed = speed;
            }
            else if (constrainDirections && !isSprinting && !isCrouched)
            {
                currentSpeed = speed * modifier;
            }
            else if (constrainDirections && _playerStamina.SprintActive && !isCrouched && _playerStamina.CanSprint) //&& !isOnMud )
            {
                currentSpeed = sprintSpeed * modifier;
            }
            else if (constrainDirections && !_playerStamina.CanSprint)
            {
                currentSpeed = speed * modifier;
            }
            else if (!constrainDirections && isCrouched)
            {
                currentSpeed = crouchSpeed;
            }
            else if (constrainDirections && isCrouched)
            {
                currentSpeed = crouchSpeed * modifier;
            }
        }
        else
        {
            currentSpeed = 0f;
        }

        if (isGrounded && !isOnIce)
        {
            _rigidbody.velocity = new Vector3(movement.x * currentSpeed, yVelocity, movement.z * currentSpeed);
            Debug.Log($"Current Speed: {currentSpeed}");
            isSlidingOnIce = false;
        }
        else if (lastMoveDirection != Vector3.zero && IsOnIce)
        {
            if (playerQuiet)
            {
                isSlidingOnIce = true;
                lastMoveDirection = Vector3.Lerp(lastMoveDirection, Vector3.zero, iceBrakeFactor);
            }
            
            _rigidbody.AddForce(lastMoveDirection * iceForce, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Makes the player jump by applying upward force.
    /// Jumping is disabled while on mud.
    /// </summary>
    void PlayerJump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Changes the player collider and camera when crouching.
    /// Prevents standing under ceilings. Crouching is disabled while on mud.
    /// </summary>
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

    /// <summary>
    /// Applies an impulse in the dodge direction and starts a cooldown.
    /// Dodging is disabled while on mud.
    /// </summary>
    void PlayerDodge()
    {
        Vector3 dodgeJump = Vector3.up;

        _rigidbody.AddForce((dodgeDirection * dodgeForce) + (dodgeJump * dodgeJumpForce), ForceMode.Impulse);

        canDodge = false;
        isDodging = false;

        StartCoroutine(TimeBetweenDodges());
    }

    /// <summary>
    /// Uses a Raycast downward to check if the player is touching the ground.
    /// </summary>
    void CheckGround()
    {
        isGrounded = Physics.Raycast(_checkGround.transform.position, -Vector3.up, 0.2f, groundMask);
    }

    /// <summary>
    /// When entering a trigger with tag "Mud" or "IceSurface", sets movement conditions.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mud"))
        {
            isOnMud = true;
        }

        if (other.CompareTag("IceSurface"))
        {
            isOnIce = true;
        }
    }

    /// <summary>
    /// Maintains mud or ice state while inside trigger area.
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mud"))
        {
            isOnMud = true;
        }

        if (other.CompareTag("IceSurface"))
        {
            isOnIce = true;
        }
    }

    /// <summary>
    /// Resets mud or ice condition when exiting the trigger area.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mud"))
        {
            isOnMud = false;
        }

        if (other.CompareTag("IceSurface"))
        {
            isOnIce = false;
        }
    }

    /// <summary>
    /// Waits for a cooldown before allowing the player to dodge again.
    /// </summary>
    IEnumerator TimeBetweenDodges()
    {
        yield return new WaitForSeconds(1.2f);
        canDodge = true;
    }
}
