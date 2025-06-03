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
    [SerializeField] private Image _greenBar;
    [SerializeField] private GameObject _staminaBar;
    private PlayerStamina _playerStamina;


    void Awake()
    {
        _playerStamina = GetComponent<PlayerStamina>();
        maxFillAmount = 1f;
    }

    void Start()
    {
        _greenBar.fillAmount = maxFillAmount;
    }


    void Update()
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
