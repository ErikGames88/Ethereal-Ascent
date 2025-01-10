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
        // Asegurarse de que el Letter Background y sus hijos estén desactivados al inicio
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

        // Asegurarse de que el cursor esté oculto al inicio
        SetCursorState(CursorLockMode.Locked, false);
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

        // Activar el Letter Background
        letterBackground.SetActive(true);

        // Activar el texto de "SALTAR TEXTO"
        keyEDismiss.SetActive(true);

        // Activar el texto de la First Note
        firstText.SetActive(true);

        // Reproducir el sonido de pasar página
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Reproduciendo sonido de pasar página.");
        }

        // Desactivar el crosshair mientras se muestra el texto
        if (crosshairManager != null)
        {
            crosshairManager.ShowCrosshair(false);
        }

        // Congelar al jugador, pero mantener el cursor oculto
        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false); // Congelar al jugador pero mantener el cursor oculto
        }

        isTextActive = true;

        Debug.Log("First Text y Key E Dismiss ACTIVADOS. Jugador congelado, cursor oculto.");
    }

    public void HideText()
    {
        if (letterBackground == null || keyEDismiss == null)
        {
            Debug.LogError("Letter Background o Key E Dismiss no están asignados en el TextManager.");
            return;
        }

        // Desactivar el Letter Background y sus hijos
        letterBackground.SetActive(false);
        keyEDismiss.SetActive(false);

        if (firstText != null)
        {
            firstText.SetActive(false);
        }

        // Reactivar el crosshair
        if (crosshairManager != null)
        {
            crosshairManager.ShowCrosshair(true);
        }

        // Descongelar al jugador
        if (playerLocked != null)
        {
            playerLocked.LockPlayer(false, false); // Descongelar y mantener el cursor oculto
        }

        isTextActive = false;

        Debug.Log("Texto y Letter Background DESACTIVADOS. Jugador descongelado, cursor oculto.");
    }

    private void SetCursorState(CursorLockMode lockMode, bool isVisible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = isVisible;
    }

    public bool IsTextActive()
    {
        return isTextActive; // Devuelve el estado actual del texto
    }
}