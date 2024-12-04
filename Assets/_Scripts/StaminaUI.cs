using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField, Tooltip("Visualización de la barra de stamina")]
    private Image staminaBar;

    [SerializeField]
    private PlayerController playerController;
    
    private CanvasGroup _canvasGroup;

    void Awake()
    {
        
        _canvasGroup = GetComponentInParent<CanvasGroup>();
    }
    
    void Start()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0;
        }
    }
    
    void Update()
    {
        float currentStamina = playerController.GetStamina();
        float maxStamina = playerController.GetMaxStamina();

        staminaBar.fillAmount = currentStamina / maxStamina;

        if (Input.GetKey(KeyCode.LeftShift) && playerController.IsSprintAllowed())
        {
            ShowBar(true); 
        }
        else if (currentStamina < maxStamina && playerController.IsSprintAllowed())
        {
            ShowBar(true); 
        }
        else
        {
            ShowBar(false); 
        }
    }

    private void ShowBar(bool isVisible)
    {
        if (_canvasGroup != null)
        {
            if (isVisible)
            {
                _canvasGroup.alpha = 1;
            }
            else
            {
                _canvasGroup.alpha = 0;
            }
        }
    }
}
