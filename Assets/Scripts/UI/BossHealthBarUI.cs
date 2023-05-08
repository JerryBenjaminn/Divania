using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private CharacterStats bossStats;
    [SerializeField] private HealthSystem bossHealthSystem;

    private float maxHealth;

    private void Start()
    {      
        maxHealth = bossStats.maxHealth;
        bossHealthBar.maxValue = maxHealth;
        bossHealthBar.value = maxHealth;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateBossHealthBar();
    }

    private void UpdateBossHealthBar()
    {
        float currentHealth = bossHealthSystem.currentHealth;
        bossHealthBar.value = currentHealth;
    }

    public void SetBossName(string bossName)
    {
        bossNameText.text = bossName;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

