using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectInteractable : MonoBehaviour
{
    [SerializeField, Tooltip("Sistema de partículas para resaltar")]
    private GameObject highlightEffect;

    private bool isHighlighted = false;

    // Activar/desactivar el sistema de partículas
    public void Highlight(bool enable)
    {
        if (highlightEffect == null)
        {
            Debug.LogWarning($"{gameObject.name}: HighlightEffect no asignado.");
            return;
        }

        if (enable && !isHighlighted)
        {
            highlightEffect.SetActive(true);
            isHighlighted = true;
            Debug.Log($"{gameObject.name}: Highlight activado.");
        }
        else if (!enable && isHighlighted)
        {
            highlightEffect.SetActive(false);
            isHighlighted = false;
            Debug.Log($"{gameObject.name}: Highlight desactivado.");
        }
    }

    // Método de interacción
    public void Interact(MessageManager messageManager, TextMeshProUGUI messageText)
    {
        if (messageManager != null && messageText != null)
        {
            messageManager.ShowMessage(messageText);
        }
        else
        {
            Debug.LogWarning("MessageManager o texto no asignado.");
        }
    }
}
