using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]private float normalStepRate = 0.5f;
    [SerializeField] private float sprintStepRate = 0.3f;
    [SerializeField] private AudioClip landingStep;
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
        if (playerController.GetComponent<Rigidbody>().velocity.magnitude <= 0.1f || !playerController.IsGrounded())
        {
            stepTimer = 0f;
            return;
        }

        float stepRate;
        if (playerController.IsSprinting())
        {
            stepRate = sprintStepRate;
        }
        else
        {
            stepRate = normalStepRate;
        }

        stepTimer += Time.deltaTime;
        if (stepTimer >= stepRate)
        {
            audioSource.Play(); 
            stepTimer = 0f; 
        }
    }

    private void LandingSound()
    {
        if (!isLanded && playerController.IsGrounded())
        {
            audioSource.PlayOneShot(landingStep); 
        }

        isLanded = playerController.IsGrounded();
    }
}
