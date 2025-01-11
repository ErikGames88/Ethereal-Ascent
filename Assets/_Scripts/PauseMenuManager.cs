using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuCanvas; // El Canvas del Pause Menu

    [SerializeField]
    private PlayerController playerController; // Referencia al PlayerController

    [SerializeField]
    private MissionManager missionManager; // Referencia al MissionManager para comprobar si el MissionText se haya cerrado

    [SerializeField]
    private TimerManager timerManager; // Referencia al TimerManager para congelar el timer

    [SerializeField]
    private Button resumeButton; // Referencia al Resume Button

    [SerializeField]
    private Button controlsButton; // Referencia al Controls Button

    [SerializeField]
    private Button exitButton; // Referencia al Exit Button

    [SerializeField]
    private Button notButton; // Referencia al Not Button

    [SerializeField]
    private Button yesButton; // Referencia al Yes Button

    [SerializeField]
    private GameObject controlsImage; // Referencia al Controls Image

    [SerializeField]
    private GameObject exitBackground; // Referencia al Exit Background

    [SerializeField, Tooltip("Referencia al TextManager para verificar si hay texto activo")]
    private TextManager textManager;

    private bool isControlsImageActive = false; // Estado de activación de Controls Image


    void Start()
    {
        // Asignar las funciones a los botones desde el inspector
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(OnClickResumeButton);
        }

        if (controlsButton != null)
        {
            controlsButton.onClick.AddListener(OnClickControlsButton);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnClickExitButton);
        }

        if (notButton != null)
        {
            notButton.onClick.AddListener(OnClickNotButton);
        }

        if (yesButton != null)
        {
            yesButton.onClick.AddListener(OnClickYesButton);
        }
    }

    void Update()
    {
        // Bloquear la apertura del Pause Menu si el MissionText no está cerrado o el texto del Letter Background está activo
        if ((missionManager != null && !missionManager.isMissionTextClosed) || 
            (textManager != null && textManager.IsTextActive()))
        {
            return; // No se puede abrir el Pause Menu en estas condiciones
        }

        // Activar el menú de pausa solo con Esc si no está abierto
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenuCanvas.activeSelf && !InventoryToggle.isInventoryOpen)
        {
            OpenPauseMenu(); // Abrir el menú de pausa
        }

        // Desactivar el Controls Image con la tecla E
        if (isControlsImageActive && Input.GetKeyDown(KeyCode.E))
        {
            CloseControlsImage();
        }
    }

    // Abrir el menú de pausa
    public void OpenPauseMenu()
    {
        pauseMenuCanvas.SetActive(true);

        // Activar el hijo Pause Menu
        Transform pauseMenu = pauseMenuCanvas.transform.Find("Pause Menu");
        if (pauseMenu != null)
        {
            pauseMenu.gameObject.SetActive(true);
        }

        // Desactivar los otros hijos: Exit Background y Controls Image
        if (exitBackground != null)
        {
            exitBackground.SetActive(false);
        }

        if (controlsImage != null)
        {
            controlsImage.SetActive(false);
        }

        Debug.Log("Pause Menu ACTIVADO");

        // Congelar el movimiento del jugador
        playerController.GetComponent<Rigidbody>().isKinematic = true;

        // Detener el Timer
        timerManager.StopTimer();

        // Mostrar el cursor cuando el Pause Menu está activo
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Liberar el cursor
    }

    // Método llamado por el Resume Button desde el inspector
    public void OnClickResumeButton()
    {
        ClosePauseMenu();
    }

    // Método llamado por el Controls Button desde el inspector
    public void OnClickControlsButton()
    {
        if (controlsImage != null)
        {
            controlsImage.SetActive(true);
            isControlsImageActive = true;
        }

        Debug.Log("Controls Image ACTIVADO");
    }

    // Método llamado por el Exit Button desde el inspector
    public void OnClickExitButton()
    {
        if (exitBackground != null)
        {
            exitBackground.SetActive(true);
        }

        Debug.Log("Exit Background ACTIVADO");
    }

    // Método llamado por el Not Button desde el inspector
    public void OnClickNotButton()
    {
        if (exitBackground != null)
        {
            exitBackground.SetActive(false);
        }

        Debug.Log("Exit Background DESACTIVADO");
    }

    // Método llamado por el Yes Button desde el inspector
    public void OnClickYesButton()
    {
        Debug.Log("Cambiando a la escena Main Menu...");
        SceneManager.LoadScene("Main Menu");
    }

    // Cerrar el menú de pausa
    void ClosePauseMenu()
    {
        pauseMenuCanvas.SetActive(false);

        Debug.Log("Pause Menu DESACTIVADO");

        // Reactivar el movimiento del jugador
        playerController.GetComponent<Rigidbody>().isKinematic = false;

        // Reanudar el Timer
        timerManager.StartTimer();

        // Ocultar el cursor cuando el Pause Menu no está activo
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Desactivar el Controls Image
    public void CloseControlsImage()
    {
        if (controlsImage != null)
        {
            controlsImage.SetActive(false);
            isControlsImageActive = false;
        }

        Debug.Log("Controls Image DESACTIVADO");
    }

    // Método para comprobar si el menú de pausa está activo
    public bool IsPaused()
    {
        return pauseMenuCanvas.activeSelf;
    }
}