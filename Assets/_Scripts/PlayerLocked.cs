using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocked : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private CrosshairManager crosshairManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private StaminaUI staminaUI;
    private Rigidbody playerRigidbody;
    

    private void Awake()
    {
        if (playerController != null)
        {
            playerRigidbody = playerController.GetComponent<Rigidbody>();
        }

        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.SetSlotNavigation(true);
            }
        }

        if (staminaUI == null)
        {
            staminaUI = FindObjectOfType<StaminaUI>();
        }

        SetCursorState(CursorLockMode.Locked, false);
    }

    public void LockPlayer(bool isLocked, bool showCursor = false)
    {
        if (playerController != null)
        {
            playerController.enabled = !isLocked;
        }

        if (cameraController != null)
        {
            cameraController.enabled = !isLocked;
        }

        if (crosshairManager != null)
        {
            crosshairManager.ShowCrosshair(!isLocked);
        }

        if (inventoryManager != null)
        {
            inventoryManager.SetSlotNavigation(!isLocked);
        }

        if (staminaUI != null)
        {
            if (isLocked)
            {
                staminaUI.ForceHideStaminaBar();
            }
            else
            {
                staminaUI.AllowStaminaBar();
            }
        }

        if (isLocked && playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        if (isLocked)
        {
            SetCursorState(CursorLockMode.None, showCursor);
        }
        else
        {
            SetCursorState(CursorLockMode.Locked, false);
        }
    }

    private void SetCursorState(CursorLockMode lockState, bool visible)
    {
        Cursor.lockState = lockState;
        Cursor.visible = visible;
    }

    public bool IsPlayerLocked()
    {
        return playerController != null && !playerController.enabled; 
    }
}