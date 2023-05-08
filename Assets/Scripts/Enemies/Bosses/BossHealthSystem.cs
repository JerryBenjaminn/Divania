using UnityEngine;

public class BossHealthSystem : EnemyHealthSystem
{
    private BossController bossController;

    private void Start()
    {
        bossController = GetComponent<BossController>();
    }

    public override void TakeDamage(int damage, Vector2 damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        if (bossController != null)
        {
            bossController.TakeDamage(damage);
            AudioManager.instance.PlayAudioClip("BossHurt");
        }
    }
}
