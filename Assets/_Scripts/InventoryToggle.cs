using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField, Tooltip("Canvas principal del inventario")]
    private GameObject inventoryCanvas;

    [SerializeField, Tooltip("Referencia al script PlayerController")]
    private PlayerController playerController;

    private bool isInventoryOpen = false;

    void Start()
    {
        // Asegurarnos de que el Canvas del inventario esté oculto al inicio
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Abrir/cerrar inventario con Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (inventoryCanvas == null || playerController == null)
        {
            Debug.LogWarning("El Inventory Canvas o PlayerController no están asignados.");
            return;
        }

        // Cambiar el estado del inventario
        isInventoryOpen = !isInventoryOpen;
        inventoryCanvas.SetActive(isInventoryOpen);

        // Bloquear/desbloquear movimiento del jugador
        playerController.enabled = !isInventoryOpen;

        // Gestionar el cursor
        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;

        Debug.Log($"Inventario {(isInventoryOpen ? "abierto" : "cerrado")}. Movimiento del jugador {(isInventoryOpen ? "bloqueado" : "permitido")}.");
    }
}
