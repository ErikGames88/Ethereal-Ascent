using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintTextManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI missionHintTextDirect;
    [SerializeField] private TextMeshProUGUI cathedralHintTextDirect;
    [SerializeField] private TextMeshProUGUI scapeHintTextDirect;
    [SerializeField] private GameObject hintTextContainer; 
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float fadeOutDuration = 2f;

    private TextMeshProUGUI missionHintText;
    private TextMeshProUGUI cathedralHintText;
    private TextMeshProUGUI scapeHintText;


    private void Start()
    {
        if (hintTextContainer != null)
        {
            if (missionHintTextDirect != null)
            {
                ConfigureHintText(missionHintTextDirect, "Mission Hint Text");
                missionHintText = missionHintTextDirect;
            }

            if (cathedralHintTextDirect != null)
            {
                ConfigureHintText(cathedralHintTextDirect, "Cathedral Key Hint Text");
                cathedralHintText = cathedralHintTextDirect;
            }

            if (scapeHintTextDirect != null)
            {
                ConfigureHintText(scapeHintTextDirect, "Scape Hint Text");
                scapeHintText = scapeHintTextDirect;
            }
        }
    }

    private void ConfigureHintText(TextMeshProUGUI text, string textName)
    {
        if (text != null)
        {
            text.gameObject.SetActive(false);
            SetAlpha(text, 0f);
        }
    }

    public void ShowMissionHintText()
    {
        if (missionHintText != null)
        {
            ActivateParentContainer();
            missionHintText.gameObject.SetActive(true);
            StartCoroutine(FadeInDisplayAndFadeOut(missionHintText));
        }
    }

    public void ShowCathedralHintText()
    {
        if (cathedralHintText != null)
        {
            ActivateParentContainer();
            cathedralHintText.gameObject.SetActive(true);
            StartCoroutine(FadeInDisplayAndFadeOut(cathedralHintText));
        }
    }

    public void ShowScapeHintText()
    {
        if (scapeHintText != null)
        {
            ActivateParentContainer();
            scapeHintText.gameObject.SetActive(true);

            StartCoroutine(FadeInDisplayAndFadeOut(scapeHintText));
        }
    }

    private IEnumerator FadeInDisplayAndFadeOut(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            SetAlpha(text, alpha);
            yield return null;
        }

        SetAlpha(text, 1f);

        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            SetAlpha(text, alpha);
            yield return null;
        }

        SetAlpha(text, 0f);
        text.gameObject.SetActive(false);
    }

    private void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    private void ActivateParentContainer()
    {
        if (hintTextContainer != null && !hintTextContainer.activeSelf)
        {
            hintTextContainer.SetActive(true);
        }
    }
}
