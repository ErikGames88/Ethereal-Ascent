using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoreScene : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private TextMeshProUGUI loreText;  
    [SerializeField] private RawImage loreImage;        
    [SerializeField] private float typingSpeed = 0.05f; 
    [SerializeField] private AudioSource audioSource;  
    private string fullText;         

    

    void Start()
    {
        Cursor.visible = false;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Maze Scene");
        asyncLoad.allowSceneActivation = false;  

        fullText = loreText.text;  
        loreText.text = "";        
        StartCoroutine(FadeInImage(asyncLoad));  
    }

    IEnumerator FadeInImage(AsyncOperation asyncLoad)
    {
        float timeElapsed = 0f;
        float duration = 4f;  

        Color startColor = loreImage.color;
        startColor.a = 0;
        loreImage.color = startColor;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timeElapsed / duration);
            loreImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        audioSource.Play();
        StartCoroutine(TypeText(asyncLoad));  
    }

    IEnumerator TypeText(AsyncOperation asyncLoad)
    {
        foreach (char letter in fullText.ToCharArray())
        {
            loreText.text += letter;  
            yield return new WaitForSeconds(typingSpeed);  
        }

        yield return new WaitForSeconds(3f);

        StartCoroutine(FadeOutImage(asyncLoad));
    }

    IEnumerator FadeOutImage(AsyncOperation asyncLoad)
    {
        loreText.text = "";  
        loreText.gameObject.SetActive(false);  

        float timeElapsed = 0f;
        float duration = 2f;  

        Color startColor = loreImage.color;
        startColor.a = 1;
        loreImage.color = startColor;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            loreImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;  
        }

        loadingImage.gameObject.SetActive(true);  
        asyncLoad.allowSceneActivation = true;  
    }
}