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

    [SerializeField, Tooltip("AudioSource para la música de Game Over")]
    private AudioSource audioSource;

    private bool isGameOverHandled = false;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null && gameManager != null)
        {
            audioSource = gameManager.GetComponent<AudioSource>();
        }

        // Asegurarse de que el AudioSource persista entre escenas
        if (audioSource != null)
        {
            DontDestroyOnLoad(audioSource.gameObject);
        }
        else
        {
            Debug.LogError("AudioSource no asignado. La música de Game Over no podrá reproducirse.");
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOverHandled) return;

        isGameOverHandled = true;

        Debug.Log("Game Over Triggered");

        PlayGameOverMusic();

        if (gameManager != null)
        {
            gameManager.ChangeState(GameManager.GameState.GameOver);
        }

        StartCoroutine(HandleGameOverSequence());
    }

    private void PlayGameOverMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("Música de Game Over iniciada.");
        }
        else
        {
            Debug.LogError("AudioSource no asignado o ya está reproduciéndose.");
        }
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