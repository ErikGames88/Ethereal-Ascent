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

    void Start()
    {
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false); // Asegurarse de que el Canvas está oculto al inicio
            Debug.Log("InventoryToggle: Canvas oculto al inicio.");
        }
        else
        {
            Debug.LogWarning("InventoryToggle: Inventory Canvas no está asignado.");
        }

        // Cursor siempre oculto y bloqueado
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerLocked == null)
        {
            playerLocked = FindObjectOfType<PlayerLocked>();
            if (playerLocked == null)
            {
                Debug.LogError("PlayerLocked no asignado y no se encontró en la escena.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ManageInventory();
        }
    }

    private void ManageInventory()
    {
        if (inventoryCanvas == null || playerLocked == null || inventoryManager == null)
        {
            Debug.LogWarning("InventoryToggle: Alguna referencia no está asignada.");
            return;
        }

        isInventoryOpen = !isInventoryOpen;
        inventoryCanvas.SetActive(isInventoryOpen);

        playerLocked.LockPlayer(isInventoryOpen); // Bloquea o desbloquea el jugador

        // Cursor siempre bloqueado e invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log($"InventoryToggle: Inventario {(isInventoryOpen ? "abierto" : "cerrado")}. Cursor siempre oculto.");

        if (!isInventoryOpen) // Cuando el inventario se cierra
        {
            Debug.Log("Cerrando inventario: Ocultando textos.");
            inventoryManager.HideAllTexts(); // Oculta todos los textos del inventario
            Debug.Log("InventoryToggle: Todos los textos desactivados al cerrar el inventario.");
        }
    }
}