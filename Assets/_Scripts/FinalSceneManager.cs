using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject blackBackground;
    [SerializeField] private Image finalScenePanel;
    [SerializeField] private Image redBackground;
    [SerializeField] private FinalTextWriter finalTextWriter;
    [SerializeField] private float blackBackgroundDuration = 2f;
    [SerializeField] private float panelFadeDuration = 3f;
    [SerializeField] private float redFadeDuration = 3f;
    

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