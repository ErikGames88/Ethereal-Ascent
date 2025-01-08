using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoreScene : MonoBehaviour
{
    public TextMeshProUGUI loreText;  // Referencia al TextMeshPro para el texto
    public float typingSpeed = 0.05f; // Velocidad de escritura del texto
    public AudioSource audioSource;  // Referencia al AudioSource para reproducir la voz

    private string fullText;  // Almacenar el texto completo

    void Start()
    {
        fullText = loreText.text;  // Asignar el texto completo de TextMeshPro al script
        loreText.text = "";        // Limpiar el texto inicialmente
        audioSource.Play();        // Reproducir el audio de la voz
        StartCoroutine(TypeText());  // Iniciar el proceso de escritura
    }

    // Corutina para escribir el texto lentamente
    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())  // Usamos el texto completo desde el inspector
        {
            loreText.text += letter;  // Añade una letra a la vez
            yield return new WaitForSeconds(typingSpeed);  // Espera antes de escribir la siguiente letra
        }
    }
}