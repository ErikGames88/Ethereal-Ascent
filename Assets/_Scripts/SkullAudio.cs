using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAudio : MonoBehaviour
{
    [SerializeField] private float maxDistance = 10f; 
    private Transform player; 
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Start()
    {
        audioSource.loop = true; 
        audioSource.playOnAwake = false; 
        audioSource.volume = 0f; 
        audioSource.Play(); 
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= maxDistance)
            {
                float volume = 1f - (distance / maxDistance);
                audioSource.volume = Mathf.Clamp01(volume); 
            }
            else
            {
                audioSource.volume = 0f; 
            }
        }
    }
}
