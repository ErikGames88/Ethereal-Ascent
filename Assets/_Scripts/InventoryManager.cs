using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField, Tooltip("Lista de botones para los slots del inventario")]
    private List<Button> slots;

    public List<Button> Slots
    {
        get => slots;
    }

    [SerializeField, Tooltip("Lista de prefabs para los slots del inventario")]
    private List<GameObject> slotPrefabs = new List<GameObject>(9);

    public List<GameObject> SlotPrefabs
    {
        get => slotPrefabs;
    }

    [SerializeField, Tooltip("Referencia al borde de selección")]
    private RectTransform selectedBorder;

    private int selectedSlotIndex = -1; // Ningún Slot seleccionado inicialmente

    public int SelectedSlotIndex
    {
        get => selectedSlotIndex;
        set => selectedSlotIndex = value;
    }

    void Start()
    {
        if (slots == null || slots.Count != 9)
        {
            Debug.LogError("La lista de slots no está correctamente configurada.");
            return;
        }

        InitializeInventory();

        // Asegura que el Selected Border comience en el Slot 1
        selectedSlotIndex = 0;

        // Establece el tamaño inicial desde el editor
        RectTransform borderRectTransform = selectedBorder.GetComponent<RectTransform>();
        if (borderRectTransform != null)
        {
            borderRectTransform.sizeDelta = new Vector2(241f, 160f); // Tamaño configurado en el editor
        }

        UpdateSelectedBorderPosition(); // Inicializa la posición correctamente
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // Mover hacia la izquierda
        {
            int newSlotIndex = selectedSlotIndex - 1; // Permitir desplazamiento sin límite
            if (newSlotIndex >= 0) // Validar que no salga de rango
            {
                OnSlotSelected(newSlotIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.D)) // Mover hacia la derecha
        {
            int newSlotIndex = selectedSlotIndex + 1; // Permitir desplazamiento sin límite
            if (newSlotIndex < slots.Count) // Validar que no salga de rango
            {
                OnSlotSelected(newSlotIndex);
            }
        }
    }

    private void InitializeInventory()
    {
        Debug.Log("Inicializando inventario...");

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogError($"El Slot {i + 1} está vacío en la lista.");
                continue;
            }

            Image slotImage = slots[i].GetComponent<Image>();
            if (slotImage == null)
            {
                Debug.LogError($"El Slot {i + 1} no tiene un componente Image.");
                continue;
            }

            // Configura la opacidad inicial de todos los slots a 100
            slotImage.color = new Color(1f, 1f, 1f, 0.392f); // Blanco opacidad 100
        }

        Debug.Log("Seleccionando el Slot 1...");
        selectedSlotIndex = 0;
    }

    public void OnSlotSelected(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            return;
        }

        selectedSlotIndex = slotIndex;
        Debug.Log($"Slot seleccionado: {slotIndex + 1}");
        UpdateSelectedBorderPosition();
    }

    private void UpdateSelectedBorderPosition()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < slots.Count && selectedBorder != null)
        {
            // Obtén el RectTransform del Slot seleccionado
            RectTransform targetSlot = slots[selectedSlotIndex].GetComponent<RectTransform>();
            RectTransform borderRectTransform = selectedBorder.GetComponent<RectTransform>();

            if (targetSlot != null && borderRectTransform != null)
            {
                // Sincroniza la posición con un ajuste fino
                borderRectTransform.position = targetSlot.position + new Vector3(0f, 9.5f, 0f); // Ajuste más preciso
                borderRectTransform.anchorMin = targetSlot.anchorMin;
                borderRectTransform.anchorMax = targetSlot.anchorMax;
                borderRectTransform.pivot = targetSlot.pivot;

                // Mantén el tamaño configurado manualmente
                borderRectTransform.sizeDelta = new Vector2(241f, 160f);
            }
            else
            {
                Debug.LogWarning($"El Slot seleccionado o el Selected Border no tienen RectTransform válido. Índice: {selectedSlotIndex}");
            }
        }
        else
        {
            Debug.LogWarning("Selected Border o el Slot seleccionado no son válidos.");
        }
    }

    public void AssignItemToSlot(int slotIndex, string itemName, Sprite itemIcon, GameObject prefab)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            return;
        }

        GameObject slot = slots[slotIndex].gameObject;

        Image slotImage = slot.GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.enabled = true;

            // Cambiar opacidad a 255 cuando el slot tiene un sprite
            Color slotColor = slotImage.color;
            slotColor.a = 1f; // Opacidad máxima
            slotImage.color = slotColor;
        }

        TextMeshProUGUI slotTextComponent = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            slotTextComponent.text = itemName;
        }

        if (slotPrefabs != null && slotIndex < slotPrefabs.Count)
        {
            slotPrefabs[slotIndex] = prefab;
        }

        Debug.Log($"Asignado {itemName} al Slot {slotIndex + 1}");
    }

    public int FindFirstAvailableSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Button slotButton = slots[i];
            if (slotButton == null)
            {
                Debug.Log($"Slot {i} no tiene botón asignado en la lista.");
                continue;
            }

            Image slotImage = slotButton.GetComponent<Image>();
            if (slotImage == null)
            {
                Debug.Log($"Slot {i} no tiene un componente Image.");
                continue;
            }

            // Si el sprite del slot es nulo, el slot está disponible
            if (slotImage.sprite == null)
            {
                Debug.Log($"Primer slot disponible encontrado: {i}");
                return i;
            }
        }

        Debug.LogWarning("No hay slots disponibles en el inventario.");
        return -1;
    }

    public GameObject GetPrefabFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotPrefabs.Count)
        {
            Debug.LogWarning($"Índice del slot {slotIndex} fuera de rango o no tiene prefab asociado.");
            return null;
        }

        return slotPrefabs[slotIndex];
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            return;
        }

        // Limpia el sprite del Slot
        Image slotImage = slots[slotIndex].GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = null; // Eliminar sprite
            slotImage.enabled = true; // Asegúrate de que el Image esté visible
            slotImage.color = new Color(1f, 1f, 1f, 0.392f); // Vuelve al color blanco opacidad 100
        }

        // Limpia el texto del Slot (si aplica)
        TextMeshProUGUI slotTextComponent = slots[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            slotTextComponent.text = ""; // Limpia el texto
            slotTextComponent.gameObject.SetActive(false); // Desactiva el texto si está activo
        }

        // Limpia el prefab asociado al Slot
        if (slotPrefabs != null && slotIndex < slotPrefabs.Count)
        {
            slotPrefabs[slotIndex] = null;
        }

        Debug.Log($"Objeto eliminado del Slot {slotIndex + 1}.");
    }
}
