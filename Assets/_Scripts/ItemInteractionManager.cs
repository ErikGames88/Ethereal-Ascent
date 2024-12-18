using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Distancia máxima de interacción")]
    private float interactionDistance = 3f;

    [SerializeField, Tooltip("Layer de los objetos interactuables")]
    private LayerMask interactableLayer;

    private InventoryManager inventoryManager;
    private KeyManager keyManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("No se encontró el InventoryManager en la escena.");
        }

        keyManager = FindObjectOfType<KeyManager>();
        if (keyManager == null)
        {
            Debug.LogError("No se encontró el KeyManager en la escena.");
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

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log($"Interacción detectada con: {hit.collider.gameObject.name}");

            DoorInteraction doorInteraction = hit.collider.GetComponentInParent<DoorInteraction>();
            if (doorInteraction != null && keyManager.IsKeySelected(inventoryManager.SelectedSlotIndex))
            {
                Debug.Log("Llave seleccionada. Abriendo la puerta...");
                doorInteraction.Interact();
            }
            else
            {
                Debug.Log("No se pudo abrir la puerta. Asegúrate de tener la llave seleccionada.");
            }
        }
        else
        {
            Debug.Log("No estás mirando ningún objeto interactuable.");
        }
    }
}
