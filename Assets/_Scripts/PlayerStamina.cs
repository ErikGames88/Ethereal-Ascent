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
    private bool sprintActive;
    public bool SprintActive{ get => sprintActive;}
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

        Debug.Log($"Current Stamina: {currentStamina}");
    }

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
}
