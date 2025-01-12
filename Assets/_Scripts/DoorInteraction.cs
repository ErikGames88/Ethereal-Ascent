using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{   
    private float openAngle = 90f;
    private float openDuration = 2f;
    private AudioSource audioSource;
    private bool isOpen = false;
    private Transform gateL;
    private Transform gateR;
    private Quaternion initialRotationL;
    private Quaternion initialRotationR;
    private Quaternion targetRotationL;
    private Quaternion targetRotationR;
    private KeyManager keyManager;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private Collider gatesCollider;
    

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && openSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = openSound;
        }

        Transform gates = transform.Find("Gates");
        if (gates != null)
        {
            gateL = gates.Find("Gate L");
            gateR = gates.Find("Gate R");

            gatesCollider = gates.GetComponent<Collider>();

            if (gateL != null && gateR != null)
            {
                initialRotationL = gateL.localRotation;
                initialRotationR = gateR.localRotation;

                targetRotationL = Quaternion.Euler(0, openAngle, 0) * initialRotationL;
                targetRotationR = Quaternion.Euler(0, -openAngle, 0) * initialRotationR;
            }
        }

        keyManager = FindObjectOfType<KeyManager>();
    }

    public void Interact()
    {
        if (!isOpen)
        {
            StartCoroutine(OpenDoor());

            if (keyManager != null)
            {
                keyManager.RemoveKey(FindObjectOfType<InventoryManager>());
            }
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

        if (gatesCollider != null)
        {
            gatesCollider.enabled = false;
        }
    }
}
