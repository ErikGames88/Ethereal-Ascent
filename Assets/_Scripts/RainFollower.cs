using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollower : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al Player para que la lluvia le siga")]
    private Transform player;

    [SerializeField, Tooltip("Altura de la lluvia")]
    private float rainHeight = 13.5f;

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;
            targetPosition.y += rainHeight; 
            transform.position = targetPosition;
        }
    }
}
