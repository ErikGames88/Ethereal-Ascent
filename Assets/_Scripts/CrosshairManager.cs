using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al puntero (Image)")]
    private Image crosshair;

    [SerializeField, Tooltip("Color normal del puntero")]
    private Color defaultColor = Color.white;

    private float interactionDistance = 5f;

    [SerializeField, Tooltip("Color al apuntar a un objeto interactuable")]
    private Color interactColor = Color.green;

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        crosshair.color = defaultColor;
        crosshair.gameObject.SetActive(true);
    }

    void Update()
    {
        HandleCrosshair();
    }

    private void HandleCrosshair()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                crosshair.color = interactColor;

                /*if (Input.GetMouseButton(0))
                {
                    Debug.Log($"Interacted with {hit.collider.name}");
                }*/

                return;
            }
        }

        crosshair.color = defaultColor;
    }
}
