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

        public HealthData(int threshold, float alpha1, float alpha2, float alphaBackground)
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
    }


    void Update()
    {
        Color backgroundAlpha = _bloodBackground.color;
        Color layerAlpha1 = _bloodLayer1.color;
        Color layerAlpha2 = _bloodLayer2.color;

        if (_playerHealth.CurrentHealth < 90)
        {
            _healthBlood.SetActive(true);

            layerAlpha1.a = (float)20 / 255;
            _bloodLayer1.color = layerAlpha1;

            if (_playerHealth.CurrentHealth < 70)
            {
                layerAlpha1.a = (float)50 / 255;
                _bloodLayer1.color = layerAlpha1;
            }
            else if (_playerHealth.CurrentHealth < 50)
            {
                layerAlpha1.a = (float)70 / 255;
                _bloodLayer1.color = layerAlpha1;

                backgroundAlpha.a = (float)10 / 255;
                _bloodBackground.color = backgroundAlpha;
            }
            else if (_playerHealth.CurrentHealth < 35)
            {
                layerAlpha1.a = (float)100 / 255;
                _bloodLayer1.color = layerAlpha1;

                layerAlpha2.a = (float)20 / 255;
                _bloodLayer2.color = layerAlpha2;

                backgroundAlpha.a = (float)10 / 255;
                _bloodBackground.color = backgroundAlpha;
            }
            else if (_playerHealth.CurrentHealth < 20)
            {
                layerAlpha1.a = (float)120 / 255;
                _bloodLayer1.color = layerAlpha1;

                layerAlpha2.a = (float)30 / 255;
                _bloodLayer2.color = layerAlpha2;

                backgroundAlpha.a = (float)10 / 255;
                _bloodBackground.color = backgroundAlpha;
            }
            else if (_playerHealth.CurrentHealth < 10)
            {
                layerAlpha1.a = (float)150 / 255;
                _bloodLayer1.color = layerAlpha1;

                layerAlpha2.a = (float)70 / 255;
                _bloodLayer2.color = layerAlpha2;

                backgroundAlpha.a = (float)50 / 255;
                _bloodBackground.color = backgroundAlpha;
            }
        }
        else
        {
            _healthBlood.SetActive(false);
        }
        // if (_playerHealth.CurrentHealth < 90)
        // {
        //     _healthBlood.SetActive(true);

        //     layerAlpha1.a = (float)20 / 255;
        //     _bloodLayer1.color = layerAlpha1;
        // }
        // else if (_playerHealth.CurrentHealth < 70)
        // {
        //     layerAlpha1.a = (float)50 / 255;
        //     _bloodLayer1.color = layerAlpha1;
        // }
        // else if (_playerHealth.CurrentHealth < 50)
        // {
        //     layerAlpha1.a = (float)70 / 255;
        //     _bloodLayer1.color = layerAlpha1;

        //     backgroundAlpha.a = (float)10 / 255;
        //     _bloodBackground.color = backgroundAlpha;
        // }
        // else if (_playerHealth.CurrentHealth < 35)
        // {
        //     layerAlpha1.a = (float)100 / 255;
        //     _bloodLayer1.color = layerAlpha1;

        //     layerAlpha2.a = (float)20 / 255;
        //     _bloodLayer2.color = layerAlpha2;

        //     backgroundAlpha.a = (float)10 / 255;
        //     _bloodBackground.color = backgroundAlpha;
        // }
        // else if (_playerHealth.CurrentHealth < 20)
        // {
        //     layerAlpha1.a = (float)120 / 255;
        //     _bloodLayer1.color = layerAlpha1;

        //     layerAlpha2.a = (float)30 / 255;
        //     _bloodLayer2.color = layerAlpha2;

        //     backgroundAlpha.a = (float)10 / 255;
        //     _bloodBackground.color = backgroundAlpha;
        // }
        // else if (_playerHealth.CurrentHealth < 10)
        // {
        //     layerAlpha1.a = (float)150 / 255;
        //     _bloodLayer1.color = layerAlpha1;

        //     layerAlpha2.a = (float)70 / 255;
        //     _bloodLayer2.color = layerAlpha2;

        //     backgroundAlpha.a = (float)50 / 255;
        //     _bloodBackground.color = backgroundAlpha;
        // }

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
