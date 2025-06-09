using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isAlive;
    private bool isDead;
    private PlayerHealth _playerHealth;

    private GameStates _gameStates;
    private enum GameStates
    {
        Playing,
        Pause,
        GameOver,
    }

    void Awake()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();

        _gameStates = GameStates.Playing;
        isAlive = true;
        isDead = false;
    }

    void Start()
    {

    }

    void Update()
    {
        GameOver();
    }

    private void GameOver()
    {
        if (_playerHealth.CurrentHealth <= 0)
        {
            if (!isDead)
            {
                _gameStates = GameStates.GameOver;
                isDead = true;
                isAlive = false;
            }
        }
        else
        {
            if (!isAlive)
            {
                _gameStates = GameStates.Playing;
                isDead = false;
                isAlive = true;
            }
        }
    }
}
