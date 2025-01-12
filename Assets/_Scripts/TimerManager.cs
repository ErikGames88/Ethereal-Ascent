using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TimerManager actualizado para iniciar el Timer después de cerrar el texto inicial
public class TimerManager : MonoBehaviour
{
    [SerializeField, Tooltip("Minutos iniciales del Timer")]
    private int initialMinutes = 15;

    [SerializeField, Tooltip("Segundos iniciales del Timer")]
    private int initialSeconds = 0;

    [SerializeField, Tooltip("Referencia al TextMeshPro para mostrar el Timer")]
    private TextMeshProUGUI timerText;

    [SerializeField, Tooltip("Referencia al GameOver script")]
    private GameOver gameOverScript;

    [SerializeField, Tooltip("Referencia al PauseMenuManager para verificar si el Pause Menu está activo")]
    private PauseMenuManager pauseMenuManager;

    private float remainingTimeInSeconds;
    private bool isTimerRunning = false;

    // Nuevo flag para pausar el Timer globalmente
    private bool isGloballyPaused = false;

    private void Start()
    {
        remainingTimeInSeconds = (initialMinutes * 60) + initialSeconds;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (isGloballyPaused || (pauseMenuManager != null && pauseMenuManager.IsPaused()))
        {
            StopTimer(); // Asegurarse de detener el Timer si está globalmente pausado o en el menú de pausa
            return;
        }

        if (isTimerRunning && remainingTimeInSeconds > 0)
        {
            remainingTimeInSeconds -= Time.deltaTime;

            if (remainingTimeInSeconds <= 0)
            {
                remainingTimeInSeconds = 0;
                StopTimer();
                gameOverScript.TriggerGameOver();
            }

            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTimeInSeconds / 60);
        int seconds = Mathf.FloorToInt(remainingTimeInSeconds % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartTimer()
    {
        if (!isTimerRunning && !isGloballyPaused)
        {
            isTimerRunning = true;
            Debug.Log("Timer iniciado.");
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer detenido.");
    }

    // Métodos para gestionar la pausa global
    public void PauseTimerGlobally()
    {
        isGloballyPaused = true;
        StopTimer();
        Debug.Log("Timer pausado globalmente.");
    }

    public void ResumeTimerGlobally()
    {
        isGloballyPaused = false;
        StartTimer();
        Debug.Log("Timer reanudado globalmente.");
    }
}
