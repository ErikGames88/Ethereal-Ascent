using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField, Tooltip("Lista de botones para los slots del inventario")]
    private List<Button> slots;

    [SerializeField, Tooltip("Prefab para la linterna")]
    private GameObject flashlightPrefab;

    [SerializeField, Tooltip("Referencia al TextMeshPro del noveno slot (cráneos)")]
    private TextMeshProUGUI skullCounterText;

    private int skullCount = 0;

    void Start()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        if (slots == null || slots.Count != 9)
        {
            Debug.LogError("Debes asignar los 9 slots del inventario en el inspector.");
            return;
        }

        // Asignar la linterna al primer slot
        AssignItemToSlot(0, "Linterna");

        // Inicializar el contador de cráneos
        UpdateSkullCounter();
    }

    public void AssignItemToSlot(int slotIndex, string itemName)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            return;
        }

        // Cambiar el texto del slot para reflejar el objeto
        TextMeshProUGUI slotText = slots[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (slotText != null)
        {
            slotText.text = itemName;
        }
    }

    public void AddSkull()
    {
        skullCount++;
        UpdateSkullCounter();
    }

    private void UpdateSkullCounter()
    {
        if (skullCounterText != null)
        {
            skullCounterText.text = skullCount.ToString();
        }
    }
}
