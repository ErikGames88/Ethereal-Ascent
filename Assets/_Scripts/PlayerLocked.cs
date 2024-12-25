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

    public void LockPlayer(bool isLocked)
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

        // Gestión del cursor según el estado
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.None; // Cursor desbloqueado
            Cursor.visible = true; // Cursor visible
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Cursor bloqueado
            Cursor.visible = false; // Cursor oculto
        }

        Debug.Log($"Cursor {(isLocked ? "visible y desbloqueado" : "oculto y bloqueado")}");
    }
}
