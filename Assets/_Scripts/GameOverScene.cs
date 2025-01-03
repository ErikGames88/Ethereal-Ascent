using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al Red Background")]
    private GameObject redBackground;

    [SerializeField, Tooltip("Referencia al Game Over Panel")]
    private GameObject gameOverPanel;

    [SerializeField, Tooltip("Duración del fade-in del Game Over Panel")]
    private float panelFadeDuration = 3f;

    [SerializeField, Tooltip("Duración del fade-in del Game Over Text")]
    private float textFadeDuration = 2f;

    [SerializeField, Tooltip("Tiempo antes de que el Game Over Panel comience a mostrarse")]
    private float panelActivationDelay = 1f;

    [SerializeField, Tooltip("Tiempo antes de que el Game Over Text comience a mostrarse")]
    private float textActivationDelay = 2f;

    [SerializeField, Tooltip("Tiempo antes de que los botones comiencen a mostrarse")]
    private float buttonsActivationDelay = 2f;

    private Image panelImage; // Componente Image del Game Over Panel
    private TextMeshProUGUI gameOverText; // Componente TextMeshPro del Game Over Text
    private List<Button> buttons = new List<Button>(); // Lista de botones

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
            if (panelImage == null)
            {
                Debug.LogError("El Game Over Panel no tiene un componente Image.");
            }

            Transform textTransform = gameOverPanel.transform.Find("Game Over Text");
            if (textTransform != null)
            {
                gameOverText = textTransform.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("Game Over Text no encontrado como hijo del Game Over Panel.");
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
                Debug.LogError($"Faltan botones en el Game Over Panel. Encontrados: {buttons.Count}. Esperados: 3.");
            }
        }
    }

    private void Start()
    {
        SetCursorVisibility(false);

        if (redBackground != null)
        {
            redBackground.SetActive(true);
        }
        else
        {
            Debug.LogError("Red Background no asignado en el Inspector.");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            DisablePanelChildren();
        }
        else
        {
            Debug.LogError("Game Over Panel no asignado en el Inspector.");
        }

        Debug.Log("Game Over Scene: Red Background activado, Game Over Panel desactivado.");
        StartCoroutine(ShowGameOverPanelWithFade());
        ConfigureButtons();
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

            Debug.Log("Game Over Panel activado. Iniciando fade-in.");

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

            Debug.Log("Fade-in del Game Over Panel completado.");
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

            Debug.Log("Game Over Text activado. Iniciando fade-in.");

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

            Debug.Log("Fade-in del Game Over Text completado.");
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
            else
            {
                Debug.LogError("Un botón no encontrado como hijo del Game Over Panel.");
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

        Debug.Log("Fade-in completo para todos los botones y sus textos.");

        // Mostrar el cursor tras completar el fade-in de los botones
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

        Debug.Log($"Cursor {(isVisible ? "mostrado" : "oculto")}.");
    }

    private void ConfigureButtons()
    {
        foreach (Button button in buttons)
        {
            if (button.name == "Retry Button")
            {
                button.onClick.AddListener(RetryGame);
                Debug.Log("Retry Button configurado para cargar la Maze Scene.");
            }
            else if (button.name == "Main Menu Button")
            {
                button.onClick.AddListener(GoToMainMenu);
                Debug.Log("Main Menu Button configurado para cargar el Main Menu.");
            }
            else if (button.name == "Exit Game Button")
            {
                button.onClick.AddListener(ExitGame);
                Debug.Log("Exit Game Button configurado para salir del juego.");
            }
        }
    }

    private void RetryGame()
    {
        SceneManager.LoadScene("Maze Scene");
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void ExitGame()
    {
        Debug.Log("SALISTE DEL JUEGO");
        Application.Quit();
    }
}