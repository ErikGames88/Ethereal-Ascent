using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    [SerializeField, Tooltip("Panel del mensaje")]
    private GameObject messageCanvas;

    private TextMeshProUGUI activeMessageText;

    private bool isMessageActive = false; // Indicador del estado del mensaje

    void Start()
    {
        if (messageCanvas == null)
        {
            Debug.LogError("El MessageCanvas no está asignado en el MessageManager.");
            return;
        }

        if (!messageCanvas.activeInHierarchy)
        {
            messageCanvas.SetActive(false); // Asegúrate de que empieza desactivado.
            Debug.Log("MessageCanvas inicializado correctamente.");
        }
    }

    public void ShowMessage(TextMeshProUGUI messageText)
    {
        if (messageCanvas != null)
        {
            messageCanvas.SetActive(true);
            activeMessageText = messageText;
            activeMessageText.gameObject.SetActive(true); // Muestra el texto específico.
            isMessageActive = true; // Marca el mensaje como activo
            Debug.Log("Mensaje mostrado.");
        }
    }

    public void HideMessage()
    {
        if (messageCanvas != null && activeMessageText != null)
        {
            activeMessageText.gameObject.SetActive(false); // Oculta el texto específico.
            messageCanvas.SetActive(false);
            isMessageActive = false; // Marca el mensaje como inactivo
            Debug.Log("Mensaje ocultado.");
        }
    }

    public bool IsMessageActive()
    {
        return isMessageActive;
    }
}
