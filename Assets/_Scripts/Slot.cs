using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField, Tooltip("Icono del objeto en el slot")]
    private Image itemIcon;

    [SerializeField, Tooltip("Objeto almacenado en el slot")]
    private GameObject storedItem;

    public void StoreItem(GameObject item, Sprite icon)
    {
        storedItem = item; // Almacena el prefab
        itemIcon.sprite = icon; // Asigna el sprite del icono
        itemIcon.enabled = true; // Activa el icono visualmente
    }

    public void ClearSlot()
    {
        storedItem = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false; // Desactiva el icono visualmente
    }

    public GameObject GetStoredItem()
    {
        return storedItem;
    }
}
