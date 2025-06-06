using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerAudio;
    [SerializeField] private AudioSource _heartBeatsAudio;
    private PlayerHealth _playerHealth;
    

    void Awake()
    {
        _heartBeatsAudio.GetComponent<AudioSource>();
        
    }
    void Start()
    {
        
    }


    void Update()
    {
        if (_playerHealth.CurrentHealth <= 30)
        {
            _heartBeatsAudio.Play();
        }
        else
        {
            _heartBeatsAudio.Stop();
        }
    }
}
