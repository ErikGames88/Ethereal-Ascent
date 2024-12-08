using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateSlotImages : MonoBehaviour
{
    [SerializeField, Tooltip("Array de todos los slots del inventario")]
    private Slot[] inventorySlots;

    void Start()
    {
        foreach (Slot slot in inventorySlots)
        {
            Image slotImage = slot.GetComponent<Image>();
            if (slotImage != null)
            {
                slotImage.enabled = true; // Asegura que el componente Image esté activo
                Debug.Log($"Slot {slot.name}: Componente Image activado.");
            }
            else
            {
                Debug.LogWarning($"Slot {slot.name} no tiene un componente Image.");
            }
        }
    }
}
