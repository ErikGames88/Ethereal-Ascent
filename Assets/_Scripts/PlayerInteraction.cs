using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Rango de interacción del jugador")]
    private float interactionRange = 5f;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            ObjectInteractable interactable = hit.collider.GetComponent<ObjectInteractable>();

            if (interactable != null)
            {
                Debug.Log($"Detectado interactuable: {hit.collider.name}"); // Depuración
                interactable.Highlight(true);

                if (Input.GetMouseButtonDown(0)) // Botón izquierdo del mouse
                {
                    SignInteraction signInteraction = hit.collider.GetComponent<SignInteraction>();
                    if (signInteraction != null)
                    {
                        signInteraction.OnMouseDown();
                    }
                }
            }
        }
        else
        {
            foreach (ObjectInteractable obj in FindObjectsOfType<ObjectInteractable>())
            {
                obj.Highlight(false);
            }
        }
    }
}
