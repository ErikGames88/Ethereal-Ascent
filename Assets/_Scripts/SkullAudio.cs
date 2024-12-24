using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAudio : MonoBehaviour
{
    [SerializeField] private float maxDistance = 10f; // Distancia máxima a la que se escucha el sonido
    private Transform player; // Referencia al jugador
    private AudioSource audioSource;

    void Awake()
    {
        // Recupera el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        // Encuentra al jugador automáticamente por su etiqueta
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player'. Asegúrate de que el Player tenga esa etiqueta.");
        }
    }

    void Start()
    {
        audioSource.loop = true; // Habilita el bucle
        audioSource.playOnAwake = false; // Evita que suene al inicio
        audioSource.volume = 0f; // Empieza en silencio
        audioSource.Play(); // Comienza a reproducir el clip
    }

    void Update()
    {
        // Solo ejecuta la lógica si el Player fue encontrado
        if (player != null)
        {
            // Calcula la distancia entre el jugador y la Skull
            float distance = Vector3.Distance(player.position, transform.position);

            // Ajusta el volumen en función de la distancia
            if (distance <= maxDistance)
            {
                // Calcula el volumen proporcional a la distancia
                float volume = 1f - (distance / maxDistance);
                audioSource.volume = Mathf.Clamp01(volume); // Asegura que el volumen esté entre 0 y 1
            }
            else
            {
                audioSource.volume = 0f; // Silencia si está fuera del rango
            }
        }
    }
}
