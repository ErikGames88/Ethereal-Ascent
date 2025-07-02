using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float currentHealth;
    public float CurrentHealth { get => currentHealth; }
    private float maxHeatlh;
    [SerializeField] private bool getDamage; // TODO: ONLY FOR TESTING, REMOVE [SerializeField] LATER
    public event Action OnPlayerHurt;
    public event Action OnPlayerDies;
    public bool GetDamage { get => getDamage; set => getDamage = value; }

    

    void Awake()
    {
        maxHeatlh = 100f;
        currentHealth = maxHeatlh;
    }

    void Update() // TEMPORALLY UPDATE!!!!!
    {
        // TEMPORALLY!!! JUST TO TEST THE HURT CLIP. REMOVE IT WHEN THERE ARE THINGS WHICH HURTS TO PLAYER

        // if (getDamage)
        // {
        //     OnPlayerHurt?.Invoke();
        //     getDamage = false;
        // }

        // if (currentHealth <= 0)
        // {
        //     OnPlayerDies?.Invoke();
        // }
    }

    /// <summary>
    /// Subtracts a specified amount of damage from the player's health.
    /// If health reaches zero, triggers the Game Over state (to be implemented).
    /// </summary>
    public void SubtractPlayerHealth(float damage)
    {
        getDamage = true;
        currentHealth -= damage;

        OnPlayerHurt?.Invoke();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnPlayerDies?.Invoke();

            // TODO: ESTADO DE GAME OVER
        }
    }
}
