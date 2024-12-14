using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    /*[SerializeField, Tooltip("Referencia al InventoryManager para manejar el inventario")]
    private InventoryManager inventoryManager;

    [SerializeField, Tooltip("Icono del objeto para añadirlo al inventario")]
    private Sprite itemIcon;

    [SerializeField, Tooltip("Slot reservado en el inventario (opcional, usa -1 para cualquier slot disponible)")]
    private int reservedSlot = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventoryManager != null)
            {
                // Añadir el objeto al inventario
                inventoryManager.AddItemToInventory(null, itemIcon, reservedSlot);
                Debug.Log("Objeto recogido y añadido al inventario.");

                // Desactivar el objeto en la escena
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("No se asignó el InventoryManager en el PickupItem.");
            }
        }
    }*/
}
