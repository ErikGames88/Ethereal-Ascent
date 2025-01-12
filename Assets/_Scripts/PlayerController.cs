using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField,] private float speed;
    [SerializeField] private float sprint;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float maxHeight = 4f;
    [SerializeField] private float maxSprintTime = 2f;
    [SerializeField] private float sprintRechargeRate = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private Transform startPoint;

    private Rigidbody _rigidbody;
    private float currentSprintTime = 0;
    private bool canSprint = true;
    private bool isSprintAllowed = true;
    private bool isJumpAllowed = true;
    private bool isGrounded;
    private LayerMask combinedLayers; 



    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentSprintTime = maxSprintTime;
        combinedLayers = groundLayer | terrainLayer;

        MoveToStartPoint();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && isJumpAllowed)
        {
            PlayerJump();
        }
    }

    void FixedUpdate()
    {
        PlayerMovement();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, combinedLayers);
        if (transform.position.y > maxHeight)
        {
            Vector3 clampedPosition = transform.position;
            clampedPosition.y = maxHeight;
            transform.position = clampedPosition;
        }
    }

    private void PlayerMovement()
    {
        if (_rigidbody.isKinematic)
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical");     

        Vector3 direction = transform.right * moveHorizontal + transform.forward * moveVertical;

        float currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && canSprint && isSprintAllowed && moveVertical > 0 && moveHorizontal == 0)
        {
            currentSpeed = sprint;
            currentSprintTime -= maxSprintTime / maxSprintTime * Time.fixedDeltaTime;

            if (currentSprintTime <= 0)
            {
                canSprint = false;
                currentSprintTime = 0;
            }
        }
        else
        {
            currentSpeed = speed;

            if (currentSprintTime < maxSprintTime)
            {
                currentSprintTime += maxSprintTime / sprintRechargeRate * Time.fixedDeltaTime;

                if (currentSprintTime >= maxSprintTime)
                {
                    currentSprintTime = maxSprintTime;
                    canSprint = true;
                }
            }
        }

        Vector3 finalVelocity = direction * currentSpeed;
        finalVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = finalVelocity;
    }

    public float GetStamina()
    {
        return currentSprintTime;
    }

    public float GetMaxStamina()
    {
        return maxSprintTime;
    }

    private void PlayerJump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsSprinting()
    {
        return Input.GetKey(KeyCode.LeftShift) && canSprint && isSprintAllowed;
    }

    public bool IsSprintAllowed()
    {
        return isSprintAllowed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NoSprintZone"))
        {
            isSprintAllowed = false;
            isJumpAllowed = false;

            RainManager rainManager = GetComponentInChildren<RainManager>();
            if (rainManager != null)
            {
                rainManager.SetRainActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NoSprintZone"))
        {
            isSprintAllowed = true;
            isJumpAllowed = true;

            RainManager rainManager = GetComponentInChildren<RainManager>();
            if (rainManager != null)
            {
                rainManager.SetRainActive(true);
            }
        }
    }

    public void MoveToStartPoint()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
        }
    }
}