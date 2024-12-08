using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SafeRoomGateInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia a la puerta izquierda")]
    private Transform gateLeft;

    [SerializeField, Tooltip("Referencia a la puerta derecha")]
    private Transform gateRight;

    [SerializeField, Tooltip("Ángulo de rotación para abrir las puertas")]
    private float openAngle = 90f;

    [SerializeField, Tooltip("Velocidad de apertura para las puertas")]
    private float openSpeed = 2f;

    [SerializeField, Tooltip("Nombre del tag de la llave requerida")]
    private string requiredKeyTag = "SafeRoomKey";

    [SerializeField, Tooltip("Audio Clip para el sonido de la puerta abriéndose")]
    private AudioClip gateOpeningSFX;

    private AudioSource audioSource;
    private ManageInventory inventory;
    private bool isGateOpen = false;

    void Start()
    {
        inventory = FindObjectOfType<ManageInventory>();
        if (inventory == null)
        {
            Debug.LogError("No se encontró el script ManageInventory en la escena.");
        }

        // Configuración del AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = gateOpeningSFX;
    }

    void Update()
    {
        // Solo abrir la puerta con E si el inventario está abierto
        if (Input.GetKeyDown(KeyCode.E) && inventory.IsInventoryOpen())
        {
            TryOpenGate();
        }
    }

    private void OnMouseDown()
    {
        // Al hacer clic, solo mostrar el mensaje
        Debug.LogWarning("Se necesita una llave para abrir esta puerta.");
    }

    public bool TryOpenGate()
    {
        Debug.Log("Intentando abrir la puerta...");

        GameObject equippedItem = inventory.GetEquippedItem(); // Obtener el objeto equipado
        if (equippedItem != null && equippedItem.CompareTag("SafeRoomKey")) // Verificar por tag
        {
            Debug.Log("Llave equipada detectada. Abriendo puerta...");
            OpenGate();
            inventory.RemoveItemByTag("SafeRoomKey");
            return true;
        }

        Debug.LogWarning("La llave equipada no es válida o no está equipada.");
        return false;
    }

    private void OpenGate()
    {
        Debug.Log("¡Abriendo las puertas de la Safe Room!");
        StartCoroutine(OpenDoor(gateLeft, -openAngle));
        StartCoroutine(OpenDoor(gateRight, openAngle));

        if (audioSource != null && gateOpeningSFX != null)
        {
            audioSource.Play();
        }

        GetComponent<Collider>().enabled = false;
        isGateOpen = true;
    }

    private IEnumerator OpenDoor(Transform gate, float targetAngle)
    {
        Quaternion initialRotation = gate.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, targetAngle, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < openSpeed)
        {
            gate.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / openSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gate.localRotation = targetRotation;
        Debug.Log($"Puerta {gate.name} abierta.");
    }
}