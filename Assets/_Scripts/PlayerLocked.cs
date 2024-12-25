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

    private Rigidbody playerRigidbody;

    private void Awake()
    {
        // Encuentra automáticamente el Rigidbody del jugador
        if (playerController != null)
        {
            playerRigidbody = playerController.GetComponent<Rigidbody>();
            if (playerRigidbody == null)
            {
                Debug.LogError("No se encontró un Rigidbody en el PlayerController.");
            }
        }
        else
        {
            Debug.LogError("PlayerController no está asignado.");
        }
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

        // Detener el movimiento residual del Rigidbody al bloquear al jugador
        if (isLocked && playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero; // Detener cualquier movimiento
            playerRigidbody.angularVelocity = Vector3.zero; // Detener rotación
            Debug.Log("Rigidbody detenido.");
        }

        // Gestión del cursor según el estado y el parámetro showCursor
        if (isLocked && showCursor)
        {
            Cursor.lockState = CursorLockMode.None; // Cursor desbloqueado
            Cursor.visible = true; // Cursor visible
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Cursor bloqueado
            Cursor.visible = false; // Cursor oculto
        }

        Debug.Log($"Cursor {(Cursor.visible ? "visible" : "oculto")} y {(Cursor.lockState == CursorLockMode.Locked ? "bloqueado" : "desbloqueado")}");
    }
}