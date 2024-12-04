using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Slot slot = inventorySlots[slotIndex].GetComponent<Slot>();
        if (slot == null || slot.GetStoredItem() == null)
        {
            Debug.LogWarning("No hay ningún objeto en este slot para equipar.");
            return;
        }

        GameObject itemPrefab = slot.GetStoredItem();

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

            if (itemPrefab.name.Contains("Flashlight"))
            {
                equippedObject.transform.localPosition = new Vector3(0.43f, -0.32f, 0.6f);
                equippedObject.transform.localRotation = Quaternion.Euler(90f, -5.06f, 0f);
                equippedObject.transform.localScale = new Vector3(1f, 1f, 1f);

                FlashLight flashLightScript = equippedObject.GetComponent<FlashLight>();
                if (flashLightScript != null)
                {
                    flashLightScript.InitializeLight();
                    Debug.Log("Luz de la linterna inicializada al equiparla.");
                }
                else
                {
                    Debug.LogWarning("El prefab de la linterna no tiene el script FlashLight asignado.");
                }
            }
            else if (itemPrefab.name.Contains("Revolver"))
            {
                equippedObject.transform.localPosition = new Vector3(0.34f, -0.27f, 0.44f);
                equippedObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                equippedObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        Debug.Log($"Equipado: {equippedObject.name}");
        Debug.Log($"Posición actualizada: {equippedObject.transform.localPosition}");
    }

    public bool AddItemToInventory(GameObject itemPrefab, Sprite itemIcon)
    {
        foreach (Slot slot in inventorySlots)
        {
            if (slot.GetStoredItem() == null) // Busca el primer slot vacío
            {
                slot.StoreItem(itemPrefab, itemIcon); // Añade el objeto y el icono al slot
                Debug.Log($"Añadido {itemPrefab.name} al inventario en el slot {System.Array.IndexOf(inventorySlots, slot)}.");
                return true;
            }
        }

        Debug.LogWarning("No hay espacio disponible en el inventario.");
        return false; // No hay espacio disponible
    }      
}
