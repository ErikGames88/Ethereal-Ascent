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

    [SerializeField, Tooltip("Texto para mostrar el objeto asignado al slot")]
    private TextMeshProUGUI slotText;

    [SerializeField, Tooltip("Color del slot seleccionado (configurable desde el Inspector)")]
    private Color selectedSlotColor;

    [SerializeField, Tooltip("Color del slot no seleccionado")]
    private Color defaultSlotColor = Color.white;

    private int selectedSlotIndex = 0; // Índice del slot actualmente seleccionado
    public int SelectedSlotIndex
    {
        get => selectedSlotIndex;
    }

    private bool isKeyCollected = false; // Verificar si la llave ha sido recogida
    private int keySlotIndex = -1;       // Índice dinámico donde se equipa la llave

    [SerializeField, Tooltip("Lista de prefabs para los slots del inventario")]
    private List<GameObject> slotPrefabs = new List<GameObject>(9);

    public List<GameObject> SlotPrefabs
    {
        get => slotPrefabs;
    }

    [SerializeField, Tooltip("Referencia al SkullCounter para manejar el contador de cráneos")]
    private SkullCounter skullCounter;

    void Start()
    {
        if (slots == null || slots.Count != 9)
        {
            Debug.LogError("La lista de slots no está correctamente configurada.");
            return;
        }

        InitializeInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // Mover hacia la izquierda
        {
            int newSlotIndex = Mathf.Max(selectedSlotIndex - 1, 0); // Limita al primer slot
            OnSlotSelected(newSlotIndex);
        }

        if (Input.GetKeyDown(KeyCode.D)) // Mover hacia la derecha
        {
            int newSlotIndex = Mathf.Min(selectedSlotIndex + 1, 7); // Limita al último slot interactuable
            OnSlotSelected(newSlotIndex);
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

            Transform slotBackgroundTransform = slots[i].transform.Find("Slot Background");
            if (slotBackgroundTransform == null)
            {
                Debug.LogError($"El Slot {i + 1} no tiene un hijo llamado 'Slot Background'.");
                continue;
            }

            Image slotBackground = slotBackgroundTransform.GetComponent<Image>();
            if (slotBackground == null)
            {
                Debug.LogError($"El 'Slot Background' del Slot {i + 1} no tiene un componente Image.");
                continue;
            }

            slotImage.sprite = null;
            slotImage.color = defaultSlotColor;

            Color bgColor = slotBackground.color;
            bgColor.a = 100f / 255f;
            slotBackground.color = bgColor;
        }

        Debug.Log("Seleccionando el Slot 1...");
        selectedSlotIndex = 0;
        UpdateSlotColors();
    }

    public void OnSlotSelected(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"Índice del slot {slotIndex} fuera de rango.");
            return;
        }

        selectedSlotIndex = slotIndex;
        UpdateSlotColors();
    }

    private void UpdateSlotColors()
    {
        Debug.Log($"Actualizando colores. Índice seleccionado: {selectedSlotIndex}");

        for (int i = 0; i < slots.Count; i++)
        {
            if (i == 8) // Ignorar el Slot 9 (índice 8)
            {
                Debug.Log($"Ignorando el Slot 9 (índice {i}).");
                continue;
            }

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

            Transform slotBackgroundTransform = slots[i].transform.Find("Slot Background");
            if (slotBackgroundTransform == null)
            {
                Debug.LogError($"El Slot {i + 1} no tiene un hijo llamado 'Slot Background'.");
                continue;
            }

            Image slotBackground = slotBackgroundTransform.GetComponent<Image>();
            if (slotBackground == null)
            {
                Debug.LogError($"El 'Slot Background' del Slot {i + 1} no tiene un componente Image.");
                continue;
            }

            if (i == selectedSlotIndex) // Slot seleccionado
            {
                if (slotImage.sprite != null) // Con objeto
                {
                    slotImage.color = new Color(0.849f, 0.230f, 0.845f, 1f); // Lila opacidad 255
                    slotBackground.color = new Color(1f, 1f, 1f, 0f); // Transparente
                    Debug.Log($"Slot {i + 1} seleccionado con objeto: Lila opacidad 255, fondo transparente.");
                }
                else // Sin objeto
                {
                    slotImage.color = new Color(0.849f, 0.230f, 0.845f, 0.8f); // Lila opacidad 204
                    slotBackground.color = new Color(1f, 1f, 1f, 0.392f); // Semitransparente
                    Debug.Log($"Slot {i + 1} seleccionado sin objeto: Lila opacidad 204, fondo semitransparente.");
                }
            }
            else // Slot no seleccionado
            {
                if (slotImage.sprite != null) // Con objeto
                {
                    slotImage.color = new Color(1f, 1f, 1f, 1f); // Blanco opacidad 255
                    slotBackground.color = new Color(1f, 1f, 1f, 0f); // Transparente
                    Debug.Log($"Slot {i + 1} no seleccionado con objeto: Blanco opacidad 255, fondo transparente.");
                }
                else // Sin objeto
                {
                    slotImage.color = defaultSlotColor; // Blanco opacidad 100
                    slotBackground.color = new Color(1f, 1f, 1f, 0.392f); // Semitransparente
                    Debug.Log($"Slot {i + 1} no seleccionado sin objeto: Blanco opacidad 100, fondo semitransparente.");
                }
            }
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
        Transform slotBackgroundTransform = slot.transform.Find("Slot Background");
        Image slotBackground = slotBackgroundTransform?.GetComponent<Image>();

        if (slotImage != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.color = selectedSlotColor; // Lila opacidad máxima
            slotImage.enabled = true;
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

        // Si no hay ningún slot seleccionado actualmente, seleccionamos el primero asignado
        if (selectedSlotIndex == -1 || !slots[selectedSlotIndex].GetComponent<Image>().sprite)
        {
            selectedSlotIndex = slotIndex;
            Debug.Log($"Seleccionando el Slot {slotIndex + 1} tras asignar objeto: {itemName}");
        }

        UpdateSlotColors();
    }

    public int FindFirstAvailableSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponentInChildren<Image>();
            if (slotImage != null && slotImage.sprite == null)
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

    public void AddSkullToCounter()
    {
        if (skullCounter != null)
        {
            skullCounter.AddSkull();
        }
        else
        {
            Debug.LogError("No se encontró el SkullCounter para añadir un cráneo.");
        }
    }
}