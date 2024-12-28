using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintTextManager : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al objeto Hint Text")]
    private GameObject hintTextContainer;

    [SerializeField, Tooltip("Tiempo de fade-in (en segundos)")]
    private float fadeInDuration = 2f;

    [SerializeField, Tooltip("Tiempo que el texto permanecerá visible (en segundos)")]
    private float displayDuration = 5f;

    [SerializeField, Tooltip("Tiempo de fade-out (en segundos)")]
    private float fadeOutDuration = 2f;

    private TextMeshProUGUI missionHintText;

    private void Start()
    {
        if (hintTextContainer != null)
        {
            missionHintText = hintTextContainer.transform.Find("Mission Hint Text")?.GetComponent<TextMeshProUGUI>();

            if (missionHintText != null)
            {
                missionHintText.gameObject.SetActive(false); 
                SetAlpha(0f); 
            }
        }
    }

    public void ShowMissionHintText()
    {
        if (missionHintText != null)
        {
            missionHintText.gameObject.SetActive(true); 
            StartCoroutine(FadeInDisplayAndFadeOut());
        }
    }

    private IEnumerator FadeInDisplayAndFadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f); 

        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
        missionHintText.gameObject.SetActive(false); 
    }

    private void SetAlpha(float alpha)
    {
        Color color = missionHintText.color;
        color.a = alpha;
        missionHintText.color = color;
    }
}
