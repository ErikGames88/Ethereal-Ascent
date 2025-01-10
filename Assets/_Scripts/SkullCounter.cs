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

    [SerializeField, Tooltip("Referencia a la Boss Door que bloquea la catedral")]
    private GameObject bossDoor;

    private int skullCount = 0;
    private bool isKeyHintActive = false;

    void Start()
    {
        counterText.text = "0";
        keyHintText.SetActive(false);
        textBackground.SetActive(false);

        if (cathedralKey != null)
        {
            cathedralKey.SetActive(false);
        }

        if (bossDoor != null)
        {
            bossDoor.SetActive(true); // La puerta está activa por defecto
        }

        if (playerLocked == null)
        {
            playerLocked = FindObjectOfType<PlayerLocked>();
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

        if (skullCount == 6)
        {
            ShowKeyHint();
            ActivateKey();
            DeactivateBossDoor(); // Desactivar la Boss Door
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

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false);
        }
    }

    private void HideKeyHint()
    {
        keyHintText.SetActive(false);
        textBackground.SetActive(false);
        isKeyHintActive = false;

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(false);
        }
    }

    private void ActivateKey()
    {
        if (cathedralKey != null)
        {
            cathedralKey.SetActive(true);
        }
    }

    private void DeactivateBossDoor()
    {
        if (bossDoor != null)
        {
            bossDoor.SetActive(false);
            Debug.Log("Boss Door desactivada. Acceso liberado.");
        }
    }
}