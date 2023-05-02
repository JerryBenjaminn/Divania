using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    //Reference to the character stats
    [SerializeField] private CharacterStats characterStats;

    //Variable to save the current health value
    private int currentHealth;

    //Unity-events for taking damage, healing and death
    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;

    private void Start()
    {
        //Set the current health to be the max health
        currentHealth = characterStats.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        //Calculates the damage by subsracting the defense power
        int actualDamage = Mathf.Max(damage - characterStats.defensePower, 0);
        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        //Call the method for taking the damage
        OnTakeDamage.Invoke();

        //If the health reaches zero, call the method to handle death
        if (currentHealth == 0)
        {
            OnDeath.Invoke();
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
}

