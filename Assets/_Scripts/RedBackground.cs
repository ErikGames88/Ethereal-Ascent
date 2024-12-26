using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedBackground : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al componente Image del Red Background")]
    private Image redBackground;

    [SerializeField, Tooltip("Duración del efecto de opacidad (en segundos)")]
    private float fadeDuration = 6f;

    [SerializeField, Tooltip("Referencia al Final Text")]
    private GameObject finalText;

    private void Start()
    {
        // Asegurarnos de que la opacidad inicial es 0
        if (redBackground != null)
        {
            Color initialColor = redBackground.color;
            initialColor.a = 0f;
            redBackground.color = initialColor;

            // Desactivar Final Text al inicio
            if (finalText != null) finalText.SetActive(false);

            // Iniciar el efecto de opacidad
            StartCoroutine(FadeInRedBackground());
        }
        else
        {
            Debug.LogError("Image del Red Background no asignado.");
        }
    }

    private IEnumerator FadeInRedBackground()
    {
        float elapsedTime = 0f;

        // Color inicial y final
        Color color = redBackground.color;
        float targetAlpha = 150f / 255f; // Opacidad objetivo (convertida a rango [0, 1])

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Incrementar la opacidad gradualmente
            color.a = Mathf.Lerp(0f, targetAlpha, elapsedTime / fadeDuration);
            redBackground.color = color;

            yield return null; // Esperar al siguiente frame
        }

        // Asegurarse de que llega al valor final
        color.a = targetAlpha;
        redBackground.color = color;

        Debug.Log("Red Background completado. Activando Final Text.");

        // Activar el Final Text
        if (finalText != null) finalText.SetActive(true);
    }
}
