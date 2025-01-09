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

    [SerializeField] private PauseMenuManager pauseMenuManager;  // Referencia al PauseMenuManager

    void Update()
    {
        // Si el menú de pausa está activo, bloqueamos la rotación
        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            return;  // Si está en pausa, no se ejecuta la rotación
        }

        // Si el menú no está activo, permitimos la rotación normal
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -limitRotation, limitRotation); 
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX);
    }
}
