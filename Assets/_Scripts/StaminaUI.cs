using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al GameObject de la barra de estamina")]
    private GameObject staminaBar;

    [SerializeField, Tooltip("Referencia a la imagen de la barra de progreso")]
    private Image foreground;

    [SerializeField, Tooltip("Referencia al PlayerController para obtener la estamina")]
    private PlayerController playerController;

    private bool isForcedHidden = false; // Indica si la barra debe permanecer oculta

    void Update()
    {
        if (playerController == null || foreground == null || staminaBar == null)
        {
            Debug.LogError("Referencias faltantes en StaminaUI.");
            return;
        }

        if (isForcedHidden)
        {
            if (staminaBar.activeSelf)
            {
                staminaBar.SetActive(false);
            }
            return; // No hacer nada más si la barra está forzada a ocultarse
        }

        float staminaPercent = playerController.GetStamina() / playerController.GetMaxStamina();
        foreground.fillAmount = staminaPercent;

        if (playerController.IsSprinting() && playerController.IsSprintAllowed() && !staminaBar.activeSelf)
        {
            staminaBar.SetActive(true);
        }
        else if (!playerController.IsSprinting() && staminaPercent >= 1 && staminaBar.activeSelf)
        {
            staminaBar.SetActive(false);
        }
    }

    public void ForceHideStaminaBar()
    {
        isForcedHidden = true;
        if (staminaBar != null && staminaBar.activeSelf)
        {
            staminaBar.SetActive(false);
            Debug.LogError("StaminaUI: Barra de estamina forzada a ocultarse.");
        }
    }

    public void AllowStaminaBar()
    {
        isForcedHidden = false;
        Debug.LogError("StaminaUI: Barra de estamina permitida nuevamente.");
    }
}
    

