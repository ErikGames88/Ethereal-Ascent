using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScenePanelFade : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al componente Image del Final Scene Panel")]
    private Image panelImage;

    [SerializeField, Tooltip("Duración del efecto de opacidad (en segundos)")]
    private float fadeDuration = 2f;

    private void Start()
    {
        // Asegurar que la opacidad inicial es 0
        if (panelImage != null)
        {
            Color initialColor = panelImage.color;
            initialColor.a = 0f;
            panelImage.color = initialColor;

            // Iniciar el efecto de fade-in
            StartCoroutine(FadeInPanel());
        }
        else
        {
            Debug.LogError("Image del Final Scene Panel no asignada.");
        }
    }

    private IEnumerator FadeInPanel()
    {
        float elapsedTime = 0f;

        // Color inicial y final
        Color color = panelImage.color;
        float targetAlpha = 1f; // Opacidad completa (255 en rango [0,1])

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Incrementar la opacidad gradualmente
            color.a = Mathf.Lerp(0f, targetAlpha, elapsedTime / fadeDuration);
            panelImage.color = color;

            yield return null; // Esperar al siguiente frame
        }

        // Asegurarse de que llega al valor final
        color.a = targetAlpha;
        panelImage.color = color;

        Debug.Log("Final Scene Panel completado.");
    }
}
