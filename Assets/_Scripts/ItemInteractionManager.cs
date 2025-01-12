using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionManager : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    private InventoryManager inventoryManager;
    private FlashlightManager flashlightManager;
    

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        flashlightManager = FindObjectOfType<FlashlightManager>();
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
            return;
        }

        Button selectedSlot = inventoryManager.Slots[inventoryManager.SelectedSlotIndex];
        Image slotImage = selectedSlot.GetComponentInChildren<Image>();

        if (slotImage == null || slotImage.sprite == null)
        {
            return;
        }

        GameObject slotPrefab = inventoryManager.SlotPrefabs[inventoryManager.SelectedSlotIndex];
        if (slotPrefab == null)
        {
            return;
        }

        if (slotPrefab.name == "Cathedral Key")
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
            {
                DoorInteraction doorInteraction = hit.collider.GetComponentInParent<DoorInteraction>();
                if (doorInteraction != null)
                {
                    doorInteraction.Interact();
                    inventoryManager.RemoveItem(inventoryManager.SelectedSlotIndex); 
                    return;
                }
            }
        }
        else if (slotPrefab.GetComponent<PickupItem>().isFlashlight) 
        {
            flashlightManager.EquipFlashlight();
        }
    }
}
