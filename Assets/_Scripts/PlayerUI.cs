using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image _greenBar;
    [SerializeField] private GameObject _staminaBar;
    private PlayerStamina _playerStamina;
    private PlayerController _playerController;


    void Awake()
    {
        _playerStamina = GetComponent<PlayerStamina>();
        _playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        _greenBar.fillAmount = 1;
    }


    void Update()
    {
        if ((_playerStamina.SprintActive || _greenBar.fillAmount < 1))
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
