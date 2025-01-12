using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField, Tooltip("Duración en segundos de la congelación de tiempo")]
    private float freezeDuration = 1f;

    [SerializeField, Tooltip("Referencia al GameManager")]
    private GameManager gameManager;

    private bool isGameOverHandled = false;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOverHandled) return;

        isGameOverHandled = true;

        Debug.Log("Game Over Triggered");

        if (gameManager != null)
        {
            gameManager.ChangeState(GameManager.GameState.GameOver);
        }

        StartCoroutine(HandleGameOverSequence());
    }

    private IEnumerator HandleGameOverSequence()
    {
        Debug.Log($"Time.timeScale antes: {Time.timeScale}");
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(freezeDuration); // Espera mientras el tiempo está congelado

        Debug.Log($"Cambiando a Game Over Scene tras {freezeDuration} segundos.");

        // Restaurar TimeScale antes de cambiar de escena
        Time.timeScale = 1f;

        // Cargar la Game Over Scene
        SceneManager.LoadScene("Game Over Scene");
    }
}