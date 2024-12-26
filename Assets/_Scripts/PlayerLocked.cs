using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocked : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al PlayerController para bloquear movimiento")]
    private PlayerController playerController;

    [SerializeField, Tooltip("Referencia al CameraController para bloquear rotación")]
    private CameraController cameraController;

    [SerializeField, Tooltip("Referencia al CrosshairManager para manejar el crosshair")]
    private CrosshairManager crosshairManager;

    [SerializeField, Tooltip("Referencia al InventoryManager para bloquear desplazamiento de slots")]
    private InventoryManager inventoryManager;

    [SerializeField, Tooltip("Referencia al StaminaUI para ocultar la barra de estamina")]
    private StaminaUI staminaUI;

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