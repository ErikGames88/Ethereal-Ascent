using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField, Tooltip("Ícono del objeto")]
    private Sprite itemIcon;

    public void Pickup(KeyManager keyManager, InventoryManager inventoryManager)
    {
        if (keyManager != null && inventoryManager != null)
        {
            int availableSlotIndex = inventoryManager.FindFirstAvailableSlot();
            if (availableSlotIndex >= 0)
            {
                keyManager.CollectKey(inventoryManager, availableSlotIndex, itemIcon); // Ajustamos con el nuevo parámetro
                Debug.Log($"Objeto {itemIcon.name} recogido.");
                gameObject.SetActive(false); // Desactivar el objeto en la escena
            }
            else
            {
                Debug.LogWarning("No hay slots disponibles en el inventario para este objeto.");
            }
        }
        else
        {
            Debug.LogError("KeyManager o InventoryManager no está asignado.");
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
