using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public enum EnemyState { Idle, Patrol, ChasePlayer, AttackPlayer }
    public EnemyState currentState;

    [Header("Behavior Settings")]
    [SerializeField] private float detectionRange = 10f; // Rango de detección del jugador
    public float DetectionRange
    {
        get => detectionRange;
        set => detectionRange = value;
    }

    [SerializeField] private float attackRange = 2f; // Rango de ataque
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }

    private Transform player;

    private Transform[] patrolPoints; // Puntos de patrulla privados
    private int currentPatrolIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("EnemyAI: No se encontró el componente NavMeshAgent en " + gameObject.name);
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("EnemyAI: No se encontró el componente Animator en " + gameObject.name);
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("EnemyAI: No se encontró un objeto con la etiqueta Player.");
        }
    }

    void Update()
    {
        if (animator == null || agent == null)
        {
            Debug.LogError("EnemyAI: Componentes esenciales no asignados en " + gameObject.name);
            return;
        }

        Debug.LogError($"EnemyAI: Estado actual -> {currentState}");

        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.ChasePlayer:
                ChasePlayer();
                break;

            case EnemyState.AttackPlayer:
                AttackPlayer();
                break;

            default:
                Debug.LogError("EnemyAI: Estado desconocido en " + gameObject.name);
                break;
        }
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0f);
        Debug.LogError("EnemyAI: Animación Idle activada.");

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Debug.LogError("EnemyAI: Cambiando de Idle a Patrol.");
            currentState = EnemyState.Patrol;
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            Debug.LogError("EnemyAI: Cambiando de Idle a ChasePlayer.");
            currentState = EnemyState.ChasePlayer;
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("EnemyAI: No hay puntos de patrulla asignados.");
            currentState = EnemyState.Idle;
            return;
        }

        animator.SetFloat("Speed", 1f);
        Debug.LogError("EnemyAI: Animación Walk activada.");
        agent.speed = 2f;

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            Debug.LogError($"EnemyAI: Moviéndose al siguiente punto de patrulla: {patrolPoints[currentPatrolIndex].position}");
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            Debug.LogError("EnemyAI: Cambiando de Patrol a ChasePlayer.");
            currentState = EnemyState.ChasePlayer;
        }
    }

    private void ChasePlayer()
    {
        animator.SetFloat("Speed", 4f);
        Debug.LogError("EnemyAI: Animación Run activada.");
        agent.speed = 4f;
        agent.SetDestination(player.position);
        Debug.LogError($"EnemyAI: Persiguiendo al jugador en posición: {player.position}");

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.LogError("EnemyAI: Cambiando de ChasePlayer a AttackPlayer.");
            currentState = EnemyState.AttackPlayer;
        }
    }

    private void AttackPlayer()
    {
        agent.isStopped = true;
        animator.SetBool("IsAttacking", true);
        Debug.LogError("EnemyAI: Animación Attack activada.");

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            Debug.LogError("EnemyAI: Cambiando de AttackPlayer a ChasePlayer.");
            animator.SetBool("IsAttacking", false);
            agent.isStopped = false;
            currentState = EnemyState.ChasePlayer;
        }
    }

    public void SetPatrolPoints(Transform[] points)
    {
        if (points == null || points.Length == 0)
        {
            Debug.LogError("EnemyAI: Los puntos de patrulla asignados son nulos o están vacíos.");
        }
        patrolPoints = points;
        Debug.LogError("EnemyAI: Puntos de patrulla asignados correctamente.");
    }
}