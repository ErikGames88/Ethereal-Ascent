using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlsImage; 
    private bool isControlsActive = false;
    

    
    void Update()
    {
        if (isControlsActive && Input.GetKeyDown(KeyCode.E))
        {
            controlsImage.SetActive(false);  
            isControlsActive = false;  
            Cursor.visible = true;  
        }
    }
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Lore Scene");
    }

    public void OnExitButtonPressed()
    {
        Application.Quit(); 
    }

    public void OnControlsButtonPressed()
    {
        controlsImage.SetActive(true);  
        isControlsActive = true;  
        Cursor.visible = false;  
    }
}
