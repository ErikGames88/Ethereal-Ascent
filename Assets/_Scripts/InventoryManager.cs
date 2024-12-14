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

    [SerializeField, Tooltip("Icono de las calaveras para el Slot 9")]
    private Sprite skullIcon;

    [SerializeField, Tooltip("Texto para mostrar el objeto asignado al slot")]
    private TextMeshProUGUI slotText;

    [SerializeField, Tooltip("Referencia al TextMeshPro del noveno slot (cráneos)")]
    private TextMeshProUGUI skullCounterText;

    [SerializeField, Tooltip("Color del slot seleccionado (configurable desde el Inspector)")]
    private Color selectedSlotColor; 

    [SerializeField, Tooltip("Color del slot no seleccionado")]
    private Color defaultSlotColor;

    private int selectedSlotIndex = 0; // Índice del slot actualmente seleccionado
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

        if (skullIcon == null)
        {
            Debug.LogError("El icono de las calaveras no está asignado en el Inspector.");
            return;
        }

        // Asignar la linterna al primer slot
        AssignItemToSlot(0, "Flashlight", flashlightIcon);

        // Asignar la calavera al Slot 9
        AssignItemToSlot(8, "Skulls", skullIcon);

        // Configurar los textos dinámicos para cada slot
        for (int i = 0; i < slots.Count; i++)
        {
            Transform slotTransform = slots[i].transform;

            // Buscar solo hijos inmediatos
            Transform slotTextTransform = slotTransform.Find("Slot Text");
            TextMeshProUGUI slotTextComponent = null;

            if (slotTextTransform != null)
            {
                slotTextComponent = slotTextTransform.GetComponent<TextMeshProUGUI>();
            }

            if (slotTextComponent != null)
            {
                slotTextComponent.gameObject.SetActive(i == 0); // Activar solo el texto del Slot 1 al inicio
            }
            else
            {
                Debug.LogWarning($"No se encontró 'Slot Text' en el Slot {i}.");
            }
        }

        // Configurar el Skull Counter Text para el Slot 9
        Transform slot9Transform = slots[8].transform;
        Transform skullCounterTransform = slot9Transform.Find("Skull Counter Text");

        if (skullCounterTransform != null)
        {
            skullCounterText = skullCounterTransform.GetComponent<TextMeshProUGUI>();
        }

        if (skullCounterText != null)
        {
            skullCounterText.gameObject.SetActive(true); // Siempre visible en el Slot 9
        }
        else
        {
            Debug.LogError("No se encontró 'Skull Counter Text' en el Slot 9.");
        }

        // Inicializar el contador de cráneos
        UpdateSkullCounter();

        // Actualizar el color inicial de los slots
        UpdateSlotColors();
    }

    public void OnSlotSelected(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            return;
        }

        Debug.Log($"Slot seleccionado: {slotIndex}.");

        // Actualizar el texto dinámico del slot seleccionado
        UpdateSlotText(slotIndex);

        // Guardar el índice del slot seleccionado
        selectedSlotIndex = slotIndex;

        // Actualizar los colores de los slots
        UpdateSlotColors();
    }

    private void UpdateSlotColors()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponent<Image>();
            if (slotImage != null)
            {
                if (i == selectedSlotIndex)
                {
                    slotImage.color = selectedSlotColor; // Aplicar el color del slot seleccionado
                }
                else
                {
                    slotImage.color = defaultSlotColor; // Restaurar el color predeterminado
                }
            }
        }
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
            slotTextComponent.text = slotTextComponent.text; // Respetar el texto configurado en el Inspector
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
            skullCounterText.text = skullCount.ToString(); // Actualizar el contador de cráneos
            skullCounterText.gameObject.SetActive(true); // Siempre visible
        }
    }

    private void UpdateSlotText(int slotIndex)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Transform slotTransform = slots[i].transform;

            // Buscar solo hijos inmediatos
            Transform slotTextTransform = slotTransform.Find("Slot Text");
            TextMeshProUGUI slotTextComponent = null;

            if (slotTextTransform != null)
            {
                slotTextComponent = slotTextTransform.GetComponent<TextMeshProUGUI>();
            }

            if (slotTextComponent != null)
            {
                slotTextComponent.gameObject.SetActive(i == slotIndex && i != 8); // Activar solo el texto del slot seleccionado, excepto el Slot 9
            }
        }
    }
}
