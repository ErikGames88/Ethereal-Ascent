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

    [SerializeField, Tooltip("Prefab para la linterna")]
    private GameObject flashlightPrefab;

    [SerializeField, Tooltip("Icono de la linterna")]
    private Sprite flashlightIcon;

    [SerializeField, Tooltip("Texto para mostrar el objeto asignado al slot")]
    private TextMeshProUGUI slotText;

    [SerializeField, Tooltip("Referencia al TextMeshPro del noveno slot (cráneos)")]
    private TextMeshProUGUI skullCounterText;

    [SerializeField, Tooltip("Color del slot seleccionado (configurable desde el Inspector)")]
    private Color selectedSlotColor; 

    [SerializeField, Tooltip("Color del slot no seleccionado")]
    private Color defaultSlotColor = Color.white;
    private bool isFlashlightEquipped = false; // Estado de la linterna

    private Light flashlightLight; // Referencia al Spot Light

    private GameObject equippedFlashlight; // Referencia al objeto de la linterna equipada

    private Vector3 flashlightLocalPosition = new Vector3(-0.922f, 0.031f, -0.031f);
    private Vector3 flashlightLocalRotation = new Vector3(0f, 85.27f, 90f);

    private int selectedSlotIndex = 0; // Índice del slot actualmente seleccionado
    public int SelectedSlotIndex
    {
        get => selectedSlotIndex;
    }

    private bool isKeyCollected = false; // Verificar si la llave ha sido recogida

    private int keySlotIndex = -1;       // Índice dinámico donde se equipa la llave

    private int skullCount = 0;

    void Start()
    {
        // Validar que el prefab de la linterna está asignado
        if (flashlightPrefab == null)
        {
            Debug.LogError("El prefab de la linterna no está asignado en el InventoryManager.");
            return;
        }

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
        Debug.Log($"InventoryManager Update ejecutándose. Inventario abierto: {InventoryToggle.isInventoryOpen}");
        // Registrar el estado del objeto y el script
        Debug.Log($"Estado del objeto: {gameObject.activeSelf}, Estado del script: {enabled}");

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

        // Alternar la linterna (equipar/desequipar) con E mientras estás en el Slot 1
        if (Input.GetKeyDown(KeyCode.E) && selectedSlotIndex == 0)
        {
            EquipFlashlight();
        }

        // Encender/Apagar la luz con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Tecla F presionada.");

            if (equippedFlashlight == null)
            {
                Debug.LogWarning("No hay linterna equipada. La tecla F no hará nada.");
                return;
            }

            Transform spotLightTransform = equippedFlashlight.transform.Find("Spot Light");
            if (spotLightTransform == null)
            {
                Debug.LogError("No se encontró el hijo 'Spot Light' en la linterna equipada.");
                return;
            }

            bool isSpotLightActive = spotLightTransform.gameObject.activeSelf;
            spotLightTransform.gameObject.SetActive(!isSpotLightActive);

            Debug.Log(isSpotLightActive ? "Luz apagada." : "Luz encendida.");
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

        // --- Controlar el Texto de la Linterna (Slot 1) ---
        Transform slot1Transform = slots[0].transform;
        Transform slot1TextTransform = slot1Transform.Find("Slot Text");

        if (slot1TextTransform != null)
        {
            TextMeshProUGUI slot1Text = slot1TextTransform.GetComponent<TextMeshProUGUI>();
            if (slot1Text != null)
            {
                slot1Text.gameObject.SetActive(slotIndex == 0); // Activar solo en Slot 1
            }
        }

        // --- Delegar lógica de la llave al KeyManager ---
        KeyManager keyManager = FindObjectOfType<KeyManager>();
        if (keyManager != null)
        {
            bool isKeyActive = keyManager.IsKeySelected(slotIndex);

            // Obtener el RectTransform del slot seleccionado
            RectTransform slotRect = slots[slotIndex].GetComponent<RectTransform>();

            // Pasar el estado y la referencia del slot al KeyManager
            keyManager.ActivateKeyText(isKeyActive, slotRect);
        }

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

        // Asignar el ícono al slot
        Image slotImage = slots[slotIndex].GetComponentInChildren<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.enabled = true; // Asegurarse de que el ícono esté visible
        }

        // Opcional: Asignar texto dinámico al slot (si es necesario)
        TextMeshProUGUI slotTextComponent = slots[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            slotTextComponent.text = itemName;
        }

        Debug.Log($"Objeto {itemName} asignado al Slot {slotIndex}.");
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

    private void EquipFlashlight()
    {
        // Encontrar el Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player'.");
            return;
        }

        Transform flashlightPoint = player.transform.Find("Main Camera/Flashlight Point");
        if (flashlightPoint == null)
        {
            Debug.LogError("No se encontró el Flashlight Point en el Player.");
            return;
        }

        // Verificar si ya hay una linterna equipada
        if (equippedFlashlight != null)
        {
            // DESEQUIPAR LA LINTERNA
            Destroy(equippedFlashlight);
            equippedFlashlight = null; // Resetear la referencia de la linterna
            flashlightLight = null; // Resetear la referencia de la luz
            Debug.Log("Linterna desequipada.");
            return;
        }

        // Instanciar la linterna como hija del Flashlight Point
        if (flashlightPrefab == null)
        {
            Debug.LogError("El prefab de la linterna no está asignado en el InventoryManager.");
            return;
        }

        GameObject flashlightInstance = Instantiate(flashlightPrefab, flashlightPoint);
        flashlightInstance.name = "Flashlight"; // Asegurar un nombre único

        // Asignar posición y rotación locales específicas
        flashlightInstance.transform.localPosition = flashlightLocalPosition;
        flashlightInstance.transform.localRotation = Quaternion.Euler(flashlightLocalRotation);

        // Buscar el hijo "Spot Light" directamente y capturar su componente Light
        Transform spotLightTransform = flashlightInstance.transform.Find("Spot Light");
        if (spotLightTransform != null)
        {
            flashlightLight = spotLightTransform.GetComponent<Light>();
            spotLightTransform.gameObject.SetActive(false); // Asegurar que el objeto completo está desactivado
        }

        if (flashlightLight == null)
        {
            Debug.LogError("No se encontró el componente Light en el hijo 'Spot Light' del prefab.");
            return;
        }

        // Guardar la referencia de la linterna equipada
        equippedFlashlight = flashlightInstance;

        Debug.Log("Linterna equipada correctamente. Spot Light desactivado.");
    }

    public bool AddItemToFirstAvailableSlot(Sprite itemIcon)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponentInChildren<Image>();
            if (slotImage != null && slotImage.sprite == null)
            {
                // Asignar el ícono de la llave al slot
                slotImage.sprite = itemIcon;
                slotImage.enabled = true;

                // Marcar que la llave ha sido recogida y guardar el slot asignado
                isKeyCollected = true;
                keySlotIndex = i;

                Debug.Log($"Llave recogida y añadida al Slot {i + 1}.");
                return true;
            }
        }

        Debug.LogWarning("No hay slots disponibles en el inventario.");
        return false;
    }

    private void ActivateSlotText(bool isActive, string itemName)
    {
        // Buscar el texto del Slot 1 (linterna)
        Transform slot1Transform = slots[0].transform; // Slot 1 es índice 0
        Transform slotTextTransform = slot1Transform.Find("Slot Text");

        if (slotTextTransform != null)
        {
            TextMeshProUGUI slotTextComponent = slotTextTransform.GetComponent<TextMeshProUGUI>();
            if (slotTextComponent != null)
            {
                slotTextComponent.gameObject.SetActive(isActive);
                Debug.Log(isActive ? $"{itemName} activado." : $"{itemName} desactivado.");
            }
        }
    }

    public void ForceSlotUpdate()
    {
        OnSlotSelected(selectedSlotIndex); // Fuerza una actualización del slot seleccionado
    }

    public bool IsKeySelected()
    {
        // Verificar si la llave ha sido recogida y está seleccionada
        return isKeyCollected && selectedSlotIndex == keySlotIndex;
    }

    public int FindFirstAvailableSlot()
    {
       for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponentInChildren<Image>();
            if (slotImage != null && slotImage.sprite == null)
            {
                return i; // Retorna el índice del primer slot disponible
            }
        }
        return -1; // No hay slots disponibles
    }
}
