using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private PlayerLocked playerLocked;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private PauseMenuManager pauseMenuManager;
    [SerializeField] private MissionManager missionManager;
    public static bool isInventoryOpen = false;
    private bool canToggleInventory = true; 

    

    void Start()
    {
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }

        if (playerLocked == null)
        {
            playerLocked = FindObjectOfType<PlayerLocked>();
        }

        if (timerManager == null)
        {
            timerManager = FindObjectOfType<TimerManager>();
        }

        if (missionManager == null)
        {
            missionManager = FindObjectOfType<MissionManager>(); 
        }
    }

    void Update()
    {
        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            return; 
        }

        if (missionManager != null && !missionManager.isMissionTextClosed)
        {
            return; 
        }

        if (!canToggleInventory)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ManageInventory();
        }
    }

    private void ManageInventory()
    {
        if (inventoryCanvas == null || playerLocked == null || inventoryManager == null || timerManager == null)
        {
            return;
        }

        if (!isInventoryOpen && playerLocked.IsPlayerLocked())
        {
            return; 
        }

        isInventoryOpen = !isInventoryOpen;
        
        inventoryCanvas.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            timerManager.StopTimer();

            playerLocked.LockPlayer(true, false);
            inventoryManager.UpdateItemTextVisibility();
        }
        else
        {
            timerManager.StartTimer();

            playerLocked.LockPlayer(false);
        }

        if (inventoryManager != null)
        {
            inventoryManager.SetSlotNavigation(isInventoryOpen);
        }

        if (!isInventoryOpen)
        {
            inventoryManager.HideAllTexts();
        }
    }

    public void EnableInventoryToggle(bool isEnabled)
    {
        canToggleInventory = isEnabled;
    }
}
