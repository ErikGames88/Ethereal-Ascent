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

    [SerializeField, Tooltip("Referencia al KeyManager para manejar las llaves")]
    private KeyManager keyManager;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if (inventoryCanvas == null || playerController == null || cameraController == null || inventoryManager == null)
        {
            Debug.LogWarning("InventoryToggle: Alguna referencia no está asignada.");
            return;
        }

        isInventoryOpen = !isInventoryOpen;
        inventoryCanvas.SetActive(isInventoryOpen);

        playerController.enabled = !isInventoryOpen;
        cameraController.enabled = !isInventoryOpen;

        Debug.Log($"InventoryToggle: Inventario {(isInventoryOpen ? "abierto" : "cerrado")}.");

        if (isInventoryOpen)
        {
            UpdateSlotState(); // Nuevo método para actualizar el estado del slot
        }

        if (!isInventoryOpen)
        {
            if (keyManager.CathedralKeyText != null)
            {
                keyManager.CathedralKeyText.SetActive(false);
                Debug.Log("Texto de la llave desactivado al cerrar el inventario.");
            }
        }
    }

    private void UpdateSlotState()
    {
        if (inventoryManager.Slots == null || inventoryManager.Slots.Count == 0)
        {
            Debug.LogWarning("InventoryToggle: No hay slots disponibles en el InventoryManager.");
            return;
        }

        // Actualizar visualmente los slots o realizar lógica personalizada
        Debug.Log("Estado de los slots actualizado al abrir el inventario.");
    }
}
