using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField, Tooltip("Ícono del objeto")]
    private Sprite itemIcon;

    [SerializeField, Tooltip("Referencia al InventoryManager")]
    private InventoryManager inventoryManager;

    public void Pickup(InventoryManager inventoryManager)
    {
        if (inventoryManager != null)
        {
            // Añadir el objeto al inventario
            bool itemAdded = inventoryManager.AddItemToFirstAvailableSlot(itemIcon);

            if (itemAdded)
            {
                Debug.Log("Objeto recogido y añadido al inventario.");
                gameObject.SetActive(false); // Desactivar el objeto en escena
            }
            else
            {
                Debug.LogWarning("No hay espacio en el inventario.");
            }
        }
        else
        {
            Debug.LogError("InventoryManager no está asignado.");
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
