using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Distancia máxima de interacción")]
    private float interactionDistance = 3f;

    [SerializeField, Tooltip("Layer de las puertas")]
    private LayerMask interactableLayer;

    private InventoryManager inventoryManager;
    private KeyManager keyManager;

    void Start()
    {
        // Obtener la referencia al InventoryManager
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("No se encontró el InventoryManager en la escena.");
        }

        // Obtener la referencia al KeyManager
        keyManager = FindObjectOfType<KeyManager>();
        if (keyManager == null)
        {
            Debug.LogError("No se encontró el KeyManager en la escena.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (!InventoryToggle.isInventoryOpen)
        {
            Debug.Log("El inventario está cerrado. No se puede interactuar con objetos.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log($"Interacción detectada con: {hit.collider.gameObject.name}");

            if (keyManager != null && keyManager.IsKeySelected(inventoryManager.SelectedSlotIndex))
            {
                Debug.Log("Llave seleccionada. Abriendo la puerta...");
                OpenGate(hit.collider.gameObject); // Llamada al método de apertura
            }
            else
            {
                Debug.Log("La llave no está seleccionada.");
            }
        }
        else
        {
            Debug.Log("No estás mirando ningún objeto interactuable.");
        }
    }

    private void OpenGate(GameObject gate)
    {
        // Encontrar el prefab raíz (Cathedral Gate)
        Transform cathedralGate = gate.transform.root;
        if (cathedralGate != null)
        {
            Debug.Log($"Prefab raíz encontrado: {cathedralGate.name}");

            // Buscar el objeto Gates dentro del prefab raíz
            Transform gates = cathedralGate.Find("Gates");
            if (gates != null)
            {
                Debug.Log("Gates encontrado.");

                // Buscar las puertas Gate L y Gate R dentro de Gates
                Transform gateL = gates.Find("Gate L");
                Transform gateR = gates.Find("Gate R");

                if (gateL != null && gateR != null)
                {
                    Debug.Log("Puertas encontradas: Gate L y Gate R.");
                    float targetAngle = 90f;
                    float duration = 2f;
                    StartCoroutine(SmoothOpenGate(gateL, gateR, targetAngle, duration));
                }
                else
                {
                    Debug.LogWarning("No se encontraron las puertas Gate L y Gate R dentro de Gates.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró Gates dentro de Cathedral Gate.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el prefab raíz Cathedral Gate.");
        }
    }

    private IEnumerator SmoothOpenGate(Transform gateL, Transform gateR, float targetAngle, float duration)
    {
        Quaternion initialRotationL = gateL.localRotation;
        Quaternion initialRotationR = gateR.localRotation;

        Quaternion targetRotationL = Quaternion.Euler(0, targetAngle, 0);
        Quaternion targetRotationR = Quaternion.Euler(0, targetAngle, 0);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            gateL.localRotation = Quaternion.Lerp(initialRotationL, targetRotationL, t);
            gateR.localRotation = Quaternion.Lerp(initialRotationR, targetRotationR, t);

            yield return null;
        }

        // Asegurar que las puertas alcancen la rotación final
        gateL.localRotation = targetRotationL;
        gateR.localRotation = targetRotationR;

        Debug.Log("Puertas abiertas suavemente.");
    }
}
