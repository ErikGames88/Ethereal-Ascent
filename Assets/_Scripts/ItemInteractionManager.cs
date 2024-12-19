using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Distancia máxima de interacción")]
    private float interactionDistance = 3f;

    [SerializeField, Tooltip("Layer de los objetos interactuables")]
    private LayerMask interactableLayer;

    private InventoryManager inventoryManager;
    private FlashlightManager flashlightManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("No se encontró el InventoryManager en la escena.");
        }

        flashlightManager = FindObjectOfType<FlashlightManager>();
        if (flashlightManager == null)
        {
            Debug.LogError("No se encontró el FlashlightManager en la escena.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (!InventoryToggle.isInventoryOpen)
        {
            Debug.Log("El inventario está cerrado. No se puede interactuar con objetos.");
            return;
        }

        Button selectedSlot = inventoryManager.Slots[inventoryManager.SelectedSlotIndex];
        Image slotImage = selectedSlot.GetComponentInChildren<Image>();

        if (slotImage == null || slotImage.sprite == null)
        {
            Debug.Log("No hay objeto seleccionado en el slot actual.");
            return;
        }

        Debug.Log($"Objeto seleccionado: {slotImage.sprite.name}");

        GameObject slotPrefab = inventoryManager.SlotPrefabs[inventoryManager.SelectedSlotIndex];
        if (slotPrefab == null)
        {
            Debug.Log("Objeto seleccionado desconocido.");
            return;
        }

        Debug.Log($"Prefab asociado: {slotPrefab.name}");

        // Comprobar si es una llave y si se está interactuando con una puerta
        if (slotPrefab.name == "Cathedral Key")
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
            {
                Debug.Log($"Interacción detectada con: {hit.collider.gameObject.name}");

                DoorInteraction doorInteraction = hit.collider.GetComponentInParent<DoorInteraction>();
                if (doorInteraction != null)
                {
                    Debug.Log("Llave seleccionada. Abriendo la puerta...");
                    doorInteraction.Interact();
                    return;
                }
            }

            Debug.Log("No se pudo abrir la puerta. Asegúrate de tener la llave seleccionada.");
        }
        else
        {
            Debug.Log("El objeto seleccionado no es una llave.");
        }
    }
}
