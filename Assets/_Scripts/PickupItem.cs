using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField, Tooltip("Ícono del objeto")]
    private Sprite itemIcon;

    [SerializeField, Tooltip("¿Es una linterna?")]
    private bool isFlashlight = false;

    public void Pickup(KeyManager keyManager, FlashlightManager flashlightManager, InventoryManager inventoryManager)
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
            Debug.Log("Es una linterna. Usando FlashlightManager.");
            flashlightManager.CollectFlashlight(availableSlotIndex, itemIcon);
        }
        else
        {
            Debug.Log("Es una llave. Usando KeyManager.");
            keyManager.CollectKey(inventoryManager, availableSlotIndex, itemIcon);
        }

        inventoryManager.AssignItemToSlot(availableSlotIndex, gameObject.name, itemIcon);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger activado: Player ha tocado la llave.");
        }
    }
}
