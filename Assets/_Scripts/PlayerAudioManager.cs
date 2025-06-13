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
    [SerializeField] private GameObject _footstepsAudio;
    [SerializeField] private GameObject _groundCheck;
    private bool wasGrounded;
    private bool isPlayingIceSlideAudio;

    [Header("Footsteps Audio Sources")]
    [SerializeField] private AudioSource _mazeFootstepsAudio;
    [SerializeField] private AudioSource _gardenFootstepsAudio;
    [SerializeField] private AudioSource _woodFootstepsAudio;
    [SerializeField] private AudioSource _grassFootstepsAudio;
    [SerializeField] private AudioSource _rockFootstepsAudio;
    [SerializeField] private AudioSource _dungeonFootstepsAudio;
    [SerializeField] private AudioSource _mudAudio;
    [SerializeField] private AudioSource _iceAudio;
    [SerializeField] private AudioSource _iceSlideAudio;


    [Header("Footsteps Audio Clips")]
    [SerializeField] private AudioClip _mazeFootstepsClip;
    [SerializeField] private AudioClip _gardenFootstepsClip;
    [SerializeField] private AudioClip _woodFootstepsClip;
    [SerializeField] private AudioClip _grassFootstepsClip;
    [SerializeField] private AudioClip _rockFootstepsClip;
    [SerializeField] private AudioClip _dungeonFootstepsClip;
    [SerializeField] private AudioClip _mudClip;
    [SerializeField] private AudioClip _iceClip;
    [SerializeField] private AudioClip _iceSlideClip;


    [Header("Heartbeats")]
    [SerializeField] private AudioSource _heartBeatsAudio;
    private PlayerHealth _playerHealth;
    private bool heartIsBeating;

    private bool isSteppingOnMaze;
    private bool isSteppingOnGarden;
    private bool isSteppingOnWood;
    private string currentFloorTag;
    private float stepTimer = 0.5f;
    private float stepInterval;

    


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
            if (!_playerController.PlayerQuiet || _playerController.IsSlidingOnIce)
            {
                if (_playerController.IsOnMud)
                {
                    UpdateMudSteps();
                }
                else if (_playerController.IsOnIce)
                {
                    UpdateIceSteps();
                }
                else
                {
                    UpdateFootsteps();
                }
            }

            stepTimer = stepInterval;
        }

        UpdateFallenSteps();
        UpdateHeartbeats();
    }

    /// <summary>
    /// Updates the footsteps sound logic.
    /// Plays appropriate footstep sounds depending on the surface type, player movement state,
    /// and whether the player is sprinting or walking.
    /// The step interval is dynamically adjusted based on sprint state and direction.
    /// </summary>
    private void UpdateFootsteps()
    {
        Vector3 raycastOrigin = _groundCheck.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Ground")))
        {
            currentFloorTag = hit.collider.tag;

            if (_playerController.IsGrounded && !_playerController.PlayerQuiet)
            {
                switch (currentFloorTag)
                {
                    case "MazeFloor":
                        isSteppingOnMaze = true;

                        _mazeFootstepsAudio.pitch = 1.6f;
                        _mazeFootstepsAudio.PlayOneShot(_mazeFootstepsClip);
                        break;
                    case "GardenFloor":
                        isSteppingOnGarden = true;

                        _gardenFootstepsAudio.pitch = 1.6f;
                        _gardenFootstepsAudio.PlayOneShot(_gardenFootstepsClip);
                        break;
                    case "WoodFloor":
                        isSteppingOnWood = true;

                        _woodFootstepsAudio.pitch = 1.6f;
                        _woodFootstepsAudio.PlayOneShot(_woodFootstepsClip);
                        break;
                    case "GrassFloor":
                        _grassFootstepsAudio.pitch = 1.6f;
                        _grassFootstepsAudio.PlayOneShot(_grassFootstepsClip);
                        break;
                    case "RockFloor":
                        _rockFootstepsAudio.pitch = 1f;
                        _rockFootstepsAudio.PlayOneShot(_rockFootstepsClip);
                        break;
                    case "DungeonFloor":
                        _dungeonFootstepsAudio.PlayOneShot(_dungeonFootstepsClip);
                        break;
                    default:
                        break;

                }

                if (_playerStamina.CanSprint && _playerStamina.SprintActive
                && !_playerController.ConstrainDirections)
                {
                    stepInterval = 0.3f;
                }
                else
                {
                    stepInterval = 0.5f;
                }
            }
        }
    }

    /// <summary>
    /// Updates the fallen step sound logic.
    /// Plays a single footstep sound when the player lands on the Maze floor after being in the air.
    /// Works independently of the regular step interval to ensure an immediate landing sound.
    /// </summary>
    private void UpdateFallenSteps()
    {
        if (!wasGrounded && _playerController.IsGrounded)
        {
            wasGrounded = true;

            switch (currentFloorTag)
            {
                case "MazeFloor":
                    _mazeFootstepsAudio.PlayOneShot(_mazeFootstepsClip);
                    break;
                case "GardenFloor":
                    _gardenFootstepsAudio.PlayOneShot(_gardenFootstepsClip);
                    break;
                case "WoodFloor":
                    _woodFootstepsAudio.PlayOneShot(_woodFootstepsClip);
                    break;
                case "GrassFloor":
                    _grassFootstepsAudio.PlayOneShot(_grassFootstepsClip);
                    break;
                case "RockFloor":
                    _rockFootstepsAudio.PlayOneShot(_rockFootstepsClip);
                    break;
                case "DungeonFloor":
                    _dungeonFootstepsAudio.PlayOneShot(_dungeonFootstepsClip);
                    break;
                default:
                    break;

            }
        }

        wasGrounded = _playerController.IsGrounded;
    }

    /// <summary>
    /// Updates the heartbeats audio logic.
    /// Plays heartbeat sound when the player is damaged and has low health.
    /// Dynamically adjusts the heartbeat pitch depending on the player's current health level.
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

    private void UpdateMudSteps()
    {
        if (_playerController.IsOnMud)
        {
            _mudAudio.PlayOneShot(_mudClip);
        }
        else
        {
            _mudAudio.Stop();
        }
    }

    private void UpdateIceSteps()
    {
        if (!_playerController.IsSlidingOnIce)
        {
            isPlayingIceSlideAudio = false;

            if (_playerController.IsOnIce)
            {
                _iceAudio.PlayOneShot(_iceClip);
            }
            else
            {
                _iceAudio.Stop();
            }
        }
        else
        {
            if (!isPlayingIceSlideAudio)
            {
                isPlayingIceSlideAudio = true;
                
                if (_playerController.IsOnIce || _playerController.PlayerQuiet)
                {
                    _iceSlideAudio.PlayOneShot(_iceSlideClip);
                }
            }
            else
            {
                if (!_playerController.IsOnIce || !_playerController.PlayerQuiet || !_playerController.IsSlidingOnIce)
                {
                    _iceSlideAudio.Stop();
                    isPlayingIceSlideAudio = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mud"))
        {
            _playerController.IsOnMud = true;
        }

        if (other.CompareTag("IceSurface"))
        {
            _playerController.IsOnIce = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mud"))
        {
            _playerController.IsOnMud = true;
        }

        if (other.CompareTag("IceSurface"))
        {
            _playerController.IsOnIce = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mud"))
        {
            _playerController.IsOnMud = false;
        }

        if (other.CompareTag("IceSurface"))
        {
            _playerController.IsOnIce = false;
            isPlayingIceSlideAudio = false;
            _iceSlideAudio.Stop();
        }
    }
}
