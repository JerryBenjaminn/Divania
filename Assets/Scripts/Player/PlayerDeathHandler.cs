using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private HealthSystem playerHealthSystem;
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        playerHealthSystem.OnDeath += HandlePlayerDeath;
    }

    private void OnDestroy()
    {
        playerHealthSystem.OnDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        uiManager.ShowEndPanel();
    }
}

