using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSkull : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al Skull Counter")]
    private SkullCounter skullCounter;

    [SerializeField, Tooltip("Sonido al recoger el cráneo")]
    private AudioClip pickupSound;

    private AudioSource pickupAudioSource; // Referencia al AudioSource dedicado para recogida

    private void Awake()
    {
        // Busca automáticamente el SkullCounter en la escena
        if (skullCounter == null)
        {
            skullCounter = FindObjectOfType<SkullCounter>();
            if (skullCounter == null)
            {
                Debug.LogError("No se encontró un SkullCounter en la escena. Asegúrate de que está configurado.");
            }
        }

        // Obtén el tercer AudioSource del Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            AudioSource[] audioSources = player.GetComponents<AudioSource>();
            if (audioSources.Length >= 3)
            {
                pickupAudioSource = audioSources[2]; // Tercer AudioSource
            }
        }

        if (pickupAudioSource == null)
        {
            Debug.LogError("No se encontró un tercer AudioSource en el Player.");
        }
    }

    public void Pickup()
    {
        if (skullCounter != null)
        {
            skullCounter.AddSkull(); // Incrementa el contador
        }

        PlayPickupSound(); // Reproduce el sonido desde el tercer AudioSource

        // Desactiva y destruye el cráneo inmediatamente
        gameObject.SetActive(false);
        Destroy(gameObject);

        Debug.Log($"Cráneo recogido: {gameObject.name}");
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null && pickupAudioSource != null)
        {
            pickupAudioSource.PlayOneShot(pickupSound);
            Debug.Log("Sonido de recogida reproducido desde el tercer AudioSource.");
        }
        else
        {
            Debug.LogWarning("No se pudo reproducir el sonido de recogida. Verifica el AudioSource y el clip.");
        }
    }
}