using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem rainParticles;
    [SerializeField] private AudioSource rainAudio;
    [SerializeField] private float outdoorVolume = 0.6f;
    [SerializeField] private float indoorVolume = 0.2f;
    private bool isActive = true; 
    

    void Awake()
    {
        if (rainParticles == null)
        {
            rainParticles = GetComponentInChildren<ParticleSystem>();
        }

        if (rainAudio == null)
        {
            rainAudio = GetComponentInChildren<AudioSource>();
        }

        if (rainParticles != null)
        {
            rainParticles.Play();
        }

        if (rainAudio != null)
        {
            rainAudio.volume = outdoorVolume;
            rainAudio.Play();
        }
    }

    void Update()
    {
        if (rainAudio != null)
        {
            float targetVolume;
            if (isActive)
            {
                targetVolume = outdoorVolume;
            }
            else
            {
                targetVolume = indoorVolume;
            }

            rainAudio.volume = Mathf.Lerp(rainAudio.volume, targetVolume, Time.deltaTime * 5f);
        }
    }

    public void SetRainActive(bool isActive)
    {
        this.isActive = isActive;

        if (rainParticles != null)
        {
            if (isActive)
            {
                rainParticles.Play();
            }
            else
            {
                rainParticles.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NoSprintZone"))
        {
            SetRainActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NoSprintZone"))
        {
            SetRainActive(true);
        }
    }
}
