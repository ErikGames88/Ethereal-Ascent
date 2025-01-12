using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolRoute
{
    public Transform[] patrolPoints; 
}

public class EnemyManager : MonoBehaviour
{
    
    // TODO: Conitune working in Final Version after!!!
    public GameObject enemyPrefab; 
    public Transform[] spawnPoints; 
    public List<PatrolRoute> patrolRoutes; 
    private List<GameObject> enemies = new List<GameObject>(); 
    public float detectionRange = 10f; 
    public float attackRange = 2f;


    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return;
        }

        if (patrolRoutes == null || patrolRoutes.Count == 0)
        {
            return;
        }

        if (spawnPoints.Length != patrolRoutes.Count)
        {;
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null)
            {
                continue;
            }

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            if (newEnemy == null)
            {
                continue;
            }

            EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
            if (enemyAI == null)
            {
                continue;
            }

            if (i < patrolRoutes.Count && patrolRoutes[i].patrolPoints != null)
            {
                enemyAI.SetPatrolPoints(patrolRoutes[i].patrolPoints);
            }

            enemyAI.DetectionRange = detectionRange;
            enemyAI.AttackRange = attackRange;

            enemies.Add(newEnemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (spawnPoints != null)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}