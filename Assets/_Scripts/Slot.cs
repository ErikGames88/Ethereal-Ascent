using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField, Tooltip("Icono del objeto en el slot")]
    private Image itemIcon; // Imagen del objeto dentro del slot

    [SerializeField, Tooltip("Fondo del slot (componente Image)")]
    private Image slotImage; // Referencia al componente Image del Slot

    [SerializeField, Tooltip("Objeto almacenado en el slot")]
    private GameObject storedItem;

    [SerializeField, Tooltip("Nombre del objeto almacenado en el slot")]
    private string storedItemName;

    private void Awake()
    {
        // Si el ItemIcon no está asignado en el Inspector, intenta buscarlo en los hijos
        if (itemIcon == null)
        {
            itemIcon = GetComponentInChildren<Image>();
            if (itemIcon == null)
            {
                Debug.LogError($"Error: El Slot {name} no tiene un 'ItemIcon' asignado.");
            }
        }

        // Busca y asegura que el slotImage está asignado
        if (slotImage == null)
        {
            slotImage = GetComponent<Image>();
            if (slotImage == null)
            {
                Debug.LogError($"Error: El Slot {name} no tiene un componente 'Image' asignado.");
            }
        }

        // Asegúrate de que el fondo (slotImage) esté siempre activo
        if (slotImage != null)
        {
            slotImage.enabled = true;
        }

        // Asegúrate de que el ícono esté desactivado inicialmente
        if (itemIcon != null)
        {
            itemIcon.enabled = false;
        }
    }

    public void StoreItem(GameObject item, Sprite icon)
    {
        storedItem = item;
        storedItemName = item.name; // Guarda el nombre del objeto almacenado

        if (itemIcon != null)
        {
            itemIcon.sprite = icon; // Asigna el sprite del ícono
            itemIcon.enabled = true; // Activa el ícono
            Debug.Log($"Slot actualizado: {name} con objeto: {storedItemName}, Ícono asignado.");
        }
        else
        {
            Debug.LogError($"Error: Slot {name} no tiene un 'ItemIcon' asignado.");
        }
    }

    public void ClearSlot()
    {
        // Limpia el contenido del slot, pero deja el fondo intacto
        storedItem = null;
        storedItemName = null;

        if (itemIcon != null)
        {
            itemIcon.sprite = null; // Limpia el ícono
            itemIcon.enabled = false; // Desactiva solo el ícono
        }

        if (slotImage != null)
        {
            slotImage.enabled = true; // Asegúrate de que el fondo siga activo
        }

        Debug.Log($"Slot limpiado: {name}");
    }

    public GameObject GetStoredItem()
    {
        return storedItem;
    }

    public string GetStoredItemName()
    {
        return storedItemName; // Devuelve el nombre almacenado
    }
}
