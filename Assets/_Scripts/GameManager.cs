using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Gameplay, Victory, GameOver, MainMenu }
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                SceneManager.LoadScene("Main Menu");
                break;

            case GameState.Gameplay:
                SceneManager.LoadScene("Maze Scene");
                break;

            case GameState.Victory:
                SceneManager.LoadScene("Final Scene");
                break;

            case GameState.GameOver:
                SceneManager.LoadScene("Game Over Scene"); 
                break;
        }
    }
}