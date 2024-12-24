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

    private int skullCount = 0;
    private bool isKeyHintActive = false;

    void Start()
    {
        counterText.text = "0";
        keyHintText.SetActive(false);
        textBackground.SetActive(false);
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

        // Llama a ShowKeyHint solo después de confirmar que son 6 cráneos
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
        Debug.Log("Pista de la llave mostrada.");
    }

    private void HideKeyHint()
    {
        keyHintText.SetActive(false);
        textBackground.SetActive(false);
        isKeyHintActive = false;
        Debug.Log("Pista de la llave oculta.");
    }
}