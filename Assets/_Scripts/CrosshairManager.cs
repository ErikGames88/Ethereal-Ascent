using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private Image crosshair;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color interactColor = Color.green;
    private float interactionDistance = 5f;
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
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                crosshair.color = interactColor;
                return;
            }
        }

        crosshair.color = defaultColor;
    }

    public void ShowCrosshair(bool isVisible)
    {
        if (crosshair != null)
        {
            crosshair.gameObject.SetActive(isVisible);
        }
    }
}
