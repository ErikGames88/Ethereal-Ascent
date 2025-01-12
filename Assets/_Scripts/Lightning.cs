using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private Light lightningFlash;
    [SerializeField] private AudioSource thunderSound; 
    [SerializeField] private float minDelay = 45f; 
    [SerializeField] private float maxDelay = 60f; 
    private float nextLightningTime; 

    void Start()
    {
        ScheduleNextLightning(); 
    }

    void Update()
    {
        if (Time.time >= nextLightningTime)
        {
            StartCoroutine(TriggerLightning());
            ScheduleNextLightning(); 
        }
    }

    private void ScheduleNextLightning()
    {
        nextLightningTime = Time.time + Random.Range(minDelay, maxDelay);
    }

    private System.Collections.IEnumerator TriggerLightning()
    {
        lightningFlash.intensity = 15f; 
        yield return new WaitForSeconds(0.6f); 
        lightningFlash.intensity = 0;

        yield return new WaitForSeconds(0.01f);
        thunderSound.Play();
    }
}