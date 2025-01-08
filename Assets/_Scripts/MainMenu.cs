using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject controlsImage;  // Referencia al Controls Image
    private bool isControlsActive = false;

    
    void Update()
    {
        if (isControlsActive && Input.GetKeyDown(KeyCode.E))
        {
            controlsImage.SetActive(false);  // Desactivar la imagen de controles
            isControlsActive = false;  // Marcar los controles como inactivos
            Cursor.visible = true;  // Mostrar el cursor
        }
    }
    public void OnStartButtonPressed()
    {
        // Cargar la siguiente escena, en este caso "Lore Scene"
        SceneManager.LoadScene("Lore Scene");
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exit Button Pressed! Closing the game.");
        Application.Quit(); // Cierra la aplicación
    }

    public void OnControlsButtonPressed()
    {
        controlsImage.SetActive(true);  // Activar la imagen de controles
        isControlsActive = true;  // Marcar los controles como activos
        Cursor.visible = false;  // Ocultar el cursor
    }
}
