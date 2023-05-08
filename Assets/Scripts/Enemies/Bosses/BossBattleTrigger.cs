using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    private BossHealthBarUI bossHealthBarUI;

    private bool triggered = false;

    private void Start()
    {
        bossHealthBarUI = GetComponent<BossHealthBarUI>();
        bossHealthBarUI.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            AudioManager.instance.PlayBossBattleMusic();
            triggered = true;
            gameObject.SetActive(false);
            bossHealthBarUI.enabled = true;
        }
    }
}


