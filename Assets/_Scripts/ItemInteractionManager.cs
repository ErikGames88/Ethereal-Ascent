using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Distancia máxima de interacción")]
    private float interactionDistance = 3f;

    [SerializeField, Tooltip("Layer de las puertas")]
    private LayerMask interactableLayer;

    private InventoryManager inventoryManager;

    void Start()
    {
        // Obtener la referencia al InventoryManager
        inventoryManager = FindObjectOfType<InventoryManager>();

        if (inventoryManager == null)
        {
            Debug.LogError("No se encontró el InventoryManager en la escena.");
        }
    }

    void Update()
    {
        // Detectar la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        // Lanza un Raycast desde el centro de la pantalla (crosshair)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // Detectar si el Raycast impacta con un objeto interactuable
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log($"Interacción detectada con: {hit.collider.gameObject.name}");

            // Verificar si la llave está seleccionada en el inventario
            if (inventoryManager != null && inventoryManager.IsKeySelected())
            {
                Debug.Log("Llave seleccionada. Interactuando con la puerta...");
                // Aquí pondremos la lógica de abrir la puerta más adelante
            }
            else
            {
                Debug.Log("La llave no está seleccionada en el inventario.");
            }
        }
        else
        {
            Debug.Log("No estás mirando ningún objeto interactuable.");
        }
    }
}
