using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkullCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private GameObject cathedralKeyText;
    [SerializeField] private GameObject cathedralKeyHintText; 
    [SerializeField] private GameObject textBackground;
    [SerializeField] private PlayerLocked playerLocked;
    [SerializeField]private HintTextManager hintTextManager; 
    [SerializeField] private TimerManager timerManager; 
    [SerializeField] private GameObject cathedralKey;
    [SerializeField] private GameObject bossDoor;
    private int skullCount = 0;
    private bool isKeyHintActive = false;


    void Start()
    {
        counterText.text = "0";
        cathedralKeyText.SetActive(false);
        textBackground.SetActive(false);

        if (cathedralKeyHintText != null)
        {
            cathedralKeyHintText.SetActive(false); 
        }

        if (cathedralKey != null)
        {
            cathedralKey.SetActive(false);
        }

        if (bossDoor != null)
        {
            bossDoor.SetActive(true); 
        }

        if (playerLocked == null)
        {
            playerLocked = FindObjectOfType<PlayerLocked>();
        }
    }

    void Update()
    {
        if (isKeyHintActive && Input.GetKeyDown(KeyCode.E))
        {
            HideKeyHint();
        }
    }

    public void AddSkull()
    {
        skullCount++;

        if (skullCount == 6)
        {
            ShowKeyHint();
            ActivateKey();
            DeactivateBossDoor(); 
        }

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        counterText.text = skullCount.ToString();
    }

    private void ShowKeyHint()
    {
        cathedralKeyText.SetActive(true);
        textBackground.SetActive(true);

        if (timerManager != null)
        {
            timerManager.StopTimer();
        }

        isKeyHintActive = true;

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(true, false);
        }
    }

    private void HideKeyHint()
    {
        cathedralKeyText.SetActive(false);
        textBackground.SetActive(false);
        isKeyHintActive = false;

        if (playerLocked != null)
        {
            playerLocked.LockPlayer(false);
        }

        if (hintTextManager != null)
        {
            hintTextManager.ShowCathedralHintText();
        }
    }

    private void ActivateKey()
    {
        if (cathedralKey != null)
        {
            cathedralKey.SetActive(true);
        }
    }

    private void DeactivateBossDoor()
    {
        if (bossDoor != null)
        {
            bossDoor.SetActive(false);
        }
    }
}