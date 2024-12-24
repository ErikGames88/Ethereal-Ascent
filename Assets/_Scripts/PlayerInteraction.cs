using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Distancia máxima de interacción")]
    private float interactionDistance = 3f;

    [SerializeField, Tooltip("LayerMask para objetos interactuables")]
    private LayerMask interactableLayer;

    [SerializeField, Tooltip("Referencia al Player con AudioSources")]
    private GameObject player; // Asigna el Player desde el Inspector

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            InteractWithObject();
        }
    }

    private void InteractWithObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log($"Interacción con: {hit.collider.name}");

            PickupItem pickupItem = hit.collider.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                AudioSource[] playerAudioSources = player.GetComponents<AudioSource>();
                pickupItem.Pickup(
                    FindObjectOfType<KeyManager>(),
                    FindObjectOfType<FlashlightManager>(),
                    FindObjectOfType<InventoryManager>(),
                    playerAudioSources
                );
            }
            else
            {
                Debug.LogWarning("El objeto no tiene el script PickupItem asignado.");
            }
        }
        else
        {
            Debug.Log("No estás mirando ningún objeto interactuable.");
        }
    }
}