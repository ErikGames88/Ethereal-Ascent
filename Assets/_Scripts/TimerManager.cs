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
    private GameOver gameOverScript; // Referencia al GameOver

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

        // Suscribirse al evento de cierre del texto inicial
        TextManager.OnTextHidden += StartTimer;
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento al destruir este objeto
        TextManager.OnTextHidden -= StartTimer;
    }

    private void Update()
    {
        // Pausar el Timer si el Pause Menu está activo
        if (pauseMenuManager != null && pauseMenuManager.IsPaused())
        {
            if (isTimerRunning)
            {
                StopTimer();
                Debug.Log("Pause Menu activo: Timer detenido.");
            }
            return;
        }

        // Actualizar el Timer si está corriendo
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
        if (!isTimerRunning)
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
}
