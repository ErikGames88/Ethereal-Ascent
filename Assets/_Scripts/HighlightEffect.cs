using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightEffect : MonoBehaviour
{
    [SerializeField, Tooltip("Sistema de partículas para resaltar")]
    private ParticleSystem highlightParticles;

    public void ShowEffect()
    {
        if (highlightParticles != null)
        {
            highlightParticles.Play();
        }
    }

    public void StopEffect()
    {
        if (highlightParticles != null)
        {
            highlightParticles.Stop();
        }
    }
}
