using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField, Tooltip("Canvas principal del inventario")]
    private GameObject inventoryCanvas;

    [SerializeField, Tooltip("Referencia al PlayerLocked para bloquear/desbloquear movimiento")]
    private PlayerLocked playerLocked;

    [SerializeField, Tooltip("Referencia al InventoryManager para manejar lógica del inventario")]
    private InventoryManager inventoryManager;

    [SerializeField, Tooltip("Referencia al TimerManager para pausar y reanudar el Timer")]
    private TimerManager timerManager;

    [SerializeField, Tooltip("Referencia al PauseMenuManager para saber si el menú está abierto")]
    private PauseMenuManager pauseMenuManager;

    public static bool isInventoryOpen = false;

    private bool canToggleInventory = true; // Nueva bandera para habilitar/deshabilitar el uso de Tab

    void Start()
    {
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }

        if (playerLocked == null)
        {
            playerLocked = FindObjectOfType<PlayerLocked>();
        }

        if (timerManager == null)
        {
            timerManager = FindObjectOfType<TimerManager>();
        }
    }

    void Update()
    {
        // No permitir abrir el inventario si el menú de pausa está activo
        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            return; // Si el menú de pausa está activo, no se puede abrir el inventario
        }

        // Bloquear interacción si no se permite abrir el inventario
        if (!canToggleInventory)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ManageInventory();
        }
    }

    private void ManageInventory()
    {
        if (inventoryCanvas == null || playerLocked == null || inventoryManager == null || timerManager == null)
        {
            return;
        }

        if (!isInventoryOpen && playerLocked.IsPlayerLocked())
        {
            return; 
        }

        isInventoryOpen = !isInventoryOpen; 
        
        inventoryCanvas.SetActive(isInventoryOpen); 
        
        if (isInventoryOpen)
        {
            playerLocked.LockPlayer(true, false); 
            inventoryManager.UpdateItemTextVisibility();
            timerManager.StopTimer(); // Detener el Timer al abrir el inventario
        }
        else
        {
            playerLocked.LockPlayer(false); 
            timerManager.StartTimer(); // Reanudar el Timer al cerrar el inventario
        }

        if (inventoryManager != null)
        {
            inventoryManager.SetSlotNavigation(isInventoryOpen);
        }

        if (!isInventoryOpen)
        {
            inventoryManager.HideAllTexts();
        }
    }

    // Nuevo método para habilitar/deshabilitar el uso de Tab
    public void EnableInventoryToggle(bool isEnabled)
    {
        canToggleInventory = isEnabled;
    }
}