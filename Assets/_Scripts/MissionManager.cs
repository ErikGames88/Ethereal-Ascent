using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al Text Background")]
    private GameObject textBackground;

    [SerializeField, Tooltip("Referencia al texto de misión")]
    private TextMeshProUGUI missionText;

    [SerializeField, Tooltip("Referencia al texto de dismiss (por ejemplo, 'Saltar')")]
    private TextMeshProUGUI dismissText;

    [SerializeField, Tooltip("Retraso en segundos para mostrar el texto de misión al iniciar la escena")]
    private float showDelay = 2f;

    [SerializeField, Tooltip("Referencia al PlayerLocked para manejar el bloqueo del jugador")]
    private PlayerLocked playerLocked;

    [SerializeField, Tooltip("Referencia al Canvas del inventario")]
    private GameObject inventoryCanvas;

    [SerializeField, Tooltip("Referencia al InventoryToggle para desactivar Tab")]
    private InventoryToggle inventoryToggle;

    [SerializeField, Tooltip("Referencia al HintTextManager para manejar textos de pista")]
    private HintTextManager hintTextManager;

    [SerializeField, Tooltip("Referencia al Timer (GameObject o Canvas)")]
    private GameObject timerObject;

    [SerializeField, Tooltip("Referencia al Timer Manager")]
    private TimerManager timerManager;

    private bool isMissionTextActive = false;
    private bool wasInventoryOpen = false;
    public bool isMissionTextClosed = false; // Nueva variable para controlar si el texto se ha cerrado

    private void Start()
    {
        if (textBackground != null && missionText != null && dismissText != null && playerLocked != null && inventoryCanvas != null && inventoryToggle != null && hintTextManager != null && timerObject != null)
        {
            textBackground.SetActive(false); // Asegurarse de que todo empieza desactivado
            timerObject.SetActive(false); // Asegurarse de que el Timer está desactivado
            Invoke(nameof(ShowMissionText), showDelay); // Mostrar el texto tras un retraso
        }
        else
        {
            Debug.LogError("Faltan referencias en el Inspector.");
        }
    }

    private void Update()
    {
        // Detectar si el jugador puede cerrar el texto pulsando E
        if (isMissionTextActive && Input.GetKeyDown(KeyCode.E))
        {
            CloseMissionText();
        }
    }

    private void ShowMissionText()
    {
        if (textBackground != null && missionText != null && dismissText != null)
        {
            // Verificar si el inventario estaba abierto
            if (inventoryCanvas.activeSelf)
            {
                wasInventoryOpen = true;
                inventoryCanvas.SetActive(false); // Ocultar el inventario
            }

            // Desactivar Tab
            inventoryToggle.EnableInventoryToggle(false);

            // Activar el fondo y el texto de misión
            textBackground.SetActive(true);
            missionText.gameObject.SetActive(true);
            dismissText.gameObject.SetActive(true);

            // Bloquear al jugador mientras el texto está activo
            playerLocked.LockPlayer(true);

            isMissionTextActive = true;

            Debug.Log("Texto de misión mostrado y jugador bloqueado.");
        }
    }

    private void CloseMissionText()
    {
        if (textBackground != null && missionText != null && dismissText != null)
        {
            // Desactivar el fondo y los textos
            textBackground.SetActive(false);
            missionText.gameObject.SetActive(false);
            dismissText.gameObject.SetActive(false);

            // Reactivar Tab
            inventoryToggle.EnableInventoryToggle(true);

            // Reactivar el inventario si estaba abierto antes
            if (wasInventoryOpen)
            {
                inventoryCanvas.SetActive(true);
                wasInventoryOpen = false;
            }

            // Desbloquear al jugador
            playerLocked.LockPlayer(false);

            isMissionTextActive = false;
            isMissionTextClosed = true; // Marcar que el texto de misión ha sido cerrado

            Debug.Log("Texto de misión cerrado y jugador desbloqueado.");

            // Mostrar el Mission Hint Text
            hintTextManager.ShowMissionHintText();

            // Activar el Timer SOLO después de que se haya cerrado el Mission Text
            timerObject.SetActive(true);
            timerManager.StartTimer(); // Iniciar la cuenta atrás
            Debug.Log("Cuenta atrás del Timer iniciada.");
        }
    }
}