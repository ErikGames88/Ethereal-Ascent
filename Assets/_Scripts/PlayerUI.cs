using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the stamina UI bar when the player is sprinting or when the stamina is refilling. 
/// Hides the bar when fully recharged and not sprinting.
/// </summary>
[RequireComponent(typeof(PlayerStamina))]
public class PlayerUI : MonoBehaviour
{
    private float maxFillAmount;

    [Header("Life")]
    private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _healthBlood;
    [SerializeField] private Image _bloodLayer1;
    [SerializeField] private Image _bloodLayer2;
    [SerializeField] private Image _bloodBackground;
    private List<HealthData> healthStages = new List<HealthData>();
    private bool healthStageFound;


    [Header("Stamina")]
    private PlayerStamina _playerStamina;
    [SerializeField] private GameObject _staminaBar;
    [SerializeField] private Image _greenBar;

    public struct HealthData
    {
        public int threshold;
        public float alpha1;
        public float alpha2;
        public float alphaBackground;

        public HealthData(int threshold, float alpha1, float alpha2, float alphaBackground) // CONSTRUCTOR  
        {
            this.threshold = threshold;
            this.alpha1 = alpha1;
            this.alpha2 = alpha2;
            this.alphaBackground = alphaBackground;
        }
    }


    void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerStamina = GetComponent<PlayerStamina>();

        maxFillAmount = 1f;
    }

    void Start()
    {
        _greenBar.fillAmount = maxFillAmount;

        healthStages.Add(new HealthData(90, 20, 0, 0));
        healthStages.Add(new HealthData(70, 50, 0, 0));
        healthStages.Add(new HealthData(50, 70, 0, 10));
        healthStages.Add(new HealthData(35, 100, 20, 10));
        healthStages.Add(new HealthData(20, 120, 30, 20));
        healthStages.Add(new HealthData(10, 150, 70, 50));
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateStaminaUI();
    }

    /// <summary>
    /// Updates the UI visuals for player's health.
    /// Changes blood layers and background alpha depending on current health level.
    /// Activates the health blood UI when health is below thresholds.
    /// </summary>
    private void UpdateHealthUI()
    {
        healthStageFound = false;

        Color layerAlpha1 = _bloodLayer1.color;
        Color layerAlpha2 = _bloodLayer2.color;
        Color backgroundAlpha = _bloodBackground.color;

        for (int i = 0; i < healthStages.Count; i++)
        {
            if (_playerHealth.CurrentHealth < healthStages[i].threshold)
            {
                _healthBlood.SetActive(true);
                healthStageFound = true;

                layerAlpha1.a = healthStages[i].alpha1 / 255f;
                layerAlpha2.a = healthStages[i].alpha2 / 255f;
                backgroundAlpha.a = healthStages[i].alphaBackground / 255f;

                _bloodLayer1.color = layerAlpha1;
                _bloodLayer2.color = layerAlpha2;
                _bloodBackground.color = backgroundAlpha;
            }
        }

        if (!healthStageFound)
        {
            _healthBlood.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the stamina bar UI.
    /// Shows the bar when sprinting or recovering stamina, hides it when full and not sprinting.
    /// </summary>
    private void UpdateStaminaUI()
    {
        if (_playerStamina.SprintActive || _greenBar.fillAmount < maxFillAmount)
        {
            _staminaBar.SetActive(true);
        }
        else
        {
            _staminaBar.SetActive(false);
        }

        _greenBar.fillAmount = _playerStamina.GetStaminaNormalized();
    }
}
