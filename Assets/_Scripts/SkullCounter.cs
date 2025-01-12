using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkullCounter : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al TextMeshPro para mostrar el contador")]
    private TextMeshProUGUI counterText;

    [SerializeField, Tooltip("Referencia al Cathedral Key Text")]
    private GameObject cathedralKeyText;

    [SerializeField, Tooltip("Referencia al Cathedral Key Hint Text")]
    private GameObject cathedralKeyHintText; // Nueva referencia

    [SerializeField, Tooltip("Referencia al TextBackground")]
    private GameObject textBackground;

    [SerializeField, Tooltip("Referencia al PlayerLocked para gestionar el bloqueo del jugador")]
    private PlayerLocked playerLocked;

    [SerializeField, Tooltip("Referencia al HintTextManager para manejar textos con fade-in y fade-out")]
    private HintTextManager hintTextManager; // Nueva referencia

    [SerializeField, Tooltip("Referencia a la llave de la catedral")]
    private GameObject cathedralKey;

    [SerializeField, Tooltip("Referencia a la Boss Door que bloquea la catedral")]
    private GameObject bossDoor;

    private int skullCount = 0;
    private bool isKeyHintActive = false;

    void Start()
    {
        counterText.text = "0";
        cathedralKeyText.SetActive(false);
        textBackground.SetActive(false);

        if (cathedralKeyHintText != null)
        {
            cathedralKeyHintText.SetActive(false); // Asegurarse de que empieza desactivado
        }

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
        cathedralKeyText.SetActive(true);
        textBackground.SetActive(true);
        isKeyHintActive = true;

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false);
        }
    }

    private void HideKeyHint()
    {
        cathedralKeyText.SetActive(false);
        textBackground.SetActive(false);
        isKeyHintActive = false;

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(false);
        }

        // Activar el Cathedral Key Hint Text con fade-in y fade-out
        if (hintTextManager != null)
        {
            hintTextManager.ShowCathedralHintText();
            Debug.Log("Llamando a ShowCathedralHintText desde SkullCounter.");
        }
        else
        {
            Debug.LogError("HintTextManager no está asignado en SkullCounter.");
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