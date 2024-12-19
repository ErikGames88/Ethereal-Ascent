using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    private bool isFlashlightCollected = false; // Verificar si la linterna ha sido recogida
    private int flashlightSlotIndex = -1;       // Índice dinámico donde se equipa la linterna

    [SerializeField, Tooltip("Prefab de la linterna")]
    private GameObject flashlightPrefab;

    [SerializeField, Tooltip("Icono de la linterna para el inventario")]
    private Sprite flashlightIcon;

    private InventoryManager inventoryManager;
    private GameObject equippedFlashlight; // Referencia al objeto de la linterna equipada
    private Light flashlightLight; // Referencia al Spot Light de la linterna equipada

    void Start()
    {
        // Obtener el InventoryManager
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("No se encontró el InventoryManager en la escena.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    public void CollectFlashlight(int slotIndex, Sprite itemIcon)
    {
        if (isFlashlightCollected)
        {
            Debug.LogWarning("La linterna ya ha sido recogida.");
            return;
        }

        isFlashlightCollected = true;
        flashlightSlotIndex = slotIndex;
        Debug.Log($"Linterna recogida y asignada al Slot {slotIndex}.");
    }


    public void EquipFlashlight()
    {
        if (!isFlashlightCollected)
        {
            Debug.LogWarning("La linterna no ha sido recogida aún.");
            return;
        }

        if (equippedFlashlight != null)
        {
            Destroy(equippedFlashlight);
            flashlightLight = null;
            equippedFlashlight = null;
            Debug.Log("Linterna desequipada.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform flashlightPoint = player.transform.Find("Main Camera/Flashlight Point");

        if (flashlightPoint == null)
        {
            Debug.LogError("No se encontró el Flashlight Point en el Player.");
            return;
        }

        // Instanciar la linterna en las coordenadas adecuadas
        equippedFlashlight = Instantiate(flashlightPrefab, flashlightPoint);
        equippedFlashlight.name = "Flashlight";
        equippedFlashlight.transform.localPosition = new Vector3(-0.922f, 0.031f, -0.031f);
        equippedFlashlight.transform.localRotation = Quaternion.Euler(0f, 85.27f, 90f);

        // Buscar el componente Light incluso si el Spot Light está desactivado
        flashlightLight = equippedFlashlight.GetComponentInChildren<Light>(true);
        if (flashlightLight == null)
        {
            Debug.LogError("El prefab de la linterna no contiene un componente Light en el Spot Light.");
        }
        else
        {
            flashlightLight.enabled = false; // Asegurarse de que la luz esté apagada al equipar
            Debug.Log("Referencia al Spot Light establecida correctamente.");
        }

        Debug.Log("Linterna equipada correctamente.");
    }

    public void ToggleFlashlight()
    {
        if (equippedFlashlight == null || flashlightLight == null)
        {
            Debug.LogWarning("No hay linterna equipada o no se encontró el Spot Light.");
            return;
        }

        // Activar el objeto padre (Spot Light) si está desactivado
        if (!flashlightLight.gameObject.activeSelf)
        {
            flashlightLight.gameObject.SetActive(true);
        }

        // Cambiar el estado de encendido/apagado del componente Light
        flashlightLight.enabled = !flashlightLight.enabled;

        Debug.Log($"Linterna {(flashlightLight.enabled ? "encendida" : "apagada")}.");
    }
}
