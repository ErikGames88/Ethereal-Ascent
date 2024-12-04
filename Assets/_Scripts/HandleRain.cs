using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRain : MonoBehaviour
{
    [SerializeField, Tooltip("Sistema de partículas de la lluvia")]
    private ParticleSystem rainParticles; 

    [SerializeField, Tooltip("Sonido de la lluvia")]
    private AudioSource rainSound;

    [SerializeField, Tooltip("Volumen de lluvia para laberinto y exteriores")]
    private float outdoorVolume = 1.0f; 

    [SerializeField, Tooltip("Volumen para zonas cubiertas")]
    private float indoorVolume = 0.2f; 

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (rainParticles != null)
            {
                rainParticles.Stop(); 
            }

            if (rainSound != null)
            {
                rainSound.volume = indoorVolume; 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (rainParticles != null)
            {
                rainParticles.Play(); 
            }

            if (rainSound != null)
            {
                rainSound.volume = outdoorVolume; 
            }
        }
    }
}
