using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private Light lightningFlash; // Luz del relámpago
    [SerializeField] private AudioSource thunderSound; // Sonido del trueno
    [SerializeField] private float minDelay = 5f; // Tiempo mínimo entre relámpagos
    [SerializeField] private float maxDelay = 15f; // Tiempo máximo entre relámpagos

    private float nextLightningTime; // Tiempo del próximo relámpago

    void Start()
    {
        ScheduleNextLightning(); // Configura el tiempo para el primer relámpago
    }

    void Update()
    {
        if (Time.time >= nextLightningTime)
        {
            StartCoroutine(TriggerLightning());
            ScheduleNextLightning(); // Programa el siguiente relámpago
        }
    }

    private void ScheduleNextLightning()
    {
        nextLightningTime = Time.time + 15f; //Random.Range(minDelay, maxDelay);
    }

    private System.Collections.IEnumerator TriggerLightning()
    {
        // Flash de luz
        lightningFlash.intensity = 15f; // Brillo aleatorio
        yield return new WaitForSeconds(0.6f); // Duración del flash
        lightningFlash.intensity = 0;

        // Sonido del trueno (ligero retraso para mayor realismo)
        yield return new WaitForSeconds(0.01f);
        thunderSound.Play();
    }
}