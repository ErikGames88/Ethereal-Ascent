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
    private GameObject player; 
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
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
            if (hit.collider.CompareTag("Skull"))
            {
                PickupSkull pickupSkull = hit.collider.GetComponent<PickupSkull>();
                if (pickupSkull != null)
                {
                    pickupSkull.Pickup();
                    return;
                }
            }

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

                return;
            }
        }
    }
}