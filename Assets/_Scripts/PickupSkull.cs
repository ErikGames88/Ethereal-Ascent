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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            AudioSource[] audioSources = player.GetComponents<AudioSource>();
            if (audioSources.Length >= 3)
            {
                pickupAudioSource = audioSources[2];
            }
        }
    }

    public void Pickup()
    {
        gameObject.SetActive(false);

        PlayPickupSound();
        NotifySkullCounter();

        Destroy(gameObject);
    }
    
    private void NotifySkullCounter()
    {
        if (skullCounter != null)
        {
            skullCounter.AddSkull();
        }
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null && pickupAudioSource != null)
        {
            pickupAudioSource.PlayOneShot(pickupSound);
        }
    }
}
