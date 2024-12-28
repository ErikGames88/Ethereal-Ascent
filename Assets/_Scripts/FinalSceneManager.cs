using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour
{
    [SerializeField, Tooltip("Black Background")]
    private GameObject blackBackground;

    [SerializeField, Tooltip("Final Scene Panel")]
    private Image finalScenePanel;

    [SerializeField, Tooltip("Red Background")]
    private Image redBackground;

    [SerializeField, Tooltip("Final Text Writer")]
    private FinalTextWriter finalTextWriter;

    [SerializeField, Tooltip("Duración del Black Background (en segundos)")]
    private float blackBackgroundDuration = 2f;

    [SerializeField, Tooltip("Duración del fade-in del Final Scene Panel (en segundos)")]
    private float panelFadeDuration = 3f;

    [SerializeField, Tooltip("Duración del fade-in del Red Background (en segundos)")]
    private float redFadeDuration = 3f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetOpacity(finalScenePanel, 0f);
        SetOpacity(redBackground, 0f);

        redBackground.gameObject.SetActive(false);

        StartCoroutine(FadeInSequence());
    }

    private IEnumerator FadeInSequence()
    {
        yield return new WaitForSeconds(blackBackgroundDuration);

        yield return FadeInImage(finalScenePanel, panelFadeDuration);

        redBackground.gameObject.SetActive(true);

        yield return FadeInImage(redBackground, redFadeDuration, 0.47f); 
        
        if (finalTextWriter != null)
        {
            finalTextWriter.ActivateAndWriteText();
        }
    }

    private IEnumerator FadeInImage(Image image, float duration, float targetAlpha = 1f)
    {
        float elapsedTime = 0f;
        Color color = image.color;
        float startAlpha = color.a;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }

    private void SetOpacity(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}