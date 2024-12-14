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

    [SerializeField, Tooltip("Texto para mostrar el objeto asignado al slot")]
    private TextMeshProUGUI slotText;

    [SerializeField, Tooltip("Referencia al TextMeshPro del noveno slot (cráneos)")]
    private TextMeshProUGUI skullCounterText;

    private int selectedSlotIndex = 0; // Índice del slot actualmente seleccionado
    [SerializeField, Tooltip("Color del slot seleccionado")]
    private Color selectedColor = Color.yellow; // Cambiar según tu preferencia
    [SerializeField, Tooltip("Color del slot no seleccionado")]
    private Color defaultColor = Color.white; // Cambiar según tu preferencia

    private int skullCount = 0;

    void Start()
    {
        InitializeInventory();
    }

    void Update()
    {
        // Moverse a la izquierda con A
        if (Input.GetKeyDown(KeyCode.A))
        {
            int newSlotIndex = selectedSlotIndex - 1;
            if (newSlotIndex < 0) newSlotIndex = slots.Count - 1; // Ciclar al último slot
            OnSlotSelected(newSlotIndex);
        }

        // Moverse a la derecha con D
        if (Input.GetKeyDown(KeyCode.D))
        {
            int newSlotIndex = selectedSlotIndex + 1;
            if (newSlotIndex >= slots.Count) newSlotIndex = 0; // Ciclar al primer slot
            OnSlotSelected(newSlotIndex);
        }
    }

    private void InitializeInventory()
    {
        if (slots == null || slots.Count != 9)
        {
            Debug.LogError("Los slots no están correctamente asignados o faltan.");
            return;
        }

        if (flashlightIcon == null)
        {
            Debug.LogError("El icono de la linterna no está asignado en el Inspector.");
            return;
        }

        // Asignar la linterna al primer slot
        AssignItemToSlot(0, "Flashlight", flashlightIcon);

        // Configurar los textos dinámicos para cada slot
        for (int i = 0; i < slots.Count; i++)
        {
            Transform slotTransform = slots[i].transform;

            // Buscar solo hijos inmediatos
            TextMeshProUGUI slotTextComponent = slotTransform.Find("Slot Text")?.GetComponent<TextMeshProUGUI>();
            if (slotTextComponent != null)
            {
                slotTextComponent.gameObject.SetActive(i == 0); // Activar solo el texto del Slot 1 al inicio
                Debug.Log($"Slot {i}: Texto {(i == 0 ? "Activado" : "Desactivado")}.");
            }
            else
            {
                Debug.LogWarning($"No se encontró 'Slot Text' en el Slot {i}.");
            }
        }

        // Configurar el Skull Counter Text para el Slot 9
        Transform slot9Transform = slots[8].transform;
        skullCounterText = slot9Transform.Find("Skull Counter Text")?.GetComponent<TextMeshProUGUI>();
        if (skullCounterText != null)
        {
            skullCounterText.gameObject.SetActive(true); // Siempre visible en el Slot 9
            Debug.Log("El Skull Counter Text ha sido activado correctamente.");
        }
        else
        {
            Debug.LogError("No se encontró 'Skull Counter Text' en el Slot 9.");
        }

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

        // Buscar el texto del Slot como hijo del slot correspondiente
        TextMeshProUGUI slotTextComponent = slots[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            // No sobrescribas el texto existente, respétalo
            slotTextComponent.text = slotTextComponent.text; // Se mantiene el texto configurado en el Inspector
        }

        // Asignar el icono al slot
        Image slotImage = slots[slotIndex].GetComponentInChildren<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.enabled = true; // Asegurarse de que el icono esté visible
        }
    }

    public void UpdateSlotText(int slotIndex)
    {
        Debug.Log($"Actualizando texto del Slot {slotIndex}.");

        for (int i = 0; i < slots.Count; i++)
        {
            Transform slotTransform = slots[i].transform;

            // Buscar solo hijos inmediatos
            TextMeshProUGUI slotTextComponent = slotTransform.Find("Slot Text")?.GetComponent<TextMeshProUGUI>();
            if (slotTextComponent != null)
            {
                bool shouldActivate = (i == slotIndex && i != 8); // Activar solo el texto del slot seleccionado, excepto el Slot 9
                slotTextComponent.gameObject.SetActive(shouldActivate);
                Debug.Log($"Slot {i}: Texto {(shouldActivate ? "Activado" : "Desactivado")}.");
            }
            else
            {
                Debug.LogWarning($"No se encontró 'Slot Text' en el Slot {i}.");
            }
        }
    }

    public void OnSlotSelected(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            return;
        }

        Debug.Log($"Slot seleccionado: {slotIndex}.");

        // Actualizar la apariencia visual de los slots
        for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponent<Image>();
            if (slotImage != null)
            {
                bool isSelected = (i == slotIndex);
                slotImage.color = isSelected ? selectedColor : defaultColor;
                Debug.Log($"Slot {i}: {(isSelected ? "Seleccionado" : "No seleccionado")}.");
            }
            else
            {
                Debug.LogWarning($"No se encontró Image en el Slot {i}.");
            }
        }

        // Actualizar el texto dinámico del slot seleccionado
        UpdateSlotText(slotIndex);

        // Guardar el índice del slot seleccionado
        selectedSlotIndex = slotIndex;
    }

    private string GetItemKeyFromSlot(int slotIndex)
    {
        // Aquí deberías obtener el objeto real del inventario en función del índice del slot
        // Esto es un ejemplo simplificado
        if (slotIndex == 0)
        {
            return "Flashlight";
        }
        else if (slotIndex == 8)
        {
            return "Skulls";
        }

        return ""; // Devuelve vacío si el slot está vacío
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
            skullCounterText.text = skullCount.ToString(); // Actualizar el contador de cráneos
            skullCounterText.gameObject.SetActive(true); // Siempre visible
        }
    }

    
}
