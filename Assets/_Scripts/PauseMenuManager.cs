using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu; // El panel del menú de pausa

    [SerializeField]
    private PlayerController playerController; // Referencia al PlayerController

    [SerializeField]
    private MissionManager missionManager; // Referencia al MissionManager para comprobar si el MissionText se ha cerrado

    [SerializeField]
    private TimerManager timerManager; // Referencia al TimerManager para congelar el timer

    void Update()
    {
        // Bloquear la apertura del Pause Menu hasta que el MissionText se haya cerrado
        if (missionManager != null && !missionManager.isMissionTextClosed)
        {
            return; // No se puede abrir el Pause Menu hasta que el MissionText esté cerrado
        }

        // Solo activar el menú de pausa si no está abierto el inventario
        if (Input.GetKeyDown(KeyCode.Escape) && !InventoryToggle.isInventoryOpen)
        {
            ManagePauseMenu(); // Llamamos a la función para activar o desactivar el menú
        }
    }

    // Activar o desactivar el menú de pausa
    void ManagePauseMenu()
    {
        bool isPaused = !pauseMenu.activeSelf;

        // Activar o desactivar el menú
        pauseMenu.SetActive(isPaused);

        // Si el menú está activo, desactivar el movimiento del jugador
        if (isPaused)
        {
            Debug.Log("Pause Menu ACTIVADO");

            // Congelar el movimiento del jugador
            playerController.GetComponent<Rigidbody>().isKinematic = true;

            // Congelar el Timer
            timerManager.StopTimer();

            // Mostrar el cursor cuando el Pause Menu está activo
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Liberar el cursor
        }
        else
        {
            Debug.Log("Pause Menu DESACTIVADO");

            // Reactivar el movimiento del jugador
            playerController.GetComponent<Rigidbody>().isKinematic = false;

            // Reactivar el Timer
            timerManager.StartTimer();

            // Ocultar el cursor cuando el Pause Menu no está activo
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor
        }
    }

    public bool IsPaused()
    {
        return pauseMenu.activeSelf;  // Devuelve true si el menú está activo
    }
}