using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float currentHealth;
    public float CurrentHealth { get => currentHealth; }
    private float maxHeatlh;
    private bool getDamage;
    public bool GetDamage { get => getDamage; set => getDamage = value; }


    void Awake()
    {
        maxHeatlh = 100f;
        currentHealth = maxHeatlh;
    }


    public void SubtractPlayerHealth(float damage)
    {
        getDamage = true;
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            // TODO: ESTADO DE GAME OVER
        }
    }
}
