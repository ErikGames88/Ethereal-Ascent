using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al MessageManager")]
    private MessageManager messageManager;

    [SerializeField, Tooltip("Texto del mensaje que se mostrará")]
    private TextMeshProUGUI gateMessageText;

    [SerializeField, Tooltip("Referencia al PlayerController para desactivar movimiento")]
    private PlayerController playerController;

    private bool isMessageActive = false;

    private void Start()
    {
        if (messageManager == null)
        {
            messageManager = FindObjectOfType<MessageManager>();
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    private void OnMouseDown()
    {
        if (!isMessageActive && messageManager != null && gateMessageText != null)
        {
            messageManager.ShowMessage(gateMessageText);
            isMessageActive = true;

            if (playerController != null)
            {
                playerController.EnableMovement(false);
            }
        }
    }

    private void Update()
    {
        if (isMessageActive && Input.GetKeyDown(KeyCode.E))
        {
            messageManager.HideMessage();
            isMessageActive = false;

            if (playerController != null)
            {
                playerController.EnableMovement(true);
            }
        }
    }
}

