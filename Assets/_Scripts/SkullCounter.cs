using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkullCounter : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al TextMeshPro para mostrar el contador")]
    private TextMeshProUGUI counterText;

    [SerializeField, Tooltip("Referencia al Key Hint Text")]
    private GameObject keyHintText;

    [SerializeField, Tooltip("Referencia al TextBackground")]
    private GameObject textBackground;

    [SerializeField, Tooltip("Referencia al PlayerLocked para gestionar el bloqueo del jugador")]
    private PlayerLocked playerLocked;

    private int skullCount = 0;
    private bool isKeyHintActive = false;

    void Start()
    {
        counterText.text = "0";
        keyHintText.SetActive(false);
        textBackground.SetActive(false);

        if (playerLocked == null)
        {
            playerLocked = FindObjectOfType<PlayerLocked>();
            if (playerLocked == null)
            {
                Debug.LogError("PlayerLocked no asignado y no se encontró en la escena.");
            }
        }
    }

    void Update()
    {
        if (isKeyHintActive && Input.GetKeyDown(KeyCode.E))
        {
            HideKeyHint();
        }
    }

    public void AddSkull()
    {
        skullCount++;
        Debug.Log($"Cráneos recogidos: {skullCount}");

        if (skullCount == 6)
        {
            ShowKeyHint();
        }

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        counterText.text = skullCount.ToString();
    }

    private void ShowKeyHint()
    {
        keyHintText.SetActive(true);
        textBackground.SetActive(true);
        isKeyHintActive = true;

        Debug.Log("Pista de la llave mostrada. Bloqueando movimiento.");

        // Bloquear al jugador pero mantener el cursor oculto
        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false); // No mostrar el cursor
        }
    }

    private void HideKeyHint()
    {
        keyHintText.SetActive(false);
        textBackground.SetActive(false);
        isKeyHintActive = false;

        Debug.Log("Pista de la llave oculta. Restaurando movimiento.");

        // Desbloquear al jugador y restaurar el estado habitual del cursor
        if (playerLocked != null)
        {
            playerLocked.LockPlayer(false);
        }
    }
}