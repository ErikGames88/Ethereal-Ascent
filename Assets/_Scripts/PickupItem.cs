using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab del objeto a recoger")]
    private GameObject itemPrefab;

    [SerializeField, Tooltip("Ícono del objeto a recoger")]
    private Sprite itemIcon;

    private ManageInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<ManageInventory>();
        if (inventory == null)
        {
            Debug.LogError("No se encontró el script ManageInventory en la escena.");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log($"Interacción detectada con el objeto: {name}");

        if (inventory != null)
        {
            bool added = inventory.AddItemToInventory(itemPrefab, itemIcon);

            if (added)
            {
                Debug.Log($"Recogido y añadido al inventario: {itemPrefab.name}");
                gameObject.SetActive(false); // Desactiva el objeto tras recogerlo
            }
            else
            {
                Debug.LogWarning("No se pudo añadir al inventario. Espacio insuficiente.");
            }
        }
    }
}
