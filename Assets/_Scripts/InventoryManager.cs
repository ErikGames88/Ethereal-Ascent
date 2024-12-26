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

    [SerializeField, Tooltip("Lista de prefabs para los slots del inventario")]
    private List<GameObject> slotPrefabs = new List<GameObject>(9);

    public List<GameObject> SlotPrefabs
    {
        get => slotPrefabs;
    }

    [SerializeField, Tooltip("Referencia al borde de selección")]
    private RectTransform selectedBorder;

    [SerializeField, Tooltip("Textos asociados a los Slots (ordenados)")]
    private List<GameObject> slotTexts; 

    private int selectedSlotIndex = -1; 

    public int SelectedSlotIndex
    {
        get => selectedSlotIndex;
        set => selectedSlotIndex = value;
    }

    private bool canNavigateSlots = true;

    void Start()
    {
        if (slots == null || slots.Count != 9)
        {
            return;
        }

        if (slotTexts == null || slotTexts.Count != 9)
        {
            return;
        }

        InitializeInventory();

        selectedSlotIndex = 0;
        UpdateSelectedBorderPosition();

        UpdateItemTextVisibility();
    }

    public void SetSlotNavigation(bool isEnabled)
    {
        canNavigateSlots = isEnabled;
    }

    void Update()
    {
        if (!canNavigateSlots)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            int newSlotIndex = selectedSlotIndex - 1;
            if (newSlotIndex >= 0)
            {
                OnSlotSelected(newSlotIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            int newSlotIndex = selectedSlotIndex + 1;
            if (newSlotIndex < slots.Count)
            {
                OnSlotSelected(newSlotIndex);
            }
        }
    }

    private void InitializeInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            Image slotImage = slots[i].GetComponent<Image>();
            if (slotImage != null)
            {
                if (i == 8) 
                {
                    slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1f); 
                }
                else
                {
                    slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 0.392f); 
                }
            }

            if (slotTexts[i] != null)
            {
                slotTexts[i].SetActive(false);
            }
        }

        selectedSlotIndex = 0;
    }

    public void OnSlotSelected(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            HideAllTexts(); 
            return;
        }

        selectedSlotIndex = slotIndex;
        UpdateSelectedBorderPosition();
        UpdateItemTextVisibility();
    }

    private void UpdateSelectedBorderPosition()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < slots.Count && selectedBorder != null)
        {
            RectTransform targetSlot = slots[selectedSlotIndex].GetComponent<RectTransform>();
            RectTransform borderRectTransform = selectedBorder.GetComponent<RectTransform>();

            if (targetSlot != null && borderRectTransform != null)
            {
                borderRectTransform.position = targetSlot.position + new Vector3(0f, 9.5f, 0f);
                borderRectTransform.anchorMin = targetSlot.anchorMin;
                borderRectTransform.anchorMax = targetSlot.anchorMax;
                borderRectTransform.pivot = targetSlot.pivot;
                borderRectTransform.sizeDelta = new Vector2(241f, 160f);
            }
        }
    }

    public void AssignItemToSlot(int slotIndex, string itemName, Sprite itemIcon, GameObject prefab, GameObject textObject)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            return;
        }

        GameObject slot = slots[slotIndex].gameObject;

        Image slotImage = slot.GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = itemIcon;
            slotImage.enabled = true;

            slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1f); 
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

        AssignTextToSlot(slotIndex, textObject);
    }

    public int FindFirstAvailableSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Button slotButton = slots[i];
            if (slotButton == null)
            {
                continue;
            }

            Image slotImage = slotButton.GetComponent<Image>();
            if (slotImage == null)
            {
                continue;
            }

            if (slotImage.sprite == null)
            {
                return i;
            }
        }

        return -1;
    }

    public GameObject GetPrefabFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotPrefabs.Count)
        {
            return null;
        }

        return slotPrefabs[slotIndex];
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            return;
        }

        Image slotImage = slots[slotIndex].GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.sprite = null; 
            slotImage.enabled = true; 
            slotImage.color = new Color(1f, 1f, 1f, 0.392f); 
        }

        TextMeshProUGUI slotTextComponent = slots[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (slotTextComponent != null)
        {
            slotTextComponent.text = ""; 
            slotTextComponent.gameObject.SetActive(false);
        }

        if (slotTexts[slotIndex] != null)
        {
            slotTexts[slotIndex].SetActive(false);
        }

        if (slotPrefabs != null && slotIndex < slotPrefabs.Count)
        {
            slotPrefabs[slotIndex] = null;
        }
    }

    public void AssignTextToSlot(int slotIndex, GameObject textObject)
    {
        if (slotIndex < 0 || slotIndex >= slotTexts.Count)
        {
            return;
        }

        slotTexts[slotIndex] = textObject;
    }

    public void UpdateItemTextVisibility()
    {
        if (!InventoryToggle.isInventoryOpen) 
        {
            HideAllTexts();
            return;
        }

        if (selectedSlotIndex < 0 || selectedSlotIndex >= slots.Count)
        {
            HideAllTexts();
            return;
        }

        
        HideAllTexts();

        Button selectedSlot = slots[selectedSlotIndex];
        GameObject selectedPrefab = slotPrefabs[selectedSlotIndex];

        if (selectedPrefab != null)
        {
            string prefabName = selectedPrefab.name;

            if (prefabName == "Flashlight")
            {
                PositionText(slotTexts[selectedSlotIndex], selectedSlot.GetComponent<RectTransform>());
            }
            else if (prefabName == "Cathedral Key")
            {
                PositionText(slotTexts[selectedSlotIndex], selectedSlot.GetComponent<RectTransform>());
            }
        }

        if (selectedSlotIndex == 8) 
        {
            PositionText(slotTexts[8], slots[8].GetComponent<RectTransform>());
        }
    }

    private void PositionText(GameObject textObject, RectTransform slotRect)
    {
        if (textObject == null || slotRect == null) return;

        textObject.SetActive(true); 

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        Vector3 slotWorldPosition = slotRect.position;

        textRect.position = new Vector3(slotWorldPosition.x, textRect.position.y, slotWorldPosition.z);
    }

    public void HideAllTexts()
    {
        foreach (GameObject text in slotTexts)
        {
            if (text != null)
            {
                text.SetActive(false);
            }
        }
    }
}
