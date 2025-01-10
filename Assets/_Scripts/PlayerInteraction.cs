using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            // Interacción con la Cathedral Door
            if (hit.collider.name == "Cathedral Door") 
            {
                Debug.Log("Interacción con la Cathedral Door. Cambiando a estado de Victoria...");
                GameManager.Instance.ChangeState(GameManager.GameState.Victory);
                return;
            }

            // Interacción con un Skull
            if (hit.collider.CompareTag("Skull"))
            {
                PickupSkull pickupSkull = hit.collider.GetComponent<PickupSkull>();
                if (pickupSkull != null)
                {
                    pickupSkull.Pickup();
                    return;
                }
            }

            // Interacción con objetos recogibles
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

            // Interacción con la First Note
            if (hit.collider.name == "First Note")
            {
                Debug.Log("Interacción con First Note. Activando First Text...");
                FindObjectOfType<TextManager>().ShowFirstText(); // Activar el First Text
                return;
            }
        }
    }
}