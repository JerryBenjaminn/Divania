using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : CharacterHealthSystem
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override IEnumerator FlashSprite()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.material.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material.color = Color.clear;
            yield return new WaitForSeconds(flashDuration);
        }
        spriteRenderer.material.color = Color.white; // reset color to normal
    }
}
