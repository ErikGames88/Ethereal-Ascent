using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField, Tooltip("Array de todos los slots del inventario")]
    private Slot[] inventorySlots;

    [SerializeField, Tooltip("Icono de la linterna")]
    private Sprite flashlightIcon;

    public void AddItemToFirstSlot(GameObject item, Sprite icon)
    {
        if (inventorySlots.Length > 0)
        {
            inventorySlots[0].StoreItem(item, icon); 
            Debug.Log("Linterna añadida al inventario.");
        }
    }
}
