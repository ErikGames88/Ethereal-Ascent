using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    private float staminaDrainTime;
    private float staminaRecoverTime;
    private float maxStamina;
    private float minStamina;
    private float currentStamina;
    private float staminaDrainRate;
    private float staminaRecoveryRate;
    private bool canSprint;
    public bool CanSprint { get => canSprint; }


    void Awake()
    {
        maxStamina = 100f;
        minStamina = 0f;

        staminaDrainTime = 5f;
        staminaRecoverTime = 10f;

        staminaDrainRate = 20f;
        staminaRecoveryRate = 10f;
    }

    void Start()
    {
        canSprint = true;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleStamina();
    }

    void HandleStamina()
    {

        
        if (currentStamina >= minStamina)
        {
            // BAJAR ESTAMINA
            canSprint = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;

            if (currentStamina <= minStamina)
            {
                canSprint = false;
                currentStamina = minStamina;
            }
        }
    
    
        if (currentStamina < maxStamina)
        {
            // RELLENAR ESTAMINA

            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if (currentStamina > minStamina)
            {
                canSprint = true;
            }
            
            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        

        Debug.Log($"Current Stamina: {currentStamina}");
    }
}
