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

    private bool isForcedHidden = false; 

    [SerializeField] private PauseMenuManager pauseMenuManager;  // Referencia al PauseMenuManager
    

    void Update()
    {
        if (playerController == null || foreground == null || staminaBar == null)
        {
            return;
        }

        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            // Si el menú de pausa está activo, ocultar la barra de estamina
            if (staminaBar.activeSelf)
            {
                staminaBar.SetActive(false);  // Desactivar la barra de estamina
            }
            return;
        }

        // Si el menú de pausa no está activo, proceder con la lógica de mostrar/ocultar la barra de estamina
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
        }
    }

    public void AllowStaminaBar()
    {
        isForcedHidden = false;
    }
}
    

