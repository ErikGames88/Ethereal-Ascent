using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private Image foreground;
    [SerializeField]private PlayerController playerController;
    [SerializeField] private TextManager textManager;
    [SerializeField] private PauseMenuManager pauseMenuManager;
    private bool isForcedHidden = false;
    

    void Update()
    {
        if (playerController == null || foreground == null || staminaBar == null)
        {
            return;
        }

        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            if (staminaBar.activeSelf)
            {
                staminaBar.SetActive(false);
            }
            return;
        }

        if (textManager != null && textManager.IsTextActive())
        {
            if (staminaBar.activeSelf)
            {
                staminaBar.SetActive(false);
            }
            return;
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
        }
    }

    public void AllowStaminaBar()
    {
        isForcedHidden = false;
    }
}
    

