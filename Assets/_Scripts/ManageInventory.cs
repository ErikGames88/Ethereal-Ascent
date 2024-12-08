using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ManageInventory : MonoBehaviour
{
    [SerializeField, Tooltip("Canvas del inventario")]
    private GameObject inventoryCanvas;

    [SerializeField, Tooltip("Array de los Slots del inventario")]
    private Slot[] inventorySlots;

    [SerializeField, Tooltip("Prefab de la linterna")]
    private GameObject flashlight;

    [SerializeField, Tooltip("Referencia al PlayerController para desactivar movimiento")]
    private PlayerController playerController;

    [SerializeField, Tooltip("Referencia al MessageManager para comprobar mensajes activos")]
    private MessageManager messageManager;

    private bool isInventoryOpen = false;
    private int selectedSlotIndex = 0;
    private GameObject equippedObject = null;

    void Start()
    {
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }

        if (inventorySlots.Length > 0)
        {
            HighlightSlot(selectedSlotIndex);
        }

        // Verifica si el MessageManager está asignado
        if (messageManager == null)
        {
            Debug.LogWarning("MessageManager no asignado en ManageInventory.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (isInventoryOpen)
        {
            HandleSlotNavigation();

            if (Input.GetKeyDown(KeyCode.E))
            {
                EquipObject(selectedSlotIndex);
            }
        }
    }

    private void ToggleInventory()
    {
        // No abrir el inventario si hay un mensaje activo
        if (messageManager != null && messageManager.IsMessageActive())
        {
            Debug.Log("No se puede abrir el inventario mientras un mensaje está activo.");
            return;
        }

        isInventoryOpen = !isInventoryOpen;

        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(isInventoryOpen);
        }

        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;

        if (playerController != null)
        {
            playerController.enabled = !isInventoryOpen;
        }
    }

    private void HandleSlotNavigation()
    {
        int previousSlotIndex = selectedSlotIndex;

        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedSlotIndex = Mathf.Max(0, selectedSlotIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            selectedSlotIndex = Mathf.Min(inventorySlots.Length - 1, selectedSlotIndex + 1);
        }

        if (selectedSlotIndex != previousSlotIndex)
        {
            HighlightSlot(selectedSlotIndex);
        }
    }

    private void HighlightSlot(int index)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Button button = inventorySlots[i].GetComponent<Button>();
            if (button != null)
            {
                ColorBlock colors = button.colors;
                if (i == index)
                {
                    colors.normalColor = Color.yellow;
                }
                else
                {
                    colors.normalColor = Color.white;
                }
                button.colors = colors;
            }
        }

        Debug.Log($"Slot seleccionado: {index}");
    }

    private void EquipObject(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Length)
        {
            Debug.LogWarning($"Índice {slotIndex} fuera de los límites del inventario.");
            return;
        }

        Slot slot = inventorySlots[slotIndex];
        if (slot == null)
        {
            Debug.LogWarning($"El slot {slotIndex} no existe o es nulo.");
            return;
        }

        GameObject itemPrefab = slot.GetStoredItem();
        string itemName = slot.GetStoredItemName();

        if (itemPrefab == null || string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning($"El slot {slotIndex} está vacío o no contiene un objeto válido.");
            return;
        }

        if (!itemPrefab.activeInHierarchy) 
        {
            Debug.LogWarning($"El objeto {itemName} está desactivado en la escena pero se encuentra en el inventario.");
        }

        Debug.Log($"Intentando equipar: {itemName} desde el slot {slotIndex}.");

        if (equippedObject != null)
        {
            Destroy(equippedObject);
            equippedObject = null;
        }

        equippedObject = Instantiate(itemPrefab);
        equippedObject.SetActive(true);

        Transform playerCamera = Camera.main.transform;
        if (playerCamera != null)
        {
            equippedObject.transform.SetParent(playerCamera);
        }

        Debug.Log($"Equipado: {itemName}.");
    }

    public bool AddItemToInventory(GameObject itemPrefab, Sprite itemIcon)
    {
        foreach (Slot slot in inventorySlots)
        {
            if (slot.GetStoredItem() == null) // Solo añade si el slot está vacío
            {
                if (itemPrefab == null || itemIcon == null)
                {
                    Debug.LogError("Error: El prefab o el ícono del objeto a añadir es nulo.");
                    return false;
                }

                slot.StoreItem(itemPrefab, itemIcon); // Asigna el prefab e ícono al slot

                Debug.Log($"Añadido al inventario: {itemPrefab.name}");
                return true;
            }
        }

        Debug.LogWarning("No hay espacio disponible en el inventario.");
        return false;
    }

    public GameObject GetEquippedItem()
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= inventorySlots.Length)
        {
            Debug.LogWarning($"Índice de slot seleccionado ({selectedSlotIndex}) fuera de rango.");
            return null;
        }

        Slot selectedSlot = inventorySlots[selectedSlotIndex];
        if (selectedSlot == null)
        {
            Debug.LogWarning("El slot seleccionado es nulo.");
            return null;
        }

        GameObject storedItem = selectedSlot.GetStoredItem();
        if (storedItem == null)
        {
            Debug.LogWarning("El slot seleccionado no contiene ningún objeto.");
            return null;
        }

        Debug.Log($"Objeto obtenido del inventario: {storedItem.name}");
        return storedItem;
    }  

    public bool IsInventoryOpen()
    {
        return isInventoryOpen;
    }
    public bool HasItemByTag(string tag)
    {
        Debug.Log($"Buscando objeto con el tag: {tag} en el inventario..."); // Depuración
        foreach (Slot slot in inventorySlots)
        {
            GameObject storedItem = slot.GetStoredItem();
            if (storedItem != null)
            {
                Debug.Log($"Objeto encontrado en slot: {slot.name}, Tag: {storedItem.tag}"); // Depuración
                if (storedItem.CompareTag(tag))
                {
                    Debug.Log($"Objeto con el tag {tag} encontrado en el inventario.");
                    return true;
                }
            }
        }

        Debug.LogWarning($"No se encontró ningún objeto con el tag {tag} en el inventario.");
        return false;
    }

    public void RemoveItemByTag(string tag)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            GameObject storedItem = inventorySlots[i].GetStoredItem();
            if (storedItem != null && storedItem.CompareTag(tag))
            {
                inventorySlots[i].ClearSlot();
                Debug.Log($"Eliminado objeto con Tag {tag} del slot {i}.");
                return;
            }
        }

        Debug.LogWarning($"Intento de eliminar objeto con Tag {tag}, pero no fue encontrado en el inventario.");
    }
}
