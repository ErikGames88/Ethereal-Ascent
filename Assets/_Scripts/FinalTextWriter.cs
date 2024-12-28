using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinalTextWriter : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al componente TextMeshPro del Final Text")]
    private TextMeshProUGUI finalText;

    [SerializeField, Tooltip("Tiempo entre letras al escribir (en segundos)")]
    private float typingSpeed = 0.05f;

    [SerializeField, Tooltip("Pausa entre párrafos (en segundos)")]
    private float paragraphPause = 3f;

    [SerializeField, Tooltip("Pausa específica entre el tercer y cuarto párrafo (en segundos)")]
    private float lastParagraphPause = 1f;

    [SerializeField, Tooltip("Referencia al Key E Image")]
    private GameObject keyEImage;

    [SerializeField, Tooltip("Duración del fade-in del Key E Image (en segundos)")]
    private float keyEFadeDuration = 2f;

    private string initialText = "";
    private string[] paragraphs;
    private bool isWriting = false;
    private bool canInteract = false; // Controla si el jugador puede pulsar E

    private void Awake()
    {
        if (finalText != null)
        {
            initialText = finalText.text;

            if (string.IsNullOrWhiteSpace(initialText))
            {
                Debug.LogError("El texto inicial está vacío en el Inspector.");
                return;
            }

            finalText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("No se asignó un componente TextMeshPro al Final Text.");
        }

        if (keyEImage != null)
        {
            keyEImage.SetActive(false);
        }
        else
        {
            Debug.LogError("No se asignó un GameObject al Key E Image.");
        }
    }

    private void Update()
    {
        // Detectar si el jugador puede interactuar y ha pulsado E
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacción final detectada. Transición al Main Menu.");
            GameManager.Instance.ChangeState(GameManager.GameState.MainMenu); // Llama al GameManager para cargar el Main Menu
        }
    }

    public void ActivateAndWriteText()
    {
        if (finalText == null)
        {
            Debug.LogError("No se puede activar el Final Text porque no está asignado.");
            return;
        }

        if (isWriting)
        {
            Debug.LogWarning("La escritura ya ha comenzado, ignorando llamada duplicada.");
            return;
        }

        isWriting = true;
        finalText.text = "";
        finalText.gameObject.SetActive(true);
        paragraphs = initialText.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

        if (paragraphs.Length == 0)
        {
            Debug.LogError("No se encontraron párrafos para escribir.");
            return;
        }

        StartCoroutine(WriteText());
    }

    private IEnumerator WriteText()
    {
        for (int i = 0; i < paragraphs.Length; i++)
        {
            yield return StartCoroutine(TypeParagraph(paragraphs[i]));

            if (i < paragraphs.Length - 1)
            {
                float pauseDuration = (i == 2) ? lastParagraphPause : paragraphPause;
                yield return new WaitForSeconds(pauseDuration);
            }
        }

        ActivateKeyEImage(); // Activar Key E Image al terminar el texto
    }

    private IEnumerator TypeParagraph(string paragraph)
    {
        foreach (char letter in paragraph)
        {
            finalText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        finalText.text += "\n\n";
    }

    private void ActivateKeyEImage()
    {
        if (keyEImage != null)
        {
            keyEImage.SetActive(true);
            StartCoroutine(FadeInKeyEImage());
        }
    }

    private IEnumerator FadeInKeyEImage()
    {
        Image image = keyEImage.GetComponent<Image>();
        TextMeshProUGUI text = keyEImage.GetComponentInChildren<TextMeshProUGUI>();

        if (image == null || text == null)
        {
            Debug.LogError("Key E Image o su texto no tienen componentes necesarios.");
            yield break;
        }

        float elapsedTime = 0f;
        Color imageColor = image.color;
        Color textColor = text.color;

        imageColor.a = 0f;
        textColor.a = 0f;
        image.color = imageColor;
        text.color = textColor;

        while (elapsedTime < keyEFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            imageColor.a = Mathf.Lerp(0f, 1f, elapsedTime / keyEFadeDuration);
            textColor.a = Mathf.Lerp(0f, 1f, elapsedTime / keyEFadeDuration);
            image.color = imageColor;
            text.color = textColor;

            yield return null;
        }

        imageColor.a = 1f;
        textColor.a = 1f;
        image.color = imageColor;
        text.color = textColor;

        Debug.Log("Key E Image activado completamente.");
        canInteract = true; // Permitir interacción con E
    }
}