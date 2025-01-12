using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private GameObject itemText;
    [SerializeField] private AudioClip pickupSound;
    
    public bool isFlashlight = false;



    public void Pickup(KeyManager keyManager, FlashlightManager flashlightManager, InventoryManager inventoryManager, AudioSource[] playerAudioSources)
    {
        if (itemIcon == null)
        {
            return;
        }

        int availableSlotIndex = inventoryManager.FindFirstAvailableSlot();
        if (availableSlotIndex < 0)
        {
            return;
        }

        if (isFlashlight)
        {
            flashlightManager.CollectFlashlight(availableSlotIndex, itemIcon);
        }
        else
        {
            keyManager.CollectKey(inventoryManager, availableSlotIndex, itemIcon);
        }

        inventoryManager.AssignItemToSlot(availableSlotIndex, gameObject.name, itemIcon, gameObject, itemText);

        PlayPickupSound(playerAudioSources);

        Transform particleEffect = transform.Find("Item Particle Effect");
        if (particleEffect != null)
        {
            particleEffect.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    private void PlayPickupSound(AudioSource[] playerAudioSources)
    {
        if (pickupSound != null && playerAudioSources.Length > 1)
        {
            playerAudioSources[1].PlayOneShot(pickupSound);
        }
    }
}