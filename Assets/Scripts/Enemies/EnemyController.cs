using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int attackPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defencePower;

    public int AttackPower
    {
        get { return attackPower; }
    }

    [Header("Enemy Options")]
    [SerializeField] private float detectionRange; // Distance at which the enemy detects the player
    [SerializeField] protected float moveSpeed; // Speed at which the enemy moves towards the player

    [Header("Edge Detection")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float edgeDetectionDistance = 0.5f;
    [SerializeField] protected Vector2 edgeDetectionOffset;

    [Header("References")]
    [SerializeField] protected Transform player; // Reference to the player's transform

    protected bool isPlayerDetected = false; // Whether the player has been detected


    [SerializeField] private CharacterStats enemyStats;
    [SerializeField] protected EnemyHealthSystem enemyHealth;


    private void Start()
    {
        attackPower = enemyStats.attackPower;
        maxHealth = enemyStats.maxHealth;
        defencePower = enemyStats.defensePower;
    }
    protected virtual void Update()
    {
        // Check if the player is within the detection range
        if (player != null && !isPlayerDetected && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isPlayerDetected = true;
        }

        // If the player has been detected and the enemy can move, move towards them
        if (isPlayerDetected && CanMove() && !enemyHealth.isDying)
        {
            MoveTowardsPlayer();
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Flip sprite to face the player
            if (player.position.x < transform.position.x)
            {
                // Player is to the right, so sprite should face right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Player is to the left, so sprite should face left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            // Check for ground before moving
            Vector2 edgeDetectionPosition = new Vector2(transform.position.x + edgeDetectionOffset.x * Mathf.Sign(transform.localScale.x), transform.position.y + edgeDetectionOffset.y);
            bool isGroundAhead = Physics2D.Raycast(edgeDetectionPosition, Vector2.down, edgeDetectionDistance, groundLayer);

            // If ground is detected, move the enemy in that direction
            if (isGroundAhead)
            {
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }
    }
    protected virtual bool CanMove()
    {
        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided is the player
        PlayerHealthSystem playerHealth = collision.gameObject.GetComponent<PlayerHealthSystem>();
        if (playerHealth != null)
        {
            Debug.Log("Player hit");

            // Get the contact point of the collision
            Vector2 contactPoint = collision.contacts[0].point;

            // Pass the contact point instead of the enemy's transform
            playerHealth.TakeDamage(enemyStats.attackPower, contactPoint);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector2 edgeDetectionPosition = new Vector2(transform.position.x + edgeDetectionOffset.x * Mathf.Sign(transform.localScale.x), transform.position.y + edgeDetectionOffset.y);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(edgeDetectionPosition, edgeDetectionPosition + Vector2.down * edgeDetectionDistance);
    }
}
