using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Ángulo de apertura de las puertas")]
    private float openAngle = 90f;

    [SerializeField, Tooltip("Duración de la animación de apertura")]
    private float openDuration = 2f;

    [SerializeField, Tooltip("Sonido al abrir la puerta")]
    private AudioClip openSound;

    private AudioSource audioSource;
    private bool isOpen = false;

    private Transform gateL;
    private Transform gateR;

    private Quaternion initialRotationL;
    private Quaternion initialRotationR;

    private Quaternion targetRotationL;
    private Quaternion targetRotationR;

    void Awake()
    {
        // Configurar el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && openSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = openSound;
        }

        // Encontrar las puertas
        Transform gates = transform.Find("Gates");
        if (gates != null)
        {
            gateL = gates.Find("Gate L");
            gateR = gates.Find("Gate R");

            if (gateL != null && gateR != null)
            {
                initialRotationL = gateL.localRotation;
                initialRotationR = gateR.localRotation;

                targetRotationL = Quaternion.Euler(0, openAngle, 0) * initialRotationL;
                targetRotationR = Quaternion.Euler(0, -openAngle, 0) * initialRotationR;
            }
            else
            {
                Debug.LogError("No se encontraron las puertas Gate L y Gate R en Gates.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el objeto Gates en la jerarquía.");
        }
    }

    public void Interact()
    {
        if (!isOpen)
        {
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        isOpen = true;
        float elapsedTime = 0f;

        if (audioSource != null && openSound != null)
        {
            audioSource.Play();
        }

        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / openDuration;

            if (gateL != null) gateL.localRotation = Quaternion.Lerp(initialRotationL, targetRotationL, t);
            if (gateR != null) gateR.localRotation = Quaternion.Lerp(initialRotationR, targetRotationR, t);

            yield return null;
        }

        if (gateL != null) gateL.localRotation = targetRotationL;
        if (gateR != null) gateR.localRotation = targetRotationR;

        Debug.Log("Puertas abiertas.");
    }
}
