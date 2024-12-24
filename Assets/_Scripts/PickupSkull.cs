using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSkull : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al Skull Counter")]
    private SkullCounter skullCounter;

    private void Awake()
    {
        // Busca automáticamente el SkullCounter en la escena
        if (skullCounter == null)
        {
            skullCounter = FindObjectOfType<SkullCounter>();
            if (skullCounter == null)
            {
                Debug.LogError("No se encontró un SkullCounter en la escena. Asegúrate de que está configurado.");
            }
        }
    }

    public void Pickup()
    {
        if (skullCounter != null)
        {
            skullCounter.AddSkull(); // Incrementa el contador
        }

        gameObject.SetActive(false); // Desactiva el cráneo
        Debug.Log($"Cráneo recogido: {gameObject.name}");
    }
}
