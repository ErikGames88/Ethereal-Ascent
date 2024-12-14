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

    [SerializeField, Tooltip("Icono de la linterna")]
    private Sprite flashlightIcon;

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

        // Cargar el sprite de la linterna desde los recursos o asignarlo manualmente
        Sprite flashlightIcon = Resources.Load<Sprite>("UI/Flashlight_icon"); // Cambia la ruta según tu organización
        if (flashlightIcon == null)
        {
            Debug.LogError("El icono de la linterna no se encontró en la ruta especificada.");
            return;
        }

        // Asignar la linterna al primer slot
        AssignItemToSlot(0, "Linterna", flashlightIcon);

        // Inicializar el contador de cráneos
        UpdateSkullCounter();
    }

    public void AssignItemToSlot(int slotIndex, string itemName, Sprite itemIcon)
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

        // Asignar el icono al slot
        Image slotImage = slots[slotIndex].GetComponentInChildren<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.enabled = true; // Asegurarse de que el icono esté visible
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
