using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles first-person camera rotation based on mouse input.
/// Controls vertical look (with optional inversion) and horizontal player rotation.
/// </summary>
public class CameraController : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    private float rotLimit;
    private float xRotation;
    private bool invertVertical;
    [SerializeField] GameObject _player;

    void Start()
    {
        rotLimit = 80f;
    }

    void Update()
    {
        Debug.Log()
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (!invertVertical)
        {
            xRotation -= mouseY;
        }
        else
        {
            xRotation += mouseY;
        }


        xRotation = Mathf.Clamp(xRotation, -rotLimit, rotLimit);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _player.transform.Rotate(0f, mouseX, 0f);
    }
    
}
