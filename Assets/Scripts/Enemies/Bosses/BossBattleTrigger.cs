using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    [SerializeField] private BossHealthBarUI bossHealthBarUI;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            AudioManager.instance.PlayBossBattleMusic();
            bossHealthBarUI.Show();
            triggered = true;
            gameObject.SetActive(false);
        }
    }
}


