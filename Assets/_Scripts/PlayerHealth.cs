using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Puntos de vida máximos del Player")]
    private float maxHealth = 100f;

    [SerializeField, Tooltip("Imagen de sangre que irá cubriendo la pantalla")]
    private Image bloodOverlay;

    [SerializeField, Tooltip("Filtro rojo para indicar vida crítica")]
    private Image redFilter;

    [SerializeField] 
    private float currentHealth;
 
    void Start()
    {
        currentHealth = maxHealth;
        UpdateBloodOverlay();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 
        UpdateBloodOverlay();

        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    private void UpdateBloodOverlay()
    {
        if (bloodOverlay != null)
        {
            float bloodOpacity = 1 - (currentHealth / maxHealth);
            Color bloodColor = bloodOverlay.color;
            bloodColor.a = bloodOpacity;
            bloodOverlay.color = bloodColor;
        }

        if (redFilter != null)
        {
            float redIntensity = Mathf.Clamp01(0.4f - (currentHealth / 30f));
            Color filterColor = redFilter.color;
            filterColor.a = redIntensity; 
            redFilter.color = filterColor;
        }
    }

    private void PlayerDeath()
    {
        // Notificar al GameManager el Game Over en el futuro:
        // Debug.Log("Player is dead");
    }

    public void Healing(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 
        UpdateBloodOverlay();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // TODO: Eliminar este método al final, SÓLO PARA PRUEBAS
    private void OnValidate()
    {
        if (Application.isPlaying && bloodOverlay != null)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 
            maxHealth = Mathf.Clamp(maxHealth, 0, 100); 
            UpdateBloodOverlay();
        }
    }
}
