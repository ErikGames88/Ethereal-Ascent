using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private GameObject textBackground;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI dismissText;
    [SerializeField] private float showDelay = 2f;
    [SerializeField] private PlayerLocked playerLocked;
    [SerializeField]private GameObject inventoryCanvas;
    [SerializeField]private InventoryToggle inventoryToggle;
    [SerializeField] private HintTextManager hintTextManager;
    [SerializeField] private GameObject timerObject;
    [SerializeField,] private TimerManager timerManager;

    private bool isMissionTextActive = false;
    private bool wasInventoryOpen = false;
    public bool isMissionTextClosed = false; 

    private void Start()
    {
        if (textBackground != null && missionText != null && dismissText != null && playerLocked != null
        && inventoryCanvas != null && inventoryToggle != null && hintTextManager != null && timerObject != null)
        {
            textBackground.SetActive(false); 
            timerObject.SetActive(false); 

            Invoke(nameof(ShowMissionText), showDelay); 
        }
    }

    private void Update()
    {
        if (isMissionTextActive && Input.GetKeyDown(KeyCode.E))
        {
            CloseMissionText();
        }
    }

    private void ShowMissionText()
    {
        if (textBackground != null && missionText != null && dismissText != null)
        {
            if (inventoryCanvas.activeSelf)
            {
                wasInventoryOpen = true;
                inventoryCanvas.SetActive(false); 
            }

            inventoryToggle.EnableInventoryToggle(false);

            textBackground.SetActive(true);
            missionText.gameObject.SetActive(true);
            dismissText.gameObject.SetActive(true);

            playerLocked.LockPlayer(true);

            isMissionTextActive = true;
        }
    }

    private void CloseMissionText()
    {
        if (textBackground != null && missionText != null && dismissText != null)
        {
            textBackground.SetActive(false);
            missionText.gameObject.SetActive(false);
            dismissText.gameObject.SetActive(false);

            inventoryToggle.EnableInventoryToggle(true);

            if (wasInventoryOpen)
            {
                inventoryCanvas.SetActive(true);
                wasInventoryOpen = false;
            }

            playerLocked.LockPlayer(false);

            isMissionTextActive = false;
            isMissionTextClosed = true; 

            hintTextManager.ShowMissionHintText();

            timerObject.SetActive(true);
            timerManager.StartTimer(); 
        }
    }
}