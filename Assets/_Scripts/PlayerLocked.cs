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

    private Rigidbody playerRigidbody;

    private void Awake()
    {
        // Configuración del PlayerController y Rigidbody
        if (playerController != null)
        {
            playerRigidbody = playerController.GetComponent<Rigidbody>();
            if (playerRigidbody == null)
            {
                Debug.LogError("PlayerLocked: No se encontró un Rigidbody en el PlayerController.");
            }
        }
        else
        {
            Debug.LogError("PlayerLocked: PlayerController no está asignado.");
        }

        // Configuración del InventoryManager
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("PlayerLocked: No se encontró un InventoryManager en la escena.");
            }
            else
            {
                inventoryManager.SetSlotNavigation(true); // Habilitar navegación de slots por defecto
                Debug.LogError("PlayerLocked: Navegación de slots habilitada al inicio.");
            }
        }

        // Bloquear y ocultar el cursor al inicio
        SetCursorState(CursorLockMode.Locked, false);
    }

    public void LockPlayer(bool isLocked, bool showCursor = false)
    {
        if (playerController != null)
        {
            playerController.enabled = !isLocked;
            Debug.Log($"PlayerController {(isLocked ? "desactivado" : "activado")}");
        }

        if (cameraController != null)
        {
            cameraController.enabled = !isLocked;
            Debug.Log($"CameraController {(isLocked ? "desactivado" : "activado")}");
        }

        if (crosshairManager != null)
        {
            crosshairManager.ShowCrosshair(!isLocked);
            Debug.Log($"Crosshair {(isLocked ? "oculto" : "visible")}");
        }

        if (inventoryManager != null)
        {
            inventoryManager.SetSlotNavigation(!isLocked);
            Debug.LogError($"PlayerLocked: Navegación de slots {(isLocked ? "bloqueada" : "habilitada")} al {(isLocked ? "bloquear" : "desbloquear")} al jugador.");
        }

        if (isLocked && playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            Debug.Log("Rigidbody detenido.");
        }

        // Gestionar el cursor
        SetCursorState(isLocked ? CursorLockMode.None : CursorLockMode.Locked, isLocked && showCursor);
    }

    private void SetCursorState(CursorLockMode lockState, bool visible)
    {
        Cursor.lockState = lockState;
        Cursor.visible = visible;
        Debug.Log($"Cursor {(Cursor.visible ? "visible" : "oculto")} y {(Cursor.lockState == CursorLockMode.Locked ? "bloqueado" : "desbloqueado")}");
    }
}