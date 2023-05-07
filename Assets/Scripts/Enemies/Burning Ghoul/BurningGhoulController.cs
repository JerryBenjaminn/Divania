using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurningGhoulController : MonoBehaviour
{
    [SerializeField] private float detectionRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private int explosionDamage;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float jumpWaitTime = 2f;

    [Header("Explosion Animation")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionAnimationDuration = 1.0f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private bool isJumping = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player != null && !isJumping &&Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            MoveTowardsPlayer();
            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                if (!isJumping)
                {
                    StartCoroutine(JumpAndExplode());
                }
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (!isJumping)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
    }
    private IEnumerator JumpAndExplode()
    {
        isJumping = true;

        // Calculate the distance and direction to the player
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Calculate the horizontal and vertical jump forces
        float horizontalForce = directionToPlayer.x * distanceToPlayer * jumpForce;
        float verticalForce = jumpHeight;

        // Apply the jump forces
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalForce, verticalForce);

        // Wait for the ghoul to reach the target position
        yield return new WaitForSeconds(jumpWaitTime);

        // Destroy the ghoul
        Destroy(gameObject);

        // Instantiate the explosion prefab
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Deal damage to all objects within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            PlayerHealthSystem playerHealthSystem = collider.GetComponent<PlayerHealthSystem>();
            if (playerHealthSystem != null)
            {
                playerHealthSystem.TakeDamage(explosionDamage, player.position);
            }
        }

        // Wait for the explosion animation to complete
        yield return new WaitForSeconds(explosionAnimationDuration);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw gizmo for detection range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw gizmo for stopping distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        // Draw gizmo for explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

