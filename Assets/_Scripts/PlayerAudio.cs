using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField, Tooltip("Tiempo entre pasos normales")]
    private float normalStepRate = 0.5f;

    [SerializeField, Tooltip("Tiempo entre pasos al esprintar")]
    private float sprintStepRate = 0.3f;

    [SerializeField, Tooltip("Sonido al aterrizar")]
    private AudioClip landingStep;

    private AudioSource audioSource;
    private PlayerController playerController;

    private float stepTimer = 0f;
    private bool isLanded = true;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        FootstepsSound();
        LandingSound();
    }

    private void FootstepsSound()
    {
        // No reproducir pasos si no hay movimiento
        if (playerController.GetComponent<Rigidbody>().velocity.magnitude <= 0.1f || !playerController.IsGrounded())
        {
            stepTimer = 0f;
            return;
        }

        // Determinar tiempo entre pasos
        float stepRate;
        if (playerController.IsSprinting())
        {
            stepRate = sprintStepRate;
        }
        else
        {
            stepRate = normalStepRate;
        }

        // Contador para reproducir pasos
        stepTimer += Time.deltaTime;
        if (stepTimer >= stepRate)
        {
            audioSource.Play(); // Reproducir el sonido asignado en el AudioSource
            stepTimer = 0f; // Reiniciar el contador
        }
    }

    private void LandingSound()
    {
        // Verificar si aterrizó después de estar en el aire
        if (!isLanded && playerController.IsGrounded())
        {
            audioSource.PlayOneShot(landingStep); // Reproducir sonido único al aterrizar
        }

        // Actualizar estado del suelo
        isLanded = playerController.IsGrounded();
    }
}
