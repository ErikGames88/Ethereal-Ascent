using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField, Tooltip("Ícono del objeto")]
    private Sprite itemIcon;

    [Tooltip("¿Es una linterna?")]
    public bool isFlashlight = false;

    [SerializeField, Tooltip("Texto del objeto para el inventario")]
    private GameObject itemText;

    [SerializeField, Tooltip("Sonido al recoger el objeto")]
    private AudioClip pickupSound;

    public void Pickup(KeyManager keyManager, FlashlightManager flashlightManager, InventoryManager inventoryManager, AudioSource[] playerAudioSources)
    {
        if (itemIcon == null)
        {
            Debug.LogError("El ícono del objeto no está asignado. Revisa el Inspector.");
            return;
        }

        int availableSlotIndex = inventoryManager.FindFirstAvailableSlot();
        if (availableSlotIndex < 0)
        {
            Debug.LogWarning("No hay slots disponibles en el inventario.");
            return;
        }

        Debug.Log($"Objeto recogido: {gameObject.name}. Asignando al Slot {availableSlotIndex}");

        if (isFlashlight)
        {
            flashlightManager.CollectFlashlight(availableSlotIndex, itemIcon);
        }
        else
        {
            keyManager.CollectKey(inventoryManager, availableSlotIndex, itemIcon);
        }

        inventoryManager.AssignItemToSlot(availableSlotIndex, gameObject.name, itemIcon, gameObject, itemText);

        // Reproduce el sonido de recogida usando el segundo AudioSource
        PlayPickupSound(playerAudioSources);

        // Desactiva el objeto en la escena tras recogerlo
        gameObject.SetActive(false);
    }

    private void PlayPickupSound(AudioSource[] playerAudioSources)
    {
        if (pickupSound != null && playerAudioSources.Length > 1)
        {
            playerAudioSources[1].PlayOneShot(pickupSound);
            Debug.Log("Sonido de recogida reproducido.");
        }
        else
        {
            Debug.LogWarning("No se asignó un sonido de recogida o el segundo AudioSource no está configurado.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger activado: Player ha tocado la llave.");
        }
    }
}