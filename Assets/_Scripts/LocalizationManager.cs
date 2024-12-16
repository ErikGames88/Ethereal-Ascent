using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance; // Singleton para acceso global

    public enum Language { Spanish, English }
    public Language currentLanguage = Language.Spanish;

    private Dictionary<string, string> spanishTexts = new Dictionary<string, string>();
    private Dictionary<string, string> englishTexts = new Dictionary<string, string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionaries()
    {
        // Textos en español
        spanishTexts.Add("Flashlight", "Linterna");
        spanishTexts.Add("Skulls", "Cráneos");

        // Textos en inglés
        englishTexts.Add("Flashlight", "Flashlight");
        englishTexts.Add("Skulls", "Skulls");
    }

    public string GetLocalizedText(string key)
    {
        switch (currentLanguage)
        {
            case Language.Spanish:
                if (spanishTexts.ContainsKey(key))
                {
                    return spanishTexts[key];
                }
                else
                {
                    return key;
                }

            case Language.English:
                if (englishTexts.ContainsKey(key))
                {
                    return englishTexts[key];
                }
                else
                {
                    return key;
                }

            default:
                return key; // Retorna la clave si no encuentra el texto
        }
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;
    }
}
