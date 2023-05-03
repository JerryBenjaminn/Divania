using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int attackPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defencePower;

    [Header("Enemy Options")]
    [SerializeField] private float detectionRange; // Distance at which the enemy detects the player
    [SerializeField] private float moveSpeed; // Speed at which the enemy moves towards the player

    [Header("References")]
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private Animator animator; // Reference to the animator component

    private bool isPlayerDetected = false; // Whether the player has been detected
    private bool hasRisen = false; // Whether the rise animation has completed

    [SerializeField] private CharacterStats enemyStats;
    [SerializeField] private EnemyHealthSystem enemyHealth;


    private void Start()
    {
        attackPower = enemyStats.attackPower;
        maxHealth = enemyStats.maxHealth;
        defencePower = enemyStats.defensePower;
    }
    private void Update()
    {
        // Check if the player is within the detection range
        if (player != null && !isPlayerDetected && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isPlayerDetected = true;

            // Trigger the rise animation
            animator.SetTrigger("Rise");
        }

        // If the player has been detected and the rise animation has completed, move towards them
        if (isPlayerDetected && hasRisen && !enemyHealth.isDying)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if(player != null)
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

            // Move the enemy in that direction
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

    }

    // This function is called from the animator at the end of the rise animation
    public void RiseComplete()
    {
        hasRisen = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if the object we collided is the player
        CharacterHealthSystem playerHealth = collision.gameObject.GetComponent<CharacterHealthSystem>();
        if(playerHealth != null)
        {
            Debug.Log("Player hit");
            playerHealth.TakeDamage(enemyStats.attackPower, player);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
