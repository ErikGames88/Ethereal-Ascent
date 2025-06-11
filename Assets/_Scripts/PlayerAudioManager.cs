using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerStamina))]
public class PlayerAudioManager : MonoBehaviour
{
    [Header("Dependences")]
    private PlayerController _playerController;
    private PlayerStamina _playerStamina;
    [SerializeField] private GameObject _playerAudio;
    [SerializeField] private GameObject _groundCheck;
    private bool wasGrounded;

    [Header("Footsteps")]
    [SerializeField] private AudioSource _mazeFootstepsAudio;
    [SerializeField] private AudioClip _mazeFootstepsClip;
    [SerializeField] private AudioSource _gardenFootstepsAudio;
    [SerializeField] private AudioSource _woodFootstepsAudio;
    private bool isSteppingOnMaze;
    private bool isSteppingOnGarden;
    private bool isSteppingOnWood;
    private float stepTimer = 0.5f;
    private float stepInterval;

    [Header("Heartbeats")]
    [SerializeField] private AudioSource _heartBeatsAudio;
    private PlayerHealth _playerHealth;
    private bool heartIsBeating;


    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerStamina = GetComponent<PlayerStamina>();
    }

    void Start()
    {
        _playerAudio.SetActive(true);
        stepTimer = stepInterval;
    }


    void Update()
    {
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            UpdateFootsteps();
            stepTimer = stepInterval;
        }

        UpdateHeartbeats();
    }

    private void UpdateFootsteps()
    {
        Vector3 raycastOrigin = _groundCheck.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Ground")))
        {
            isSteppingOnMaze = true;

            if (_playerController.IsGrounded && !_playerController.PlayerQuiet
            && hit.collider.tag == "MazeFloor")
            {
                wasGrounded = true;

                if (_playerStamina.CanSprint && _playerStamina.SprintActive)
                {
                    stepInterval = 0.3f;
                }
                else
                {
                    stepInterval = 0.5f;
                }

                _mazeFootstepsAudio.pitch = 1.6f;
                _mazeFootstepsAudio.PlayOneShot(_mazeFootstepsClip);

                if (!wasGrounded && _playerController.IsGrounded)
                {
                    wasGrounded = true;
                }
            }
        }
        else
        {
            isSteppingOnMaze = false;
        }
    }

    /// <summary>
    /// Updates the heartbeats audio logic.
    /// Plays heartbeat sound when the player is damaged and has low health.
    /// Adjusts pitch depending on the current health level.
    /// </summary>
    private void UpdateHeartbeats()
    {
        if (_playerHealth.GetDamage && _playerHealth.CurrentHealth <= 35)
        {
            heartIsBeating = true;

            if (_playerHealth.CurrentHealth <= 35 && _playerHealth.CurrentHealth > 10)
            {
                _heartBeatsAudio.pitch = 1.3f;
            }
            else if (_playerHealth.CurrentHealth <= 10 && _playerHealth.CurrentHealth > 0)
            {
                _heartBeatsAudio.pitch = 1.7f;
            }
        }
        else
        {
            heartIsBeating = false;
            _heartBeatsAudio.Stop();
        }

        if (heartIsBeating && !_heartBeatsAudio.isPlaying)
        {
            _heartBeatsAudio.Play();
        }
    }
}
