using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 0.1f; 
    [SerializeField] private float movement = 1.5f; 

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * movement) * velocity;
        transform.position = startPosition + new Vector3(0, newY, 0);
    }
}
