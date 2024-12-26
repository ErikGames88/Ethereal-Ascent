using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("Sensibilidad del mouse")]
    private float mouseSensitivity; 

    [SerializeField, Tooltip("Referencia a la posición del Player")]
    public Transform player;   

    [Tooltip("Rotación del mouse")]      
    private float xRotation;

    [Tooltip("Límite de rotación del Player")]   
    private float limitRotation = 60f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -limitRotation, limitRotation); 
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX);
    }
}
