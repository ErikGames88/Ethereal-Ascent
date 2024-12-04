using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al MessageManager")]
    private MessageManager messageManager;

    [SerializeField, Tooltip("Texto a mostrar al interactuar")]
    private TextMeshProUGUI signText;

    [SerializeField, Tooltip("Referencia al PlayerController")]
    private PlayerController playerController;

    private bool isMessageActive = false;

    void Start()
    {
        if (messageManager == null)
        {
            // Busca directamente entre todos los objetos de la escena, incluidos los desactivados
            MessageManager foundManager = FindObjectOfType<MessageManager>(true); // Activa la búsqueda en objetos desactivados
            if (foundManager != null)
            {
                messageManager = foundManager;
                Debug.Log("MessageManager asignado correctamente desde Messages Canvas.");
            }
            else
            {
                Debug.LogError("MessageManager no encontrado en la escena.");
            }
        }
    }

    public void OnMouseDown()
    {
        if (messageManager == null || signText == null)
        {
            Debug.LogWarning("MessageManager o texto no asignado.");
            return;
        }

        if (!isMessageActive)
        {
            // Mostrar el mensaje y bloquear el movimiento
            messageManager.ShowMessage(signText);
            if (playerController != null)
            {
                playerController.EnableMovement(false);
            }
            isMessageActive = true;
            Debug.Log("Mensaje mostrado.");
        }
    }

    private void Update()
    {
        if (isMessageActive && Input.GetKeyDown(KeyCode.E))
        {
            // Ocultar el mensaje y permitir el movimiento
            messageManager.HideMessage();
            if (playerController != null)
            {
                playerController.EnableMovement(true);
            }
            isMessageActive = false;
            Debug.Log("Mensaje ocultado.");
        }
    }
}
