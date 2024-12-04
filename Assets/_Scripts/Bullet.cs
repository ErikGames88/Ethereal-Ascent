using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Aquí puedes añadir lógica para interactuar con enemigos o el entorno
        Debug.Log("Bala impactó en: " + collision.gameObject.name);

        Destroy(gameObject); // Destruir la bala al impactar
    }
}
