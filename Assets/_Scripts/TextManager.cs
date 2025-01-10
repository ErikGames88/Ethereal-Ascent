using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [Header("Referencias del Text Canvas")]
    [SerializeField, Tooltip("Referencia al Letter Background")]
    private GameObject letterBackground;

    [SerializeField, Tooltip("Texto para cerrar (Key E Dismiss)")]
    private GameObject keyEDismiss;

    [SerializeField, Tooltip("Texto de la First Note")]
    private GameObject firstText;

    [SerializeField, Tooltip("Referencia al Noticeboard Background")]
    private GameObject noticeboardBackground;

    [SerializeField, Tooltip("Texto de la Cathedral Board")]
    private GameObject cathedralBoardText;

    [Header("Otros")]
    [SerializeField, Tooltip("Referencia al Crosshair Manager")]
    private CrosshairManager crosshairManager;

    [SerializeField, Tooltip("Referencia al PlayerLocked para congelar/descongelar al jugador")]
    private PlayerLocked playerLocked;

    private bool isTextActive = false; // Estado para saber si el texto está activo

    [SerializeField, Tooltip("Referencia al AudioSource para reproducir sonidos")]
    private AudioSource audioSource;

    void Start()
    {
        // Desactivar elementos al inicio
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Detectar si se presiona la tecla E para cerrar el texto activo
        if (isTextActive && Input.GetKeyDown(KeyCode.E))
        {
            HideText();
        }
    }

    public void ShowFirstText()
    {
        if (letterBackground == null || keyEDismiss == null || firstText == null)
        {
            Debug.LogError("Alguna referencia está sin asignar en el TextManager.");
            return;
        }

        letterBackground.SetActive(true);
        keyEDismiss.SetActive(true);
        firstText.SetActive(true);

        PlaySoundAndLockUI();
        Debug.Log("First Text y Key E Dismiss ACTIVADOS.");
    }

    public void ShowCathedralBoardText()
    {
        if (noticeboardBackground == null || keyEDismiss == null || cathedralBoardText == null)
        {
            Debug.LogError("Alguna referencia está sin asignar en el TextManager.");
            return;
        }

        noticeboardBackground.SetActive(true);
        keyEDismiss.SetActive(true);
        cathedralBoardText.SetActive(true);

        PlaySoundAndLockUI();
        Debug.Log("Cathedral Board Text y Key E Dismiss ACTIVADOS.");
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

        UnlockUI();
        Debug.Log("Textos y fondos DESACTIVADOS.");
    }

    private void PlaySoundAndLockUI()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (crosshairManager != null)
        {
            crosshairManager.ShowCrosshair(false);
        }

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false); // Congelar al jugador pero no mostrar el cursor
        }

        // Comentamos estas líneas para evitar que el cursor se active
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;

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

    public bool IsTextActive()
    {
        return isTextActive;
    }
}