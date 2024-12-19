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

        equippedFlashlight = Instantiate(flashlightPrefab, flashlightPoint);
        flashlightLight = equippedFlashlight.transform.Find("Spot Light").GetComponent<Light>();
        flashlightLight.enabled = false;

        Debug.Log("Linterna equipada correctamente.");
    }

    public void ToggleFlashlight()
    {
        if (flashlightLight == null)
        {
            Debug.LogWarning("No hay linterna equipada o no se encontró el Spot Light.");
            return;
        }

        flashlightLight.enabled = !flashlightLight.enabled;
        Debug.Log($"Linterna {(flashlightLight.enabled ? "encendida" : "apagada")}.");
    }
}
