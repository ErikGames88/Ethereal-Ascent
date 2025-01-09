using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoreScene : MonoBehaviour
{
    public TextMeshProUGUI loreText;  // Referencia al TextMeshPro para el texto
    public RawImage loreImage;        // Referencia a la RawImage para el fondo (Lore Image)
    public float typingSpeed = 0.05f; // Velocidad de escritura del texto
    public AudioSource audioSource;  // Referencia al AudioSource para reproducir la voz
    private string fullText;         // Almacenar el texto completo

    public Image loadingImage;

    void Start()
    {
        Cursor.visible = false;

        // Iniciar la carga de la Maze Scene en segundo plano
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Maze Scene");
        asyncLoad.allowSceneActivation = false;  // No activar la escena inmediatamente

        fullText = loreText.text;  // Obtener el texto completo del Inspector
        loreText.text = "";        // Deja el texto vacío para escribirlo poco a poco
        StartCoroutine(FadeInImage(asyncLoad));  // Iniciar la corutina para el fade-in de la imagen
    }

    IEnumerator FadeInImage(AsyncOperation asyncLoad)
    {
        float timeElapsed = 0f;
        float duration = 4f;  // Duración del fade-in (4 segundos)

        // Asegurarse de que la imagen comienza completamente transparente
        Color startColor = loreImage.color;
        startColor.a = 0;
        loreImage.color = startColor;

        // Gradualmente aumentar la opacidad de la imagen
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timeElapsed / duration);
            loreImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Al finalizar el fade-in, comenzar a reproducir el audio y escribir el texto
        audioSource.Play();
        StartCoroutine(TypeText(asyncLoad));  // Iniciar el proceso de escritura del texto
    }

    IEnumerator TypeText(AsyncOperation asyncLoad)
    {
        foreach (char letter in fullText.ToCharArray())
        {
            loreText.text += letter;  // Añade una letra a la vez
            yield return new WaitForSeconds(typingSpeed);  // Espera antes de escribir la siguiente letra
        }

        // Cuando termine de escribir el texto, espera 3 segundos antes de hacer el fade-out
        yield return new WaitForSeconds(3f);

        // Iniciar el fade-out de la imagen (de opacidad 1 a 0 en 2 segundos)
        StartCoroutine(FadeOutImage(asyncLoad));
    }

    IEnumerator FadeOutImage(AsyncOperation asyncLoad)
    {
        // Desactivar el Lore Text cuando comience el fade-out
        loreText.text = "";  // Elimina el texto de la pantalla
        loreText.gameObject.SetActive(false);  // Desactiva el objeto Lore Text

        float timeElapsed = 0f;
        float duration = 2f;  // Duración del fade-out (2 segundos)

        // Asegurarse de que la imagen comienza completamente opaca
        Color startColor = loreImage.color;
        startColor.a = 1;
        loreImage.color = startColor;

        // Gradualmente disminuir la opacidad de la imagen
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            loreImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;  // Mantener el frame sin bloquear
        }

        // Una vez que el fade-out termine, activamos el Loading Image
        loadingImage.gameObject.SetActive(true);  // Activar el Loading Image

        // Iniciar la carga de la escena
        asyncLoad.allowSceneActivation = true;  // Activar la escena cuando termine el fade-out
    }
}