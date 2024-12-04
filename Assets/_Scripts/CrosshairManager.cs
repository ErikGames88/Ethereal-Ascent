using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al puntero")]
    private GameObject crosshair;

    void Start()
    {
        crosshair.SetActive(true);
    }

    
    public void ShowCrosshair(bool isEnabled)
    {
        crosshair.SetActive(isEnabled);
    }

    //TODO: Para desctivar el puntero durante una cinemática:
    //FindObjectOfType<CrosshairManager>().EnableCrosshair(false);

    //TODO: Para volver a activar el puntero tras pasar una cinemática:
    //FindObjectOfType<CrosshairManager>().EnableCrosshair(true);
}
