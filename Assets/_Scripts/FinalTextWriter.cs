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

    private string initialText = ""; // Almacenará el texto inicial
    private string[] paragraphs;

    private bool isWriting = false; // Nueva bandera para evitar múltiples escrituras

    private void Awake()
    {
        if (finalText != null)
        {
            // Guardar el texto inicial antes de limpiar o desactivar
            initialText = finalText.text;

            // Asegurar que el texto inicial no esté vacío
            if (string.IsNullOrWhiteSpace(initialText))
            {
                Debug.LogError("El texto inicial está vacío en el Inspector.");
                return;
            }

            finalText.gameObject.SetActive(false); // Desactivar inicialmente
            Debug.Log($"Texto inicial guardado correctamente: {initialText}");
        }
        else
        {
            Debug.LogError("No se asignó un componente TextMeshPro al Final Text.");
        }

        if (keyEImage != null)
        {
            keyEImage.SetActive(false); // Desactivar Key E Image inicialmente
        }
        else
        {
            Debug.LogError("No se asignó un GameObject al Key E Image.");
        }
    }

    public void ActivateAndWriteText()
    {
        if (finalText == null)
        {
            Debug.LogError("No se puede activar el Final Text porque no está asignado.");
            return;
        }

        if (isWriting) // Si ya está escribiendo, salir
        {
            Debug.LogWarning("La escritura ya ha comenzado, ignorando llamada duplicada.");
            return;
        }

        isWriting = true; // Marcar como escribiendo

        // Restaurar el texto inicial y activar el objeto
        finalText.text = "";
        finalText.gameObject.SetActive(true);
        Debug.Log("Final Text activado y listo para escribir.");

        // Dividir en párrafos
        paragraphs = initialText.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

        if (paragraphs.Length == 0)
        {
            Debug.LogError("No se encontraron párrafos para escribir.");
            return;
        }

        Debug.Log($"Párrafos encontrados: {paragraphs.Length}");
        StartCoroutine(WriteText());
    }

    private IEnumerator WriteText()
    {
        for (int i = 0; i < paragraphs.Length; i++)
        {
            Debug.Log($"Escribiendo párrafo {i + 1}: {paragraphs[i]}");

            yield return StartCoroutine(TypeParagraph(paragraphs[i]));

            // Pausa específica para el tercer párrafo
            if (i < paragraphs.Length - 1)
            {
                float pauseDuration = (i == 2) ? lastParagraphPause : paragraphPause;
                Debug.Log($"Pausa entre párrafo {i + 1} y párrafo {i + 2}: {pauseDuration} segundos.");
                yield return new WaitForSeconds(pauseDuration);
            }
        }

        Debug.Log("Escritura de texto completada.");
        ActivateKeyEImage(); // Activar Key E Image al terminar el texto
    }

    private IEnumerator TypeParagraph(string paragraph)
    {
        foreach (char letter in paragraph)
        {
            finalText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Añadir salto de línea después del párrafo
        finalText.text += "\n\n";
        Debug.Log($"Párrafo completado: {paragraph}");
    }

    private void ActivateKeyEImage()
    {
        if (keyEImage != null)
        {
            keyEImage.SetActive(true); // Activar Key E Image
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

        // Inicializar opacidad a 0
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
    }
}