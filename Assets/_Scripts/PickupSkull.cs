using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSkull : MonoBehaviour
{
    [SerializeField, Tooltip("Sonido al recoger el cráneo")]
    private AudioClip pickupSound;

    private SkullCounter skullCounter;
    private AudioSource pickupAudioSource;

    private void Awake()
    {
        skullCounter = FindObjectOfType<SkullCounter>();
        if (skullCounter == null)
        {
            Debug.LogError("No se encontró un SkullCounter en la escena.");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            AudioSource[] audioSources = player.GetComponents<AudioSource>();
            if (audioSources.Length >= 3)
            {
                pickupAudioSource = audioSources[2];
            }
        }

        if (pickupAudioSource == null)
        {
            Debug.LogError("No se encontró un tercer AudioSource en el Player.");
        }
    }

    public void Pickup()
    {
        Debug.Log($"Iniciando recogida de cráneo: {gameObject.name}");

        // Reproduce el sonido
        PlayPickupSound();

        // Notifica al SkullCounter DESPUÉS de desactivar y procesar completamente
        NotifySkullCounter();

        // Finalmente, destruye el objeto
        Destroy(gameObject);

        Debug.Log($"Cráneo recogido completamente: {gameObject.name}");
    }

    private void NotifySkullCounter()
    {
        if (skullCounter != null)
        {
            Debug.Log($"Notificando al SkullCounter: {gameObject.name}");
            skullCounter.AddSkull();
        }
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null && pickupAudioSource != null)
        {
            pickupAudioSource.PlayOneShot(pickupSound);
            Debug.Log("Sonido de recogida reproducido.");
        }
        else
        {
            Debug.LogWarning("No se pudo reproducir el sonido de recogida.");
        }
    }
}
