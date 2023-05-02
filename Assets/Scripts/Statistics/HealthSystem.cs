using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;

    private int currentHealth;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;

    private void Start()
    {
        currentHealth = characterStats.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - characterStats.defensePower, 0);
        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        OnTakeDamage.Invoke();

        if (currentHealth == 0)
        {
            OnDeath.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, characterStats.maxHealth);

        OnHeal.Invoke();
    }
}

