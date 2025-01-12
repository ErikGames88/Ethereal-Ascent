using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyManager : MonoBehaviour
{   
    private bool isKeyCollected = false; 
    private int keySlotIndex = -1;       

    [SerializeField, Tooltip("Icono de la llave")]
    private Sprite keyIcon;

    [SerializeField, Tooltip("Texto de la llave (Cathedral Key Text)")]
    private GameObject cathedralKeyText;

    [SerializeField, Tooltip("Referencia al HintTextManager para manejar textos de pista")]
    private HintTextManager hintTextManager; // Nueva referencia

    public GameObject CathedralKeyText 
    {
        get => cathedralKeyText;
    }

    public void CollectKey(InventoryManager inventoryManager, int slotIndex, Sprite itemIcon)
    {
        if (isKeyCollected)
        {
            return;
        }

        isKeyCollected = true;
        keySlotIndex = slotIndex;

        inventoryManager.AssignItemToSlot(slotIndex, "Cathedral Key", itemIcon, null, cathedralKeyText);

        // Activar el Scape Hint Text al recoger la llave
        if (hintTextManager != null)
        {
            hintTextManager.ShowScapeHintText();
            Debug.Log("Scape Hint Text activado tras recoger la llave.");
        }
        else
        {
            Debug.LogError("HintTextManager no está asignado en el KeyManager.");
        }
    }

    public void ActivateKeyText(bool isActive, RectTransform slotRect = null)
    {
        if (cathedralKeyText != null)
        {
            cathedralKeyText.SetActive(isActive);

            if (isActive && slotRect != null)
            {
                RectTransform textRect = cathedralKeyText.GetComponent<RectTransform>();
                if (textRect != null)
                {
                    textRect.SetParent(slotRect);

                    textRect.localPosition = new Vector3(0f, textRect.localPosition.y, 0f);
                    textRect.anchoredPosition = new Vector2(0f, textRect.anchoredPosition.y);
                }
            }
        }
    }

    public bool IsKeySelected(int selectedSlotIndex)
    {
        return isKeyCollected && selectedSlotIndex == keySlotIndex;
    }

    public void RemoveKey(InventoryManager inventoryManager)
    {
        if (!isKeyCollected || keySlotIndex < 0 || inventoryManager == null)
        {
            return;
        }

        Image slotImage = inventoryManager.Slots[keySlotIndex].GetComponentInChildren<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = null;
            slotImage.enabled = true;

            Color slotColor = slotImage.color;
            slotColor.a = 0.392f; 
            slotImage.color = slotColor;
        }

        TextMeshProUGUI slotTextComponent = inventoryManager.Slots[keySlotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            slotTextComponent.gameObject.SetActive(false);
            slotTextComponent.text = "";
        }

        isKeyCollected = false;
        keySlotIndex = -1;
    }
}
