using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogFollower : MonoBehaviour
{
    [SerializeField] private Transform mainCamera; // Arrastra la Main Camera aquí desde el Inspector
    [SerializeField] private float fixedYPosition = 8f; // Altura fija para el contenedor
    [SerializeField] private float distance = 30f; // Distancia fija frente al Player

    

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Calcula la posición frente a la cámara
            Vector3 targetPosition = mainCamera.position + mainCamera.forward * distance;

            // Mantén la altura fija
            targetPosition.y = fixedYPosition;

            // Actualiza la posición del contenedor
            transform.position = targetPosition;

            // Alinea la orientación del contenedor con la cámara
            transform.rotation = Quaternion.Euler(0, mainCamera.eulerAngles.y, 0);
        }
    }
}