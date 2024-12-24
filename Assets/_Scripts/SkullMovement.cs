using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 0.1f; // Distancia máxima del movimiento (subir/bajar)
    [SerializeField] private float movement = 1.5f; // Velocidad del movimiento

    private Vector3 startPosition;

    void Start()
    {
        // Guarda la posición inicial del cráneo
        startPosition = transform.position;
    }

    void Update()
    {
        // Aplica un movimiento suave en el eje Y
        float newY = Mathf.Sin(Time.time * movement) * velocity;
        transform.position = startPosition + new Vector3(0, newY, 0);
    }
}
