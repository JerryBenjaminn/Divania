using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private CharacterStats playerHealth;
    [SerializeField] private HealthSystem healthSystem;
    

    private float maxHealth;

    private void Start()
    {
        maxHealth = playerHealth.maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = 200;
        Debug.Log(maxHealth);
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float currentHealth = healthSystem.currentHealth;
        healthBar.value = currentHealth;
        Debug.Log(currentHealth);
    }
}

