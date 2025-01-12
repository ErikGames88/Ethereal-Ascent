using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextManager : MonoBehaviour
{
    [SerializeField] private GameObject letterBackground;
    [SerializeField] private GameObject keyEDismiss;
    [SerializeField] private GameObject firstText;
    [SerializeField] private GameObject noticeboardBackground;
    [SerializeField] private GameObject cathedralBoardText;
    [SerializeField] private GameObject textBackground;
    [SerializeField] private GameObject hospitalDoorText;
    [SerializeField] private CrosshairManager crosshairManager;
    [SerializeField] private PlayerLocked playerLocked;
    [SerializeField]private TimerManager timerManager; 
    [SerializeField] private AudioSource audioSource;
    private bool isTextActive = false;

    public static event Action OnTextHidden;


    void Start()
    {
        if (letterBackground != null)
        {
            letterBackground.SetActive(false);
        }

        if (keyEDismiss != null)
        {
            keyEDismiss.SetActive(false);
        }

        if (firstText != null)
        {
            firstText.SetActive(false);
        }

        if (noticeboardBackground != null)
        {
            noticeboardBackground.SetActive(false);
        }

        if (cathedralBoardText != null)
        {
            cathedralBoardText.SetActive(false);
        }

        if (textBackground != null)
        {
            textBackground.SetActive(false);
        }

        if (hospitalDoorText != null)
        {
            hospitalDoorText.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isTextActive && Input.GetKeyDown(KeyCode.E))
        {
            HideText();
        }
    }

    public void ShowFirstText()
    {
        if (letterBackground == null || keyEDismiss == null || firstText == null)
        {
            return;
        }

        letterBackground.SetActive(true);
        keyEDismiss.SetActive(true);
        firstText.SetActive(true);

        PlaySoundAndLockUI();
        PauseTimer(); 
    }

    public void ShowCathedralBoardText()
    {
        if (noticeboardBackground == null || keyEDismiss == null || cathedralBoardText == null)
        {
            return;
        }

        noticeboardBackground.SetActive(true);
        keyEDismiss.SetActive(true);
        cathedralBoardText.SetActive(true);

        PlaySoundAndLockUI();
        PauseTimer();
    }

    public void ShowHospitalDoorText()
    {
        if (textBackground == null || keyEDismiss == null || hospitalDoorText == null)
        {
            return;
        }

        textBackground.SetActive(true);
        keyEDismiss.SetActive(true);
        hospitalDoorText.SetActive(true);

        LockUI();
        PauseTimer(); 
    }

    public void HideText()
    {
        if (letterBackground != null)
        {
            letterBackground.SetActive(false);
        }

        if (noticeboardBackground != null)
        {
            noticeboardBackground.SetActive(false);
        }

        if (textBackground != null)
        {
            textBackground.SetActive(false);
        }

        if (keyEDismiss != null)
        {
            keyEDismiss.SetActive(false);
        }

        if (firstText != null)
        {
            firstText.SetActive(false);
        }

        if (cathedralBoardText != null)
        {
            cathedralBoardText.SetActive(false);
        }

        if (hospitalDoorText != null)
        {
            hospitalDoorText.SetActive(false);
        }

        UnlockUI();
        ResumeTimer(); 

        if (OnTextHidden != null)
        {
            OnTextHidden.Invoke(); 
        }
    }

    private void PlaySoundAndLockUI()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        LockUI();
    }

    private void LockUI()
    {
        if (crosshairManager != null)
        {
            crosshairManager.ShowCrosshair(false);
        }

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false); 
        }

        isTextActive = true;
    }

    private void UnlockUI()
    {
        if (crosshairManager != null)
        {
            crosshairManager.ShowCrosshair(true);
        }

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(false, false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isTextActive = false;
    }

    private void PauseTimer()
    {
        if (timerManager != null)
        {
            timerManager.PauseTimerGlobally();
        }
    }

    private void ResumeTimer()
    {
        if (timerManager != null)
        {
            timerManager.ResumeTimerGlobally();
        }
    }

    public bool IsTextActive()
    {
        return isTextActive;
    }
}