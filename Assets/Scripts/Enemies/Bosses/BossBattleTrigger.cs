using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            AudioManager.instance.PlayBossBattleMusic();
            triggered = true;
            gameObject.SetActive(false);
        }
    }
}


