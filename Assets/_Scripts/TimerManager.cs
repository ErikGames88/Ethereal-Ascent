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

    private float remainingTimeInSeconds;
    private bool isTimerRunning = false;

    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("No se asignó un TextMeshPro al Timer Manager.");
            return;
        }

        if (gameOverScript == null)
        {
            Debug.LogError("No se asignó un GameOver script al Timer Manager.");
            return;
        }

        // Convertir el tiempo inicial a segundos
        remainingTimeInSeconds = (initialMinutes * 60) + initialSeconds;

        // Actualizar el texto inicial del Timer
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (isTimerRunning && remainingTimeInSeconds > 0)
        {
            // Restar tiempo en tiempo real
            remainingTimeInSeconds -= Time.deltaTime;

            // Evitar valores negativos
            if (remainingTimeInSeconds <= 0)
            {
                remainingTimeInSeconds = 0;
                StopTimer(); // Detener el Timer al llegar a 0
                gameOverScript.TriggerGameOver(); // Activar la lógica de Game Over
            }

            // Actualizar el texto del Timer en pantalla
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
        Debug.Log("Timer detenido. Se llegó a 00:00.");
    }
}
