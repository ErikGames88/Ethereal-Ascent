using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField, Tooltip("Luz de la linterna (Spot Light)")]
    private Light flashlightLight;

    private bool hasFlashlight = true;

    void Start()
    {
        InitializeLight();
    }

    public void InitializeLight()
    {
        if (flashlightLight == null)
        {
            flashlightLight = GetComponentInChildren<Light>(); // Busca el Light dentro de los hijos del prefab instanciado
        }

        if (flashlightLight != null)
        {
            flashlightLight.enabled = false; // Asegúrate de que comienza apagada
            Debug.Log("Luz de la linterna lista para usar.");
        }
        else
        {
            Debug.LogWarning($"No se encontró ningún componente Light en los hijos de {gameObject.name}.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && hasFlashlight)
        {
            ManageFlashlight();
        }
    }

    private void ManageFlashlight()
    {
        if (flashlightLight != null)
        {
            flashlightLight.enabled = !flashlightLight.enabled;
            Debug.Log($"Linterna encendida: {flashlightLight.enabled}");
        }
    } 
}
