using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia a la imagen de la barra de progreso")]
    private Image foreground;

    [SerializeField, Tooltip("Referencia al PlayerController para obtener la estamina")]
    private PlayerController playerController;

    void Update()
    {
        if (playerController != null && foreground != null)
        {
            // Obtener el porcentaje de estamina
            float staminaPercent = playerController.GetStamina() / playerController.GetMaxStamina();
            foreground.fillAmount = staminaPercent;

            Debug.Log($"Estamina actual: {playerController.GetStamina()}, Porcentaje: {staminaPercent}");

            // Activar o desactivar la barra según el estado del sprint
            if (playerController.IsSprinting())
            {
                Debug.Log("El jugador está esprintando.");
                if (!gameObject.activeSelf)
                {
                    Debug.Log("Activando barra de estamina.");
                    gameObject.SetActive(true);
                }
            }
            else if (staminaPercent >= 1f && gameObject.activeSelf)
            {
                Debug.Log("Desactivando barra de estamina.");
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Referencias de PlayerController o Foreground no asignadas.");
        }
    }
}
    

