using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fogSystem; // Sistema de partículas de la niebla
    [SerializeField] private float fadeDuration = 4f; // Tiempo para transiciones suaves
    private HashSet<Collider> activeTerrains = new HashSet<Collider>();
    private Coroutine currentFadeCoroutine;
    private ParticleSystem.EmissionModule emissionModule; // Referencia al módulo de emisión

    private void Start()
    {
        if (fogSystem != null)
        {
            emissionModule = fogSystem.emission; // Obtener el módulo de emisión una vez al inicio
        }
        else
        {
            Debug.LogError("Fog System no asignado en el Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            activeTerrains.Add(other);
            UpdateFogState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            activeTerrains.Remove(other);
            UpdateFogState();
        }
    }

    private void UpdateFogState()
    {
        if (activeTerrains.Count > 0)
        {
            AdjustFogVisibility(false); // Oculta la niebla
        }
        else
        {
            AdjustFogVisibility(true); // Muestra la niebla
        }
    }

    private void AdjustFogVisibility(bool isVisible)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeFog(isVisible));
    }

    private IEnumerator FadeFog(bool isVisible)
    {
        if (fogSystem == null)
        {
            Debug.LogError("Fog System no está asignado en el Inspector.");
            yield break;
        }

        float startRate = emissionModule.rateOverTime.constant;
        float targetRate = isVisible ? 50f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newRate = Mathf.Lerp(startRate, targetRate, elapsedTime / fadeDuration);

            // Asigna directamente al módulo inicializado
            var rate = emissionModule.rateOverTime;
            rate.constant = newRate;
            emissionModule.rateOverTime = rate;

            yield return null;
        }

        // Establece el valor final
        var finalRate = emissionModule.rateOverTime;
        finalRate.constant = targetRate;
        emissionModule.rateOverTime = finalRate;

        // Detén o reproduce el sistema según el estado
        if (targetRate == 0f && fogSystem.isPlaying)
        {
            fogSystem.Stop();
        }
        else if (targetRate > 0f && !fogSystem.isPlaying)
        {
            fogSystem.Play();
        }
    }
}