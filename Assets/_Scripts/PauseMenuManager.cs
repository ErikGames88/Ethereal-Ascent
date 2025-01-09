using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu; // El panel del menú de pausa

    [SerializeField]
    private PlayerController playerController; // Referencia al PlayerController

    void Update()
    {
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

            // Mostrar el cursor cuando el Pause Menu está activo
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Liberar el cursor
        }
        else
        {
            Debug.Log("Pause Menu DESACTIVADO");

            // Reactivar el movimiento del jugador
            playerController.GetComponent<Rigidbody>().isKinematic = false;

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