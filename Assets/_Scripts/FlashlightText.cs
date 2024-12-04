using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightText : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al prefab de la linterna")]
    private GameObject flashlight;

    void Start()
    {
        if (flashlight != null)
        {
            flashlight.SetActive(false); // Asegurarse de que empieza desactivada
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (flashlight != null)
            {
                flashlight.SetActive(!flashlight.activeSelf); // Activar/desactivar con L
                Debug.Log($"Linterna activa: {flashlight.activeSelf}");
            }
            else
            {
                Debug.LogWarning("No hay prefab de linterna asignado.");
            }
        }
    }
}
