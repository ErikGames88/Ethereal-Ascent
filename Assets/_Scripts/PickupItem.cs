using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab del objeto a recoger")]
    private GameObject itemPrefab;

    [SerializeField, Tooltip("Sprite del icono del objeto")]
    private Sprite itemIcon;

    private ManageInventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<ManageInventory>();
        if (inventory == null)
        {
            Debug.LogError("No se encontró el script ManageInventory en la escena.");
        }
    }

    private void OnMouseDown()
    {
        if (inventory != null)
        {
            bool added = inventory.AddItemToInventory(itemPrefab, itemIcon);
            if (added)
            {
                Debug.Log($"{gameObject.name} recogido y añadido al inventario.");
                Destroy(gameObject); // Destruye el objeto tras recogerlo
            }
            else
            {
                Debug.LogWarning("El inventario está lleno. No se pudo recoger el objeto.");
            }
        }
    }
}
