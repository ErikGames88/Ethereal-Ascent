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
    private float jumpStaminaCost;
    private float dodgeStaminaCost;
    private bool canSprint;
    private bool sprintActive;
    
    
    public float CurrentStamina { get => currentStamina; }
    public float JumpStaminaCost { get => jumpStaminaCost; }
    public float DodgeStaminaCost { get => dodgeStaminaCost; }
    public bool SprintActive { get => sprintActive; }
    public bool CanSprint { get => canSprint; set => canSprint = value; }


    void Awake()
    {
        maxStamina = 100f;
        minStamina = 0f;

        staminaDrainTime = 5f;
        staminaRecoverTime = 10f;

        staminaDrainRate = 20f;
        staminaRecoveryRate = 10f;

        jumpStaminaCost = 35f;
        dodgeStaminaCost = 49f;
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

    /// <summary>
    /// Controls stamina logic for sprinting. Depletes stamina when sprinting and 
    /// recovers it over time when not sprinting. Disables sprinting if stamina reaches zero.
    /// </summary>
    public void HandleStamina()
    {
        if (sprintActive && canSprint)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;

            if (currentStamina <= minStamina)
            {
                currentStamina = minStamina;
                canSprint = false;
                sprintActive = false;
            }
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;

            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                canSprint = true;
            }
        }
    }

    /// <summary>
    /// Enables or disables the stamina-draining state depending on whether the player
    /// is attempting to sprint and sprinting is currently allowed.
    /// </summary>
    public void SetSprintState(bool sprinting)
    {
        if (canSprint && sprinting)
        {
            sprintActive = true;
        }
        else
        {
            sprintActive = false;
        }
    }

    /// <summary>
    /// Subtracts the stamina cost of jumping from the current stamina.
    /// </summary>
    public float JumpStamina()
    {
        return currentStamina -= jumpStaminaCost;
    }

    /// <summary>
    /// Subtracts the stamina cost of dodging from the current stamina.
    /// </summary>
    public float DodgeStamina()
    {
        return currentStamina -= dodgeStaminaCost;
    }

    /// <summary>
    /// Returns the current stamina as a normalized value between 0 and 1, 
    /// useful for UI representation.
    /// </summary>
    public float GetStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }
}
