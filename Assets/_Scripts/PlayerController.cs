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
    private float maxSprintTime = 5f;

    [SerializeField, Tooltip("Velocidad de recarga del sprint")]
    private float sprintRechargeRate = 10f;

    private float currentSprintTime;
    public bool canSprint = true;

    [SerializeField, Tooltip("Fuerza del salto del Player")]
    private float jumpForce = 5f;

    private bool isGrounded;

    [SerializeField, Tooltip("Capa para identificar el suelo")]
    private LayerMask groundLayer;

    [SerializeField, Tooltip("Posición para verificar si el Player está en el suelo")]
    private Transform groundCheck;

    [SerializeField, Tooltip("Radio para detectar el suelo")]
    private float groundCheckRadius = 0.2f;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        currentSprintTime = maxSprintTime; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlayerJump();
        }
    }

    void FixedUpdate()
    {
        PlayerMovement();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Evitar rotación no deseada
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * moveHorizontal + transform.forward * moveVertical;

        float currentSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && direction.magnitude > 0)
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

    private void PlayerJump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public float GetStamina()
    {
        return currentSprintTime;
    }

    public float GetMaxStamina()
    {
        return maxSprintTime;
    }
}