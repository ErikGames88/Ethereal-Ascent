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

    [SerializeField, Tooltip("Referencia al TextMeshPro del noveno slot (cráneos)")]
    private TextMeshProUGUI skullCounterText;

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
    private int skullCount = 0;

    [SerializeField, Tooltip("Lista de prefabs para los slots del inventario")]
    private List<GameObject> slotPrefabs = new List<GameObject>(9); 

    public List<GameObject> SlotPrefabs
    {
        get => slotPrefabs;
    }

    void Start()
    {
        // Validar que la lista de slots está asignada
        if (slots == null || slots.Count != 9)
        {
            Debug.LogError("La lista de slots no está correctamente configurada.");
            return;
        }

        // Inicializar el inventario
        InitializeInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int newSlotIndex = selectedSlotIndex - 1;
            if (newSlotIndex < 0)
            {
                newSlotIndex = slots.Count - 1;
            }
            OnSlotSelected(newSlotIndex);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            int newSlotIndex = selectedSlotIndex + 1;
            if (newSlotIndex >= slots.Count)
            {
                newSlotIndex = 0;
            }
            OnSlotSelected(newSlotIndex);
        }
    }

    private void InitializeInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Transform slotTransform = slots[i].transform;
            Transform slotTextTransform = slotTransform.Find("Slot Text");
            TextMeshProUGUI slotTextComponent = null;

            if (slotTextTransform != null)
            {
                slotTextComponent = slotTextTransform.GetComponent<TextMeshProUGUI>();
            }

            if (slotTextComponent != null)
            {
                slotTextComponent.gameObject.SetActive(false);
            }

            if (i >= slotPrefabs.Count)
            {
                slotPrefabs.Add(null);
            }
        }

        Transform slot9Transform = slots[8].transform;
        Transform skullCounterTransform = slot9Transform.Find("Skull Counter Text");
        skullCounterText = null;

        if (skullCounterTransform != null)
        {
            skullCounterText = skullCounterTransform.GetComponent<TextMeshProUGUI>();
        }

        if (skullCounterText != null)
        {
            skullCounterText.gameObject.SetActive(true);
        }

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
        for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponent<Image>();
            if (i == selectedSlotIndex)
            {
                slotImage.color = selectedSlotColor;
            }
            else
            {
                slotImage.color = defaultSlotColor;
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

        Image slotImage = slots[slotIndex].GetComponentInChildren<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.enabled = true;
        }

        TextMeshProUGUI slotTextComponent = slots[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            slotTextComponent.text = itemName;
        }

        // Asegurar que se asigna el prefab al slot correspondiente
        if (slotPrefabs != null && slotIndex < slotPrefabs.Count)
        {
            slotPrefabs[slotIndex] = prefab;
        }

        Debug.Log($"Objeto {itemName} asignado al Slot {slotIndex} con prefab {prefab?.name}.");
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

    public int FindFirstAvailableSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponentInChildren<Image>();
            if (slotImage != null)
            {
                Debug.Log($"Revisando Slot {i}: {(slotImage.sprite == null ? "Vacío" : "Ocupado")}");
            }

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
}
