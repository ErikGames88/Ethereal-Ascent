using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolRoute
{
    public Transform[] patrolPoints; // Array de puntos de patrulla para un enemigo
}

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // Prefab del enemigo
    public Transform[] spawnPoints; // Puntos de aparición

    [Header("Patrol Settings")]
    public List<PatrolRoute> patrolRoutes; // Lista de rutas de patrulla (visible en el Inspector)

    private List<GameObject> enemies = new List<GameObject>(); // Lista de enemigos activos

    [Header("Enemy Behavior Settings")]
    public float detectionRange = 10f; // Rango de detección del jugador
    public float attackRange = 2f; // Rango de ataque

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        // Validar que los arrays estén configurados
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyManager: El prefab de enemigo no está asignado.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("EnemyManager: No se han asignado puntos de spawn.");
            return;
        }

        if (patrolRoutes == null || patrolRoutes.Count == 0)
        {
            Debug.LogError("EnemyManager: No se han asignado rutas de patrulla.");
            return;
        }

        // Validar que la cantidad de spawnPoints coincida con patrolRoutes
        if (spawnPoints.Length != patrolRoutes.Count)
        {
            Debug.LogError("EnemyManager: La cantidad de puntos de spawn y rutas de patrulla no coincide.");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null)
            {
                Debug.LogError($"EnemyManager: El punto de spawn en el índice {i} no está asignado.");
                continue;
            }

            // Instanciar el enemigo en el punto de spawn
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            if (newEnemy == null)
            {
                Debug.LogError($"EnemyManager: Fallo al instanciar enemigo en el índice {i}.");
                continue;
            }

            Debug.Log($"EnemyManager: Enemigo instanciado en el índice {i} en la posición {spawnPoints[i].position}.");

            // Configurar los puntos de patrulla y parámetros de comportamiento
            EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
            if (enemyAI == null)
            {
                Debug.LogError($"EnemyManager: El prefab {enemyPrefab.name} no tiene el script EnemyAI asignado.");
                continue;
            }

            if (i < patrolRoutes.Count && patrolRoutes[i].patrolPoints != null)
            {
                enemyAI.SetPatrolPoints(patrolRoutes[i].patrolPoints);
                Debug.Log($"EnemyManager: Puntos de patrulla asignados al enemigo en el índice {i}.");
            }
            else
            {
                Debug.LogError($"EnemyManager: La ruta de patrulla en el índice {i} no está asignada o está vacía.");
            }

            // Configurar parámetros de comportamiento
            enemyAI.DetectionRange = detectionRange;
            enemyAI.AttackRange = attackRange;

            // Añadir el enemigo a la lista
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