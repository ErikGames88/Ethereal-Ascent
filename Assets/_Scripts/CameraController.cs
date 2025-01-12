using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity; 
    [SerializeField] public Transform player;   
    [SerializeField] private PauseMenuManager pauseMenuManager;  
    private float xRotation;
    private float limitRotation = 60f;
    
    void Update()
    {
        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            return;  
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -limitRotation, limitRotation); 
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX);
    }
}
