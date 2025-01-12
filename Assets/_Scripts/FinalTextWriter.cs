using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalTextWriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float paragraphPause = 3f;
    [SerializeField] private float lastParagraphPause = 1f;
    [SerializeField] private GameObject keyEImage;
    [SerializeField] private float keyEFadeDuration = 2f;

    private string initialText = "";
    private string[] paragraphs;
    private bool isWriting = false;
    private bool canInteract = false; 

    private void Awake()
    {
        if (finalText != null)
        {
            initialText = finalText.text;

            if (string.IsNullOrWhiteSpace(initialText))
            {
                return;
            }

            finalText.gameObject.SetActive(false);
        }

        if (keyEImage != null)
        {
            keyEImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Main Menu"); 
        }
    }

    public void ActivateAndWriteText()
    {
        if (finalText == null)
        {
            return;
        }

        if (isWriting)
        {
            return;
        }

        isWriting = true;
        finalText.text = "";
        finalText.gameObject.SetActive(true);
        paragraphs = initialText.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

        if (paragraphs.Length == 0)
        {
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
                float pauseDuration;
                if (i == 2)
                {
                    pauseDuration = lastParagraphPause;
                }
                else
                {
                    pauseDuration = paragraphPause;
                }

                yield return new WaitForSeconds(pauseDuration);
            }
        }

        ActivateKeyEImage(); 
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

        canInteract = true; 
    }
}