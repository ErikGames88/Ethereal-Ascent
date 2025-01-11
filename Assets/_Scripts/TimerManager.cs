using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField, Tooltip("Minutos iniciales del Timer")]
    private int initialMinutes = 15;

    [SerializeField, Tooltip("Segundos iniciales del Timer")]
    private int initialSeconds = 0;

    [SerializeField, Tooltip("Referencia al TextMeshPro para mostrar el Timer")]
    private TextMeshProUGUI timerText;

    [SerializeField, Tooltip("Referencia al GameOver script")]
    private GameOver gameOverScript; // Referencia al GameOver

    [SerializeField, Tooltip("Referencia al TextManager para verificar si hay texto activo")]
    private TextManager textManager;

    [SerializeField, Tooltip("Referencia al PauseMenuManager para verificar si el Pause Menu está activo")]
    private PauseMenuManager pauseMenuManager;

    private float remainingTimeInSeconds;
    private bool isTimerRunning = false; // Controla si el Timer está en ejecución

    private void Start()
    {
        // Convertir el tiempo inicial a segundos
        remainingTimeInSeconds = (initialMinutes * 60) + initialSeconds;

        // Actualizar el texto inicial del Timer
        UpdateTimerDisplay();
    }

    private void Update()
    {
        // *** PRIORIDAD 1: Detener toda lógica si el Pause Menu está activo ***
        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            if (isTimerRunning)
            {
                StopTimer(); // Garantizamos que el Timer no siga corriendo
                Debug.Log("Pause Menu activo: Timer detenido.");
            }
            return; // Salir de Update completamente
        }

        // *** PRIORIDAD 2: Detener el Timer si hay textos activos ***
        if (textManager != null && textManager.IsTextActive())
        {
            if (isTimerRunning)
            {
                StopTimer();
                Debug.Log("Timer pausado por texto activo.");
            }
            return; // Salir de Update completamente
        }

        // *** Reanudar el Timer solo si no hay textos y el Pause Menu no está activo ***
        if (!isTimerRunning && (textManager == null || !textManager.IsTextActive()))
        {
            StartTimer();
            Debug.Log("Timer reanudado.");
        }

        // Actualizar el Timer si está corriendo
        if (isTimerRunning && remainingTimeInSeconds > 0)
        {
            remainingTimeInSeconds -= Time.deltaTime;

            // Si el tiempo llega a 0, activar Game Over
            if (remainingTimeInSeconds <= 0)
            {
                remainingTimeInSeconds = 0;
                StopTimer();
                gameOverScript.TriggerGameOver();
            }

            // Actualizar el texto del Timer
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTimeInSeconds / 60);
        int seconds = Mathf.FloorToInt(remainingTimeInSeconds % 60);

        // Formatear el texto como mm:ss
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        Debug.Log("Timer iniciado.");
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer detenido.");
    }
}