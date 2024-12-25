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

    [SerializeField, Tooltip("Referencia a la llave de la catedral")]
    private GameObject cathedralKey;

    private int skullCount = 0;
    private bool isKeyHintActive = false;

    void Start()
    {
        counterText.text = "0";
        keyHintText.SetActive(false);
        textBackground.SetActive(false);

        if (cathedralKey != null)
        {
            Debug.Log($"Cathedral Key asignada: {cathedralKey.name}");
            cathedralKey.SetActive(false);
            Debug.Log("Cathedral Key desactivada al inicio.");
        }
        else
        {
            Debug.LogError("Cathedral Key no está asignada en el Inspector.");
        }

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
            Debug.Log("Se han recogido los 6 cráneos. Mostrando Key Hint y activando la llave.");
            ShowKeyHint();
            ActivateKey();
        }

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        Debug.Log($"Actualizando contador de cráneos: {skullCount}");
        counterText.text = skullCount.ToString();
    }

    private void ShowKeyHint()
    {
        keyHintText.SetActive(true);
        textBackground.SetActive(true);
        isKeyHintActive = true;

        Debug.Log("Pista de la llave mostrada en pantalla.");

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false); // No mostrar el cursor
            Debug.Log("Movimiento del jugador bloqueado.");
        }
        else
        {
            Debug.LogWarning("PlayerLocked no está asignado. No se pudo bloquear al jugador.");
        }
    }

    private void HideKeyHint()
    {
        keyHintText.SetActive(false);
        textBackground.SetActive(false);
        isKeyHintActive = false;

        Debug.Log("Pista de la llave oculta.");

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(false);
            Debug.Log("Movimiento del jugador restaurado.");
        }
    }

    private void ActivateKey()
    {
        if (cathedralKey != null)
        {
            Debug.Log($"Intentando activar la llave: {cathedralKey.name}");
            cathedralKey.SetActive(true);
            if (cathedralKey.activeSelf)
            {
                Debug.Log("Cathedral Key activada correctamente.");
            }
            else
            {
                Debug.LogError("Se intentó activar la Cathedral Key pero sigue desactivada.");
            }
        }
        else
        {
            Debug.LogError("Cathedral Key no está asignada o no se pudo activar.");
        }
    }
}