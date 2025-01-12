using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintTextManager : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al objeto Hint Text")]
    private GameObject hintTextContainer; // Hint Text (padre)

    [SerializeField, Tooltip("Tiempo de fade-in (en segundos)")]
    private float fadeInDuration = 2f;

    [SerializeField, Tooltip("Tiempo que el texto permanecerá visible (en segundos)")]
    private float displayDuration = 5f;

    [SerializeField, Tooltip("Tiempo de fade-out (en segundos)")]
    private float fadeOutDuration = 2f;

    private TextMeshProUGUI missionHintText;
    private TextMeshProUGUI cathedralHintText;
    private TextMeshProUGUI scapeHintText;

    [SerializeField, Tooltip("Referencia directa al Mission Hint Text")]
    private TextMeshProUGUI missionHintTextDirect;

    [SerializeField, Tooltip("Referencia directa al Cathedral Key Hint Text")]
    private TextMeshProUGUI cathedralHintTextDirect;

    [SerializeField, Tooltip("Referencia directa al Scape Hint Text")]
    private TextMeshProUGUI scapeHintTextDirect;

    private void Start()
    {
        if (hintTextContainer != null)
        {
            Debug.Log("Hint Text Container encontrado correctamente.");

            // Usar referencias directas
            if (missionHintTextDirect != null)
            {
                ConfigureHintText(missionHintTextDirect, "Mission Hint Text");
                missionHintText = missionHintTextDirect;
            }
            else
            {
                Debug.LogError("Mission Hint Text no está asignado manualmente.");
            }

            if (cathedralHintTextDirect != null)
            {
                ConfigureHintText(cathedralHintTextDirect, "Cathedral Key Hint Text");
                cathedralHintText = cathedralHintTextDirect;
            }
            else
            {
                Debug.LogError("Cathedral Key Hint Text no está asignado manualmente.");
            }

            if (scapeHintTextDirect != null)
            {
                ConfigureHintText(scapeHintTextDirect, "Scape Hint Text");
                scapeHintText = scapeHintTextDirect;
            }
            else
            {
                Debug.LogError("Scape Hint Text no está asignado manualmente.");
            }
        }
        else
        {
            Debug.LogError("Hint Text Container no está asignado en el Inspector.");
        }
    }

    private void ConfigureHintText(TextMeshProUGUI text, string textName)
    {
        if (text != null)
        {
            text.gameObject.SetActive(false);
            SetAlpha(text, 0f);
            Debug.Log($"{textName} asignado correctamente.");
        }
        else
        {
            Debug.LogError($"{textName} no está asignado o no se encontró.");
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
        else
        {
            Debug.LogError("Cathedral Key Hint Text no está asignado o no se encontró.");
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
        else
        {
            Debug.LogError("Scape Hint Text no está asignado o no se encontró.");
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
            Debug.Log("Hint Text Container activado.");
        }
    }
}
