using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedBackground : MonoBehaviour
{
    [SerializeField] private Image redBackground;
    [SerializeField] private float fadeDuration = 6f;
    [SerializeField] private GameObject finalText;


    private void Start()
    {
        if (redBackground != null)
        {
            Color initialColor = redBackground.color;
            initialColor.a = 0f;
            redBackground.color = initialColor;

            if (finalText != null) finalText.SetActive(false);

            StartCoroutine(FadeInRedBackground());
        }
    }

    private IEnumerator FadeInRedBackground()
    {
        float elapsedTime = 0f;

        Color color = redBackground.color;
        float targetAlpha = 150f / 255f; 

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            color.a = Mathf.Lerp(0f, targetAlpha, elapsedTime / fadeDuration);
            redBackground.color = color;

            yield return null; 
        }

        color.a = targetAlpha;
        redBackground.color = color;

        if (finalText != null)
        {
            finalText.SetActive(true);
        }
    }
}
