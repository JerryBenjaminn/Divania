using UnityEngine;
using UnityEngine.Events;
using System;

public class HealthSystem : MonoBehaviour
{
    //Reference to the character stats
    [SerializeField] private CharacterStats characterStats;

    //Variable to save the current health value
    [SerializeField] public int currentHealth;
    [SerializeField] private int maxHealth;

    //Unity-events for taking damage, healing and death
    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public event Action OnDeath;

    private void Start()
    {
        // Set the max health from the characterStats
        maxHealth = characterStats.maxHealth;
        // Set the current health to be the max health
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        //Calculates the damage by subtracting the defense power
        int actualDamage = Mathf.Max(damage - characterStats.defensePower, 0);
        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log(currentHealth);
        //Call the method for taking the damage
        OnTakeDamage.Invoke();
        if (CompareTag("Enemy"))
        {
            AudioManager.instance.PlayAudioClip("EnemyHurt");
            AudioManager.instance.PlayAudioClip("PlayerMeleeAttack");
        }
        else
        {
            AudioManager.instance.PlayAudioClip("PlayerHurt");
        }
        

        //If the health reaches zero, call the method to handle death
        if (currentHealth == 0)
        {
            // Check if the OnDeath event is not null before invoking it
            OnDeath?.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        //Add health and check that it doesn't go over the max health
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, characterStats.maxHealth);

        //Call the method for healing
        OnHeal.Invoke();
    }

    public bool IsDead()
    {
        //Checks if the health is zero or less
        return currentHealth <= 0;
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

