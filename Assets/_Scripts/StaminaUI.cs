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

    [SerializeField, Tooltip("Referencia al TextManager para verificar si hay texto activo")]
    private TextManager textManager;

    private bool isForcedHidden = false;

    [SerializeField, Tooltip("Referencia al PauseMenuManager")]
    private PauseMenuManager pauseMenuManager;

    void Update()
    {
        if (playerController == null || foreground == null || staminaBar == null)
        {
            return;
        }

        // Ocultar la barra de estamina si el menú de pausa está activo
        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            if (staminaBar.activeSelf)
            {
                staminaBar.SetActive(false);
            }
            return;
        }

        // Ocultar la barra de estamina si el Letter Background y Key E Dismiss están activos
        if (textManager != null && textManager.IsTextActive())
        {
            if (staminaBar.activeSelf)
            {
                staminaBar.SetActive(false);
            }
            return;
        }

        // Lógica de la barra de estamina
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
    

