using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkullCounter : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al TextMeshPro para mostrar el contador")]
    private TextMeshProUGUI counterText;

    [SerializeField, Tooltip("Sprite del icono de los cráneos")]
    private Image skullIcon;

    private int skullCount = 0; // Contador interno de cráneos

    void Start()
    {
        // Inicializa el contador a 0
        if (counterText != null)
        {
            counterText.text = "0";
        }

        // Asegura que el icono del cráneo está visible
        if (skullIcon != null)
        {
            skullIcon.enabled = true;
        }
    }

    public void AddSkull()
    {
        skullCount++;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = skullCount.ToString();

            if (skullCount >= 6)
            {
                Debug.Log("¡Todos los cráneos recolectados!");
            }
        }
    }
}
