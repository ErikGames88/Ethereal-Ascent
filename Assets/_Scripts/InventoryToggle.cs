using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField, Tooltip("Canvas principal del inventario")]
    private GameObject inventoryCanvas;

    [SerializeField, Tooltip("Referencia al PlayerController para bloquear movimiento")]
    private PlayerController playerController;

    [SerializeField, Tooltip("Referencia al script CameraController para bloquear rotación")]
    private CameraController cameraController;

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

        // Asegurarnos de que el cursor esté siempre bloqueado y oculto
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Alternar el inventario con la tecla Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ManageInventory();
        }
    }

    private void ManageInventory()
    {
        if (inventoryCanvas == null || playerController == null || cameraController == null || inventoryManager == null)
        {
            Debug.LogWarning("InventoryToggle: Alguna referencia no está asignada.");
            return;
        }

        // Alternar el estado del inventario
        isInventoryOpen = !isInventoryOpen; // Actualizar la variable estática
        inventoryCanvas.SetActive(isInventoryOpen);

        // Bloquear movimiento y rotación del jugador
        playerController.enabled = !isInventoryOpen;
        cameraController.enabled = !isInventoryOpen;

        Debug.Log($"InventoryToggle: Inventario {(isInventoryOpen ? "abierto" : "cerrado")}.");
    }
}
