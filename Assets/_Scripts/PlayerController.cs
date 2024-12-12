using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("Velocidad normal del Player")]
    private float speed;

    [SerializeField, Tooltip("Velocidad de sprint del Player")]
    private float sprint;

    private Rigidbody _rigidbody;

    [SerializeField, Tooltip("Tiempo máximo de sprint en segundos")]
    private float maxSprintTime = 2f;

    [SerializeField, Tooltip("Velocidad de recarga del sprint")]
    private float sprintRechargeRate = 10f;
    private float currentSprintTime = 0;
    private bool canSprint = true;

    private bool isSprintAllowed = true; 
    private bool isJumpAllowed = true;

    [SerializeField, Tooltip("Fuerza del salto del Player")]
    private float jumpForce = 5f;

    [SerializeField, Tooltip("Radio para detectar el suelo")]
    private float groundCheckRadius = 0.2f;

    [SerializeField, Tooltip("Capa para identificar el suelo")]
    private LayerMask groundLayer;

    private bool isGrounded;

    [SerializeField, Tooltip("Posición para verificar si el Player está en el suelo")]
    private Transform groundCheck;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        currentSprintTime = maxSprintTime; 
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

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void PlayerMovement()
    {
        // Si el Rigidbody está en modo Kinematic, no hacer nada
        if (_rigidbody.isKinematic)
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * moveHorizontal + transform.forward * moveVertical;

        float currentSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && isSprintAllowed && direction.magnitude > 0)
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
        return Input.GetKey(KeyCode.LeftShift) && canSprint;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NoSprintZone"))
        {
            Debug.Log("Player entró en NoSprintZone");
            isSprintAllowed = false;
            isJumpAllowed = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NoSprintZone"))
        {
            Debug.Log("Player salió de NoSprintZone");
            isSprintAllowed = true;
            isJumpAllowed = true;
        }
    }
}