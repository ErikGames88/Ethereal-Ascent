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

    [SerializeField, Tooltip("Referencia al Key E Image")]
    private GameObject keyEImage;

    private string[] paragraphs;

    private void Start()
    {
        // Obtener los párrafos desde el texto original
        paragraphs = finalText.text.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

        // Asegurarnos de que el texto esté vacío al inicio
        finalText.text = "";

        // Desactivar el Key E Image
        if (keyEImage != null) keyEImage.SetActive(false);

        // Iniciar la escritura progresiva
        StartCoroutine(WriteText());
    }

    private IEnumerator WriteText()
    {
        // Escribir cada párrafo con pausas
        foreach (string paragraph in paragraphs)
        {
            yield return StartCoroutine(TypeParagraph(paragraph));
            yield return new WaitForSeconds(paragraphPause);
        }

        // Mostrar el Key E Image al final
        if (keyEImage != null) keyEImage.SetActive(true);

        Debug.Log("Texto completo y Key E Image activado.");
    }

    private IEnumerator TypeParagraph(string paragraph)
    {
        foreach (char letter in paragraph)
        {
            finalText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Añadir un salto de línea después de cada párrafo
        finalText.text += "\n\n";
    }
}