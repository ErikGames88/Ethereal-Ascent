using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas; 
    [SerializeField] private PlayerController playerController; 
    [SerializeField] private MissionManager missionManager; 
    [SerializeField] private TimerManager timerManager; 
    [SerializeField] private Button resumeButton; 
    [SerializeField] private Button controlsButton; 
    [SerializeField] private Button exitButton; 
    [SerializeField] private Button notButton; 
    [SerializeField] private Button yesButton; 
    [SerializeField]private GameObject controlsImage; 
    [SerializeField] private GameObject exitBackground; 
    [SerializeField] private TextManager textManager;

    private bool isControlsImageActive = false; 



    void Start()
    {
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
        if ((missionManager != null && !missionManager.isMissionTextClosed) || 
            (textManager != null && textManager.IsTextActive()))
        {
            return; 
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenuCanvas.activeSelf && !InventoryToggle.isInventoryOpen)
        {
            OpenPauseMenu(); 
        }

        if (isControlsImageActive && Input.GetKeyDown(KeyCode.E))
        {
            CloseControlsImage();
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenuCanvas.SetActive(true);

        Transform pauseMenu = pauseMenuCanvas.transform.Find("Pause Menu");
        if (pauseMenu != null)
        {
            pauseMenu.gameObject.SetActive(true);
        }

        if (exitBackground != null)
        {
            exitBackground.SetActive(false);
        }

        if (controlsImage != null)
        {
            controlsImage.SetActive(false);
        }

        playerController.GetComponent<Rigidbody>().isKinematic = true;

        timerManager.StopTimer();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }

    public void OnClickResumeButton()
    {
        ClosePauseMenu();
    }
    public void OnClickControlsButton()
    {
        if (controlsImage != null)
        {
            controlsImage.SetActive(true);
            isControlsImageActive = true;
        }
    }

    public void OnClickExitButton()
    {
        if (exitBackground != null)
        {
            exitBackground.SetActive(true);
        }
    }

    public void OnClickNotButton()
    {
        if (exitBackground != null)
        {
            exitBackground.SetActive(false);
        }
    }

    public void OnClickYesButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    void ClosePauseMenu()
    {
        pauseMenuCanvas.SetActive(false);
        playerController.GetComponent<Rigidbody>().isKinematic = false;

        timerManager.StartTimer();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CloseControlsImage()
    {
        if (controlsImage != null)
        {
            controlsImage.SetActive(false);
            isControlsImageActive = false;
        }
    }
    public bool IsPaused()
    {
        return pauseMenuCanvas.activeSelf;
    }
}