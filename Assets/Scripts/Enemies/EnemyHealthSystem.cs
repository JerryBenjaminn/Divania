using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : CharacterHealthSystem
{

    public override void TakeDamage(int damage, Vector2 damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        if (!isDying)
        {
            //Calculate the knockback direction
            Vector2 knockbackDirection = (transform.position - (Vector3)damageDealer).normalized;
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration));
        }
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
    private IEnumerator ApplyKnockback(Vector2 direction, float force, float duration)
    {
        // Apply the knockback force
        GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);

        // Wait for the knockback to complete
        yield return new WaitForSeconds(duration);

        // Clear the forces from the Rigidbody
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}
