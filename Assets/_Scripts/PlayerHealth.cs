using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float currentHealth;
    public float CurrentHealth { get => currentHealth; }
    private float maxHeatlh;

   


    void Awake()
    {
        maxHeatlh = 100f;
        currentHealth = maxHeatlh;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
