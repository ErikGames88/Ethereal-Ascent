using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    private bool isFlashlightCollected = false; 
    private int flashlightSlotIndex = -1;       

    [SerializeField, Tooltip("Prefab de la linterna")]
    private GameObject flashlightPrefab;

    [SerializeField, Tooltip("Icono de la linterna para el inventario")]
    private Sprite flashlightIcon;

    private InventoryManager inventoryManager;
    private GameObject equippedFlashlight; 
    private Light flashlightLight; 

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
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
            return;
        }

        isFlashlightCollected = true;
        flashlightSlotIndex = slotIndex;
    }


    public void EquipFlashlight()
    {
        if (!isFlashlightCollected)
        {
            return;
        }

        if (equippedFlashlight != null)
        {
            Destroy(equippedFlashlight);
            flashlightLight = null;
            equippedFlashlight = null;

            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform flashlightPoint = player.transform.Find("Main Camera/Flashlight Point");

        if (flashlightPoint == null)
        {
            return;
        }

        equippedFlashlight = Instantiate(flashlightPrefab, flashlightPoint);
        equippedFlashlight.name = "Flashlight";
        equippedFlashlight.transform.localPosition = new Vector3(-0.922f, 0.031f, -0.031f);
        equippedFlashlight.transform.localRotation = Quaternion.Euler(0f, 85.27f, 90f);

        Transform particleEffect = equippedFlashlight.transform.Find("Item Particle Effect");
        if (particleEffect != null)
        {
            particleEffect.gameObject.SetActive(false);
        }

        flashlightLight = equippedFlashlight.GetComponentInChildren<Light>(true);
        if (flashlightLight != null)
        {
            flashlightLight.enabled = false;
        }
    }

    public void ToggleFlashlight()
    {
        if (equippedFlashlight == null || flashlightLight == null)
        {
            return;
        }

        if (!flashlightLight.gameObject.activeSelf)
        {
            flashlightLight.gameObject.SetActive(true);
        }

        flashlightLight.enabled = !flashlightLight.enabled;
    }
}
