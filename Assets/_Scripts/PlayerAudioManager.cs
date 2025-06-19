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
    [SerializeField] private AudioSource _mudSteepAudio;
    [SerializeField] private AudioSource _iceStepsAudio;
    [SerializeField] private AudioSource _iceSlideAudio;
    [SerializeField] private AudioSource _crawlAudio;
    [SerializeField] private AudioSource _gruntAudio;


    [Header("Footsteps Audio Clips")]
    [SerializeField] private AudioClip _mazeFootstepsClip;
    [SerializeField] private AudioClip _gardenFootstepsClip;
    [SerializeField] private AudioClip _woodFootstepsClip;
    [SerializeField] private AudioClip _grassFootstepsClip;
    [SerializeField] private AudioClip _rockFootstepsClip;
    [SerializeField] private AudioClip _dungeonFootstepsClip;
    [SerializeField] private AudioClip _mudStepsClip;
    [SerializeField] private AudioClip _iceStepsClip;
    [SerializeField] private AudioClip _iceSlideClip;
    [SerializeField] private AudioClip _crawlClip;
    [SerializeField] private AudioClip _gruntClip;


    [Header("Heartbeats")]
    [SerializeField] private AudioSource _heartBeatsAudio;
    private PlayerHealth _playerHealth;
    private bool heartIsBeating;

    private bool isSteppingOnMaze;
    private bool isSteppingOnGarden;
    private bool isSteppingOnWood;
    private bool isCrawling;
    private bool hasPlayedJumpGrunt;
    private bool hasPlayedDodgeGrunt;

    private string currentFloorTag;
    private float stepTimer = 0.5f;
    private float stepInterval;
    [SerializeField] private float normalStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;
    [SerializeField] private float iceStepInterval = 0.25f;
    [SerializeField] private float crawlInterval;




    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerStamina = GetComponent<PlayerStamina>();
    }

    void OnEnable()
    {
        _playerController.OnJump += UpdateGruntAudio;
        _playerController.OnDodge += UpdateGruntAudio;
    }

    void Start()
    {
        _playerAudio.SetActive(true);
        stepTimer = stepInterval;
    }


    void Update()
    {
        stepTimer -= Time.deltaTime;

        if (_playerController.IsSprinting)
        {
            stepInterval = sprintStepInterval;
        }
        else if (_playerController.IsOnIce && !_playerController.IsSlidingOnIce)
        {
            stepInterval = iceStepInterval;
        }
        else if (_playerController.IsCrouched)
        {
            stepInterval = crawlInterval;
        }
        else
        {
            stepInterval = normalStepInterval;
        }

        if (stepTimer <= 0)
        {
            if (!_playerController.PlayerQuiet || _playerController.IsSlidingOnIce)
            {
                if (_playerController.IsOnMud)
                {
                    UpdateMudSteps();
                    stepTimer = stepInterval;
                }
                else if (_playerController.IsOnIce && !_playerController.IsSlidingOnIce)
                {
                    UpdateIceSteps();
                    stepTimer = stepInterval;
                }
                else if (_playerController.IsOnIce && _playerController.IsSlidingOnIce)
                {
                    UpdateIceSteps();
                }
                else if (_playerController.IsCrouched)
                {
                    UpdateCrawlAudio();
                    stepTimer = stepInterval;
                }
                else
                {
                    UpdateFootsteps();
                    stepTimer = stepInterval;
                }
            }
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
        if (_playerController.IsGrounded && !_playerController.PlayerQuiet)
        {
            UpdateCurrentFloorTag();

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
                stepInterval = sprintStepInterval;
            }
            else
            {
                stepInterval = normalStepInterval;
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
            UpdateCurrentFloorTag();

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
                case "IceSurface":
                    _iceStepsAudio.PlayOneShot(_iceStepsClip);
                    break;
                default:
                    break;

            }
        }

        wasGrounded = _playerController.IsGrounded;
    }

    private void UpdateCurrentFloorTag()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = _groundCheck.transform.position;

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Ground")))
        {
            currentFloorTag = hit.collider.tag;
        }
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
            _mudSteepAudio.PlayOneShot(_mudStepsClip);
        }
        else
        {
            _mudSteepAudio.Stop();
        }
    }

    private void UpdateIceSteps()
    {
        if (!_playerController.IsOnIce)
        {
            if (_iceStepsAudio.isPlaying) _iceStepsAudio.Stop();
            if (_iceSlideAudio.isPlaying) _iceSlideAudio.Stop();
            isPlayingIceSlideAudio = false;
            return;
        }

        if (_playerController.IsSlidingOnIce)
        {
            if (_iceStepsAudio.isPlaying) _iceStepsAudio.Stop();

            if (!isPlayingIceSlideAudio)
            {
                _iceSlideAudio.PlayOneShot(_iceSlideClip);
                isPlayingIceSlideAudio = true;
            }
        }
        else
        {
            if (_iceSlideAudio.isPlaying)
            {
                _iceSlideAudio.Stop();
                isPlayingIceSlideAudio = false;
            }

            if (!_playerController.PlayerQuiet && _playerController.IsGrounded)
            {
                _iceStepsAudio.PlayOneShot(_iceStepsClip);
            }
        }
    }

    private void UpdateCrawlAudio()
    {
        if (_playerController.IsCrouched)
        {
            isCrawling = true;
            _crawlAudio.PlayOneShot(_crawlClip);
        }
        else
        {
            isCrawling = false;
            _crawlAudio.Stop();
        }
    }

    private void UpdateGruntAudio()
    {
        _gruntAudio.PlayOneShot(_gruntClip);
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerStay(Collider other)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mud"))
        {
            _playerController.IsOnMud = false;
        }

        if (other.CompareTag("IceSurface"))
        {
            _playerController.IsOnIce = false;
            isPlayingIceSlideAudio = false;
            _iceStepsAudio.Stop();
            _iceSlideAudio.Stop();
        }
    }

    void OnDisable()
    {
        _playerController.OnJump -= UpdateGruntAudio;
        _playerController.OnDodge -= UpdateGruntAudio;
    }
}
