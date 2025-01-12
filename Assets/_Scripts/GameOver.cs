using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 1f;
    [SerializeField] private GameManager gameManager;

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

        if (gameManager != null)
        {
            gameManager.ChangeState(GameManager.GameState.GameOver);
        }

        StartCoroutine(HandleGameOverSequence());
    }

    private IEnumerator HandleGameOverSequence()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(freezeDuration); 

        Time.timeScale = 1f;
        SceneManager.LoadScene("Game Over Scene");
    }
}