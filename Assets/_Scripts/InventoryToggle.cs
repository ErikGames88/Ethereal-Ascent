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
    }

    void Update()
    {
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
        if (inventoryCanvas == null || playerLocked == null || inventoryManager == null)
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
        }
        else
        {
            playerLocked.LockPlayer(false); 
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