using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField, Tooltip("AudioSource para reproducir sonidos")]
    private AudioSource audioSource;

    [SerializeField, Tooltip("Sonido para todas las acciones")]
    private AudioClip audioClip;

    [SerializeField, Tooltip("Intervalo entre pasos al caminar (en segundos)")]
    private float stepInterval = 0.5f;

    private PlayerController playerController;
    private float stepTimer;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        HandleFootsteps();
    }

    public void PlaySound()
    {
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    private void HandleFootsteps()
    {
        if (!playerController.isGrounded || playerController.GetCurrentSpeed() <= 0.1f)
        {
            stepTimer = 0; // Resetear el temporizador si no se cumplen las condiciones
            return;
        }

        // Ajustar la frecuencia de pasos según la velocidad
        float interval = playerController.GetCurrentSpeed() > 5f ? stepInterval / 2f : stepInterval;

        stepTimer += Time.deltaTime;

        if (stepTimer >= interval)
        {
            PlaySound();
            stepTimer = 0;
        }
    }
}
