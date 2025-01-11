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

    [SerializeField, Tooltip("Capa para identificar el suelo (Ground)")]
    private LayerMask groundLayer;

    [SerializeField, Tooltip("Capa para identificar el Terrain")]
    private LayerMask terrainLayer;

    private bool isGrounded;

    [SerializeField, Tooltip("Posición para verificar si el Player está en el suelo")]
    private Transform groundCheck;

    [SerializeField, Tooltip("Altura máxima permitida para el Player")]
    private float maxHeight = 4f;

    [SerializeField, Tooltip("Referencia al punto de inicio del Player")]
    private Transform startPoint;

    private LayerMask combinedLayers; // Capas combinadas para Ground y Terrain

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentSprintTime = maxSprintTime;

        // Combinar las capas Ground y Terrain
        combinedLayers = groundLayer | terrainLayer;
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

        // Verificar si el Player está en el suelo usando las capas combinadas
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
        return Input.GetKey(KeyCode.LeftShift) && canSprint && isSprintAllowed;
    }

    public bool IsSprintAllowed()
    {
        return isSprintAllowed;
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto activador pertenece a la capa NoSprintZone
        if (other.gameObject.layer == LayerMask.NameToLayer("NoSprintZone"))
        {
            isSprintAllowed = false;
            isJumpAllowed = false;

            // Desactiva la lluvia si el RainManager está presente
            RainManager rainManager = GetComponentInChildren<RainManager>();
            if (rainManager != null)
            {
                rainManager.SetRainActive(false);
            }

            Debug.Log("Entrando en NoSprintZone: Sprint y lluvia desactivados.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto saliendo pertenece a la capa NoSprintZone
        if (other.gameObject.layer == LayerMask.NameToLayer("NoSprintZone"))
        {
            isSprintAllowed = true;
            isJumpAllowed = true;

            // Reactiva la lluvia si el RainManager está presente
            RainManager rainManager = GetComponentInChildren<RainManager>();
            if (rainManager != null)
            {
                rainManager.SetRainActive(true);
            }

            Debug.Log("Saliendo de NoSprintZone: Sprint y lluvia reactivados.");
        }
    }

    public void MoveToStartPoint()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
            Debug.Log("Player reposicionado al Start Point.");
        }
        else
        {
            Debug.LogError("Start Point no asignado en el Inspector.");
        }
    }
}