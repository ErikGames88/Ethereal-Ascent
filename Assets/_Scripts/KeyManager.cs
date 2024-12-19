using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{   
    private bool isKeyCollected = false; // Verificar si la llave ha sido recogida
    private int keySlotIndex = -1;       // Índice dinámico donde se equipa la llave

    [SerializeField, Tooltip("Icono de la llave")]
    private Sprite keyIcon;

    [SerializeField, Tooltip("Texto de la llave (Cathedral Key Text)")]
    private GameObject cathedralKeyText;

    public GameObject CathedralKeyText 
    {
        get => cathedralKeyText;
    }

    // Método para recoger la llave y asociarla al primer slot disponible
    public void CollectKey(InventoryManager inventoryManager, int slotIndex, Sprite itemIcon)
    {
        if (isKeyCollected)
        {
            Debug.LogWarning("La llave ya ha sido recogida.");
            return;
        }

        isKeyCollected = true;
        keySlotIndex = slotIndex;
        inventoryManager.AssignItemToSlot(slotIndex, "Cathedral Key", itemIcon);

        Debug.Log($"Llave recogida y asignada al Slot {slotIndex}.");
    }

    // Método para activar o desactivar el texto de la llave
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
                    // Asegurarse de que el texto esté vinculado al slot
                    textRect.SetParent(slotRect);

                    // Ajustar la posición para centrarlo, manteniendo la altura configurada desde el editor
                    textRect.localPosition = new Vector3(0f, textRect.localPosition.y, 0f);
                    textRect.anchoredPosition = new Vector2(0f, textRect.anchoredPosition.y);

                    Debug.Log("Texto de la llave centrado en el slot.");
                }
            }
            else
            {
                Debug.Log(isActive ? "Texto de la llave activado." : "Texto de la llave desactivado.");
            }
        }
        else
        {
            Debug.LogWarning("Cathedral Key Text no está asignado en el KeyManager.");
        }
    }

    // Método para verificar si la llave está recogida y seleccionada
    public bool IsKeySelected(int selectedSlotIndex)
    {
        return isKeyCollected && selectedSlotIndex == keySlotIndex;
    }
}
