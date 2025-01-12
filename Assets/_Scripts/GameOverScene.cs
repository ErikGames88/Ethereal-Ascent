using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    [SerializeField] private GameObject redBackground;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private float panelFadeDuration = 3f;
    [SerializeField] private float textFadeDuration = 2f;
    [SerializeField] private float panelActivationDelay = 1f;
    [SerializeField]private float textActivationDelay = 2f;
    [SerializeField]private float buttonsActivationDelay = 2f;
    [SerializeField] private GameObject loadingImage; 

    private Image panelImage; 
    private TextMeshProUGUI gameOverText; 
    private List<Button> buttons = new List<Button>(); 
    

    private void Awake()
    {
        if (redBackground == null)
        {
            redBackground = GameObject.Find("Red Background");
        }

        if (gameOverPanel == null)
        {
            gameOverPanel = GameObject.Find("Game Over Panel");
        }
        else
        {
            panelImage = gameOverPanel.GetComponent<Image>();

            Transform textTransform = gameOverPanel.transform.Find("Game Over Text");
            if (textTransform != null)
            {
                gameOverText = textTransform.GetComponent<TextMeshProUGUI>();
            }

            foreach (Transform child in gameOverPanel.transform)
            {
                Button button = child.GetComponent<Button>();
                if (button != null)
                {
                    buttons.Add(button);
                }
            }

            if (buttons.Count < 3)
            {
                // Just in case for over three buttons (not yet)
            }
        }

        if (loadingImage != null)
        {
            loadingImage.SetActive(false); 
        }
    }

    private void Start()
    {
        SetCursorVisibility(false);

        if (redBackground != null)
        {
            redBackground.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            DisablePanelChildren();
        }

        StartCoroutine(ShowGameOverPanelWithFade());
    }

    private void DisablePanelChildren()
    {
        foreach (Transform child in gameOverPanel.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowGameOverPanelWithFade()
    {
        yield return new WaitForSeconds(panelActivationDelay);

        if (gameOverPanel != null && panelImage != null)
        {
            gameOverPanel.SetActive(true);
            Color panelColor = panelImage.color;
            panelColor.a = 0f;
            panelImage.color = panelColor;

            float elapsedTime = 0f;
            while (elapsedTime < panelFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                panelColor.a = Mathf.Clamp01(elapsedTime / panelFadeDuration);
                panelImage.color = panelColor;
                yield return null;
            }

            panelColor.a = 1f;
            panelImage.color = panelColor;

            StartCoroutine(ShowGameOverTextWithFade());
        }
    }

    private IEnumerator ShowGameOverTextWithFade()
    {
        yield return new WaitForSeconds(textActivationDelay);

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            Color textColor = gameOverText.color;
            textColor.a = 0f;
            gameOverText.color = textColor;

            float elapsedTime = 0f;
            while (elapsedTime < textFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                textColor.a = Mathf.Clamp01(elapsedTime / textFadeDuration);
                gameOverText.color = textColor;
                yield return null;
            }

            textColor.a = 1f;
            gameOverText.color = textColor;

            StartCoroutine(ShowAllButtonsWithFade());
        }
    }

    private IEnumerator ShowAllButtonsWithFade()
    {
        yield return new WaitForSeconds(buttonsActivationDelay);

        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(true);
            }
        }

        float elapsedTime = 0f;

        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;

            foreach (Button button in buttons)
            {
                if (button != null)
                {
                    Image buttonImage = button.GetComponent<Image>();
                    if (buttonImage != null)
                    {
                        Color buttonColor = buttonImage.color;
                        buttonColor.a = Mathf.Clamp01(elapsedTime / textFadeDuration);
                        buttonImage.color = buttonColor;
                    }

                    TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                    {
                        Color textColor = buttonText.color;
                        textColor.a = Mathf.Clamp01(elapsedTime / textFadeDuration);
                        buttonText.color = textColor;
                    }
                }
            }

            yield return null;
        }

        foreach (Button button in buttons)
        {
            if (button != null)
            {
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    Color buttonColor = buttonImage.color;
                    buttonColor.a = 1f;
                    buttonImage.color = buttonColor;
                }

                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    Color textColor = buttonText.color;
                    textColor.a = 1f;
                    buttonText.color = textColor;
                }
            }
        }

        SetCursorVisibility(true);
    }

    private void SetCursorVisibility(bool isVisible)
    {
        if (isVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnClickRetryButton()
    {
        ActivateLoadingImage();
        SceneManager.LoadScene("Maze Scene");
    }

    public void OnClickMainMenuButton()
    {
        ActivateLoadingImage();
        SceneManager.LoadScene("Main Menu");
    }

    public void OnClickExitGameButton()
    {
        Application.Quit();
    }

    private void ActivateLoadingImage()
    {
        if (loadingImage != null)
        {
            if (!loadingImage.activeSelf)
            {
                loadingImage.SetActive(true);
            }
        }
    }
}