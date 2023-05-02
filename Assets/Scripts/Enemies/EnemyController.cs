using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float detectionRange; // Distance at which the enemy detects the player
    [SerializeField] private float moveSpeed; // Speed at which the enemy moves towards the player

    [Header("References")]
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private Animator animator; // Reference to the animator component

    private bool isPlayerDetected = false; // Whether the player has been detected
    private bool hasRisen = false; // Whether the rise animation has completed

    private void Update()
    {
        // Check if the player is within the detection range
        if (!isPlayerDetected && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isPlayerDetected = true;

            // Trigger the rise animation
            animator.SetTrigger("Rise");
        }

        // If the player has been detected and the rise animation has completed, move towards them
        if (isPlayerDetected && hasRisen)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the enemy in that direction
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // This function is called from the animator at the end of the rise animation
    public void RiseComplete()
    {
        hasRisen = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
