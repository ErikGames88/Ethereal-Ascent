using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fogSystem; 
    [SerializeField] private float fadeDuration = 4f; 
    private HashSet<Collider> activeTerrains = new HashSet<Collider>();
    private Coroutine currentFadeCoroutine;
    private ParticleSystem.EmissionModule emissionModule; 
    

    private void Start()
    {
        if (fogSystem != null)
        {
            emissionModule = fogSystem.emission; 
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Terrain") && other.gameObject.layer != LayerMask.NameToLayer("Volcano"))
    {
        activeTerrains.Add(other);
        UpdateFogState();
    }
}

private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Terrain") && other.gameObject.layer != LayerMask.NameToLayer("Volcano"))
    {
        activeTerrains.Remove(other);
        UpdateFogState();
    }
}

    private void UpdateFogState()
    {
        if (activeTerrains.Count > 0)
        {
            AdjustFogVisibility(false); 
        }
        else
        {
            AdjustFogVisibility(true); 
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
            yield break;
        }

        float startRate = emissionModule.rateOverTime.constant;
        float targetRate;
        if (isVisible)
        {
            targetRate = 50f;
        }
        else
        {
            targetRate = 0f;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newRate = Mathf.Lerp(startRate, targetRate, elapsedTime / fadeDuration);

            var rate = emissionModule.rateOverTime;
            rate.constant = newRate;
            emissionModule.rateOverTime = rate;

            yield return null;
        }

        var finalRate = emissionModule.rateOverTime;
        finalRate.constant = targetRate;
        emissionModule.rateOverTime = finalRate;

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