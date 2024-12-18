using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Distancia máxima de interacción")]
    private float interactionDistance = 3f; // Ajusta según el diseño del juego

    [SerializeField, Tooltip("LayerMask para objetos interactuables")]
    private LayerMask interactableLayer;

    private void Update()
    {
        // Detectar interacciones con Raycast cuando el jugador pulsa click izquierdo
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            InteractWithObject();
        }
    }

    private void InteractWithObject()
    {
        // Crear un Raycast desde el centro de la cámara
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        // Raycast limitado a objetos en la capa interactuable
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log($"Interacción con: {hit.collider.name}");

            // Verificar si el objeto tiene un script PickupItem
            PickupItem pickupItem = hit.collider.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                // Llamar al método Pickup del objeto interactuado
                pickupItem.Pickup(FindObjectOfType<KeyManager>(), FindObjectOfType<InventoryManager>());
            }
        }
    }
}
