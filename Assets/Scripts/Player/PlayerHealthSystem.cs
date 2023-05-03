using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : CharacterHealthSystem
{
    //Reference to the Player Controller
    [SerializeField] private PlayerController playerController;

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

    private IEnumerator ApplyKnockback(Vector2 direction, float force, float duration)
    {
        // Temporarily disable player control
        playerController.enabled = false;
        //Debug.Log("Taking damage. Knockback direction: " + direction + ", force: " + force);

        // Apply the knockback force
        GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);

        // Wait for the knockback to complete
        yield return new WaitForSeconds(duration);

        // Clear the forces from the Rigidbody
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // Re-enable player control
        playerController.enabled = true;
    }
}


