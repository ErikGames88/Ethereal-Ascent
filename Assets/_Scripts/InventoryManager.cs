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

    [SerializeField, Tooltip("Color del slot seleccionado (configurable desde el Inspector)")]
    private Color selectedSlotColor = new Color(1f, 1f, 0f, 0.5f); // Amarillo con transparencia

    [SerializeField, Tooltip("Color del slot no seleccionado")]
    private Color defaultSlotColor = Color.white;

    private int selectedSlotIndex = 0; // Índice del slot actualmente seleccionado
    private bool isFlashlightEquipped = false; // Estado de la linterna

    private Light flashlightLight; // Referencia al Spot Light

    private GameObject equippedFlashlight; // Referencia al objeto de la linterna equipada

    private Vector3 flashlightLocalPosition = new Vector3(-0.922f, 0.031f, -0.031f);
    private Vector3 flashlightLocalRotation = new Vector3(0f, 85.27f, 90f);

    [SerializeField, Tooltip("Texto del objeto para el Slot 2 (llave)")]
    private GameObject cathedralKeyText;
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

        // Actualizar el texto dinámico del slot seleccionado
        UpdateSlotText(slotIndex);

        // Controlar la visibilidad del texto de la llave
        if (slotIndex == 1) // Slot 2 (índice 1)
        {
            ActivateObjectText(true); // Mostrar el texto de la llave
        }
        else
        {
            ActivateObjectText(false); // Ocultar el texto de la llave
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
            // Verificar si el slot está vacío
            Image slotImage = slots[i].GetComponentInChildren<Image>();
            if (slotImage != null && slotImage.sprite == null)
            {
                // Asignar el ícono al slot
                slotImage.sprite = itemIcon;
                slotImage.enabled = true;

                Debug.Log($"Objeto añadido al Slot {i + 1}");
                return true; // Objeto añadido correctamente
            }
        }

        Debug.LogWarning("No hay slots disponibles en el inventario.");
        return false; // No hay espacio disponible
    }

    private void ActivateObjectText(bool isActive)
    {
        if (cathedralKeyText != null)
        {
            cathedralKeyText.SetActive(isActive);

            if (isActive)
            {
                // Centrar el texto en el Slot correspondiente (Slot de la llave)
                RectTransform slotRect = slots[1].GetComponent<RectTransform>(); // Índice dinámico si necesario
                RectTransform textRect = cathedralKeyText.GetComponent<RectTransform>();

                if (slotRect != null && textRect != null)
                {
                    textRect.SetParent(slotRect); // Hacer que el texto sea hijo del Slot
                    textRect.localPosition = new Vector3(0f, textRect.localPosition.y, 0f); // Centrar horizontalmente, mantener altura
                    textRect.anchoredPosition = new Vector2(0f, textRect.anchoredPosition.y); // Centrar horizontalmente, mantener altura
                }

                Debug.Log("Texto de la llave activado y centrado en el Slot correspondiente.");
            }
            else
            {
                Debug.Log("Texto de la llave desactivado.");
            }
        }
        else
        {
            Debug.LogWarning("No se ha asignado la referencia 'Cathedral Key Text' en el Inspector.");
        }
    }
}
