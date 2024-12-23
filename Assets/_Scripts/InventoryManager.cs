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

    [SerializeField, Tooltip("Textos asociados a los Slots (ordenados)")]
    private List<GameObject> slotTexts; // Textos correspondientes a los slots

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

        if (slotTexts == null || slotTexts.Count != 9)
        {
            Debug.LogError("La lista de textos no está correctamente configurada.");
            return;
        }

        InitializeInventory();

        // Asegura que el Selected Border comience en el Slot 1
        selectedSlotIndex = 0;

        UpdateSelectedBorderPosition(); // Inicializa la posición correctamente
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // Mover hacia la izquierda
        {
            int newSlotIndex = selectedSlotIndex - 1; 
            if (newSlotIndex >= 0)
            {
                OnSlotSelected(newSlotIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.D)) // Mover hacia la derecha
        {
            int newSlotIndex = selectedSlotIndex + 1; 
            if (newSlotIndex < slots.Count)
            {
                OnSlotSelected(newSlotIndex);
            }
        }

        UpdateItemTextVisibility();
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

            // Configurar la opacidad inicial de los Slots
            Image slotImage = slots[i].GetComponent<Image>();
            if (slotImage != null)
            {
                if (i == 8) // Excepción para el Slot 9
                {
                    slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1f); // Opacidad 255
                }
                else
                {
                    slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 0.392f); // Opacidad 100
                }
            }

            // Desactiva todos los textos al inicio
            if (slotTexts[i] != null)
            {
                slotTexts[i].SetActive(false);
            }
        }

        Debug.Log("Seleccionando el Slot 1...");
        selectedSlotIndex = 0;
    }

    public void OnSlotSelected(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            HideAllTexts(); // Oculta textos si el Slot es inválido
            return;
        }

        selectedSlotIndex = slotIndex;
        Debug.Log($"Slot seleccionado: {slotIndex + 1}");
        UpdateSelectedBorderPosition();
        UpdateItemTextVisibility();
    }

    private void UpdateSelectedBorderPosition()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < slots.Count && selectedBorder != null)
        {
            RectTransform targetSlot = slots[selectedSlotIndex].GetComponent<RectTransform>();
            RectTransform borderRectTransform = selectedBorder.GetComponent<RectTransform>();

            if (targetSlot != null && borderRectTransform != null)
            {
                borderRectTransform.position = targetSlot.position + new Vector3(0f, 9.5f, 0f);
                borderRectTransform.anchorMin = targetSlot.anchorMin;
                borderRectTransform.anchorMax = targetSlot.anchorMax;
                borderRectTransform.pivot = targetSlot.pivot;
                borderRectTransform.sizeDelta = new Vector2(241f, 160f);
            }
        }
    }

    public void AssignItemToSlot(int slotIndex, string itemName, Sprite itemIcon, GameObject prefab, GameObject textObject)
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
            slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1f); // Opacidad máxima
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

        // Asigna el texto al Slot
        AssignTextToSlot(slotIndex, textObject);

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

    public void AssignTextToSlot(int slotIndex, GameObject textObject)
    {
        if (slotIndex < 0 || slotIndex >= slotTexts.Count)
        {
            Debug.LogWarning("Índice del slot para texto fuera de rango.");
            return;
        }

        slotTexts[slotIndex] = textObject;
        Debug.Log($"Texto asignado al Slot {slotIndex + 1}");
    }

    private void UpdateItemTextVisibility()
    {
        if (!InventoryToggle.isInventoryOpen) // Si el inventario está cerrado
        {
            Debug.Log("Inventario cerrado: no mostrar textos.");
            HideAllTexts();
            return;
        }

        if (selectedSlotIndex < 0 || selectedSlotIndex >= slots.Count)
        {
            Debug.LogWarning("Índice de slot seleccionado fuera de rango.");
            HideAllTexts();
            return;
        }

        // Ocultar todos los textos antes de mostrar el del Slot actual
        HideAllTexts();

        Button selectedSlot = slots[selectedSlotIndex];
        GameObject selectedPrefab = slotPrefabs[selectedSlotIndex];

        if (selectedPrefab != null)
        {
            string prefabName = selectedPrefab.name;

            if (prefabName == "Flashlight")
            {
                PositionText(slotTexts[selectedSlotIndex], selectedSlot.GetComponent<RectTransform>());
            }
            else if (prefabName == "Cathedral Key")
            {
                PositionText(slotTexts[selectedSlotIndex], selectedSlot.GetComponent<RectTransform>());
            }
        }

        if (selectedSlotIndex == 8) // Slot 9 (Cráneos)
        {
            PositionText(slotTexts[8], slots[8].GetComponent<RectTransform>());
        }
    }

    private void PositionText(GameObject textObject, RectTransform slotRect)
    {
        if (textObject == null || slotRect == null) return;

        textObject.SetActive(true); // Asegúrate de que el texto esté activo

        // Obtén el RectTransform del texto y ajusta su posición en relación al Slot
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        Vector3 slotWorldPosition = slotRect.position;

        // Mantén la posición configurada desde el Canvas y ajusta la posición horizontal
        textRect.position = new Vector3(slotWorldPosition.x, textRect.position.y, slotWorldPosition.z);
    }

    public void HideAllTexts()
    {
        foreach (GameObject text in slotTexts)
        {
            if (text != null)
            {
                text.SetActive(false);
                Debug.Log($"Texto {text.name} desactivado.");
            }
        }
        Debug.Log("Todos los textos del inventario han sido ocultados.");
    }
}
