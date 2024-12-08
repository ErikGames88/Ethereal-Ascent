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

    // Obtener la posición del crosshair en el mundo
    public Vector3 GetCrosshairWorldPosition()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                return hit.point;
            }
        }
        return Vector3.zero; // Retornar una posición neutral si no se detecta nada
    }

    //TODO: Para desctivar el puntero durante una cinemática:
    //FindObjectOfType<CrosshairManager>().EnableCrosshair(false);

    //TODO: Para volver a activar el puntero tras pasar una cinemática:
    //FindObjectOfType<CrosshairManager>().EnableCrosshair(true);
}
