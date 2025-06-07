using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Heartbeats")]
    [SerializeField] private GameObject _playerAudio;
    [SerializeField] private AudioSource _heartBeatsAudio;
    private PlayerHealth _playerHealth;
    private bool heartIsBeating;


    void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        _playerAudio.SetActive(false);
    }


    void Update()
    {
        UpdateHeartbeats();
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
            _playerAudio.SetActive(true);

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
            _playerAudio.SetActive(false);
        }

        if (heartIsBeating && !_heartBeatsAudio.isPlaying)
        {
            _heartBeatsAudio.Play();
        }
    }
}
