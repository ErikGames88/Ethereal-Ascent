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
        if (slots == null || slots.Count != 9)
        {
            Debug.LogError("La lista de slots no está correctamente configurada.");
            return;
        }

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
            Image slotImage = slots[i].GetComponent<Image>();
            Transform slotBackgroundTransform = slots[i].transform.Find("Slot Background");
            Image slotBackground = slotBackgroundTransform?.GetComponent<Image>();

            if (slotImage != null)
            {
                slotImage.sprite = null;
                slotImage.color = defaultSlotColor;
            }

            if (slotBackground != null)
            {
                Color bgColor = slotBackground.color;
                bgColor.a = 100f / 255f;
                slotBackground.color = bgColor;
            }
        }

        if (skullCounterText != null)
        {
            skullCounterText.gameObject.SetActive(true);
            skullCounterText.text = "0"; // Inicializar contador de cráneos
        }

        UpdateSlotColors();
    }

    public void OnSlotSelected(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError($"\u00cdndice del slot {slotIndex} fuera de rango.");
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
            Transform slotBackgroundTransform = slots[i].transform.Find("Slot Background");
            Image slotBackground = slotBackgroundTransform != null ? slotBackgroundTransform.GetComponent<Image>() : null;

            if (slotImage != null)
            {
                if (i == selectedSlotIndex) // Slot seleccionado
                {
                    if (slotImage.sprite != null) // Con objeto
                    {
                        slotImage.color = new Color(0.849f, 0.230f, 0.845f, 1f); // Lila opacidad 255
                        if (slotBackground != null)
                        {
                            slotBackground.color = new Color(1f, 1f, 1f, 0f); // Transparente
                        }
                    }
                    else // Sin objeto
                    {
                        slotImage.color = new Color(0.849f, 0.230f, 0.845f, 0.8f); // Lila opacidad 204
                        if (slotBackground != null)
                        {
                            slotBackground.color = new Color(1f, 1f, 1f, 0.392f); // Semitransparente
                        }
                    }
                }
                else // Slot no seleccionado
                {
                    if (slotImage.sprite != null) // Con objeto
                    {
                        slotImage.color = new Color(1f, 1f, 1f, 1f); // Blanco opacidad 255
                        if (slotBackground != null)
                        {
                            slotBackground.color = new Color(1f, 1f, 1f, 0f); // Transparente
                        }
                    }
                    else // Sin objeto
                    {
                        slotImage.color = new Color(1f, 1f, 1f, 0.392f); // Blanco opacidad 100
                        if (slotBackground != null)
                        {
                            slotBackground.color = new Color(1f, 1f, 1f, 0.392f); // Semitransparente
                        }
                    }
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

        // Obtener el Slot principal
        GameObject slot = slots[slotIndex].gameObject;

        // Obtener el componente Image del Slot
        Image slotImage = slot.GetComponent<Image>();
        Transform slotBackgroundTransform = slot.transform.Find("Slot Background");
        Image slotBackground = null;

        if (slotBackgroundTransform != null)
        {
            slotBackground = slotBackgroundTransform.GetComponent<Image>();
        }

        if (slotImage != null)
        {
            // Configurar el sprite del objeto y el color del Slot
            slotImage.sprite = itemIcon;
            slotImage.color = selectedSlotColor; // Lila opacidad máxima
            slotImage.enabled = true;

            Debug.Log($"Slot {slotIndex} asignando objeto: {itemName} con sprite: {itemIcon?.name}");

            // Configurar el Slot Background
            if (slotBackground != null)
            {
                Color bgColor = slotBackground.color;
                bgColor.a = 0f; // Ocultar el fondo blanco al asignar objeto
                slotBackground.color = bgColor;
                Debug.Log($"Slot {slotIndex} Background oculto al asignar objeto | Color: {bgColor}");
            }
        }
        else
        {
            Debug.LogWarning($"Slot {slotIndex} no tiene un componente Image asignado.");
        }

        // Asignar texto al Slot si es necesario
        TextMeshProUGUI slotTextComponent = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            slotTextComponent.text = itemName;
        }

        // Asignar prefab al Slot correspondiente
        if (slotPrefabs != null && slotIndex < slotPrefabs.Count)
        {
            slotPrefabs[slotIndex] = prefab;
        }

        Debug.Log($"Objeto {itemName} asignado al Slot {slotIndex} con prefab {prefab?.name}.");

        // Actualizar colores inmediatamente después de asignar
        if (selectedSlotIndex == 0) // Asegurar que Slot 1 comienza seleccionado al abrir el inventario
        {
            selectedSlotIndex = 0;
        }
        else
        {
            selectedSlotIndex = slotIndex;
        }

        UpdateSlotColors();
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
            Debug.LogWarning($"\u00cdndice del slot {slotIndex} fuera de rango o no tiene prefab asociado.");
            return null;
        }

        return slotPrefabs[slotIndex];
    }
}
