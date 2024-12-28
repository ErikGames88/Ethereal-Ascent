using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al Text Background")]
    private GameObject textBackground;

    [SerializeField, Tooltip("Referencia al texto de misión")]
    private TextMeshProUGUI missionText;

    [SerializeField, Tooltip("Referencia al texto de dismiss (por ejemplo, 'Saltar')")]
    private TextMeshProUGUI dismissText;

    [SerializeField, Tooltip("Retraso en segundos para mostrar el texto de misión al iniciar la escena")]
    private float showDelay = 2f;

    [SerializeField, Tooltip("Referencia al PlayerLocked para manejar el bloqueo del jugador")]
    private PlayerLocked playerLocked;

    [SerializeField, Tooltip("Referencia al Canvas del inventario")]
    private GameObject inventoryCanvas;

    [SerializeField, Tooltip("Referencia al InventoryToggle para desactivar Tab")]
    private InventoryToggle inventoryToggle;

    [SerializeField, Tooltip("Referencia al HintTextManager para manejar textos de pista")]
    private HintTextManager hintTextManager;

    private bool isMissionTextActive = false;
    private bool wasInventoryOpen = false;

    private void Start()
    {
        if (textBackground != null && missionText != null && dismissText != null && playerLocked != null && inventoryCanvas != null && inventoryToggle != null)
        {
            textBackground.SetActive(false); 
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

            hintTextManager.ShowMissionHintText();
        }
    }
}
