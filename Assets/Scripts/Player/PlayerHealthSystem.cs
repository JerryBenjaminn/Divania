using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : CharacterHealthSystem
{
    //Reference to the Player Controller
    [SerializeField] private PlayerController playerController;

    [Header("Invulnerability settings")]
    [SerializeField] private float invulnerabilityDuration = 1f;
    private bool isInvulnerable = false;

    public override void TakeDamage(int damage, Vector2 damageDealer)
    {
        if (!isInvulnerable)
        {
            base.TakeDamage(damage, damageDealer);

            if (!isDying)
            {
                //Calculate the knockback direction
                Vector2 knockbackDirection = (transform.position - (Vector3)damageDealer).normalized;
                StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration, knockbackAngle));
            }

            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction, float force, float duration, float knockbackAngle = 45f)
    {
        // Temporarily disable player control
        playerController.enabled = false;

        // Convert knockback angle to radians
        float angleInRadians = knockbackAngle * Mathf.Deg2Rad;

        // Create a new vector with horizontal and vertical components
        Vector2 knockbackDirection = new Vector2(direction.x * Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;

        // Apply the knockback force
        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * force, ForceMode2D.Impulse);

        // Wait for the knockback to complete
        yield return new WaitForSeconds(duration);

        // Clear the forces from the Rigidbody
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // Re-enable player control
        playerController.enabled = true;
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
}
