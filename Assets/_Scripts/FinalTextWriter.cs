using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
}