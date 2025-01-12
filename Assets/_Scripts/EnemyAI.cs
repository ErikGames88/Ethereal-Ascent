using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //TODO: Conitune working in Final Version after!!!
    private NavMeshAgent agent;
    private Animator animator;

    public enum EnemyState { Idle, Patrol, ChasePlayer, AttackPlayer }
    public EnemyState currentState;

    [SerializeField] private float detectionRange = 10f; 
    public float DetectionRange
    {
        get => detectionRange;
        set => detectionRange = value;
    }

    [SerializeField] private float attackRange = 2f; 
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }

    private Transform player;
    private Transform[] patrolPoints; 
    private int currentPatrolIndex;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            return;// Salir si no se encuentra el NavMeshAgent
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            return; 
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (animator == null || agent == null)
        {
            return;
        }

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

                break;
        }
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0f);

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentState = EnemyState.Patrol;
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            currentState = EnemyState.ChasePlayer;
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            currentState = EnemyState.Idle;
            return;
        }

        animator.SetFloat("Speed", 1f);
        agent.speed = 2f;

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            currentState = EnemyState.ChasePlayer;
        }
    }

    private void ChasePlayer()
    {
        animator.SetFloat("Speed", 4f);
        agent.speed = 4f;
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = EnemyState.AttackPlayer;
        }
    }

    private void AttackPlayer()
    {
        agent.isStopped = true;
        animator.SetBool("IsAttacking", true);

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            animator.SetBool("IsAttacking", false);
            agent.isStopped = false;
            currentState = EnemyState.ChasePlayer;
        }
    }

    public void SetPatrolPoints(Transform[] points)
    {
        if (points == null || points.Length == 0)
        {
            return; 
        }
        patrolPoints = points;
    }
}