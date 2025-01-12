using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject player;


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
            if (hit.collider.name == "Cathedral Door")
            {
                GameManager.Instance.ChangeState(GameManager.GameState.Victory);
                return;
            }

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

            if (hit.collider.name == "First Note")
            {
                FindObjectOfType<TextManager>().ShowFirstText();
                return;
            }

            if (hit.collider.name == "Cathedral Board")
            {
                FindObjectOfType<TextManager>().ShowCathedralBoardText();
                return;
            }

            if (hit.collider.name == "Hospital Door")
            {
                FindObjectOfType<TextManager>().ShowHospitalDoorText();
                return;
            }
        }
    }
}