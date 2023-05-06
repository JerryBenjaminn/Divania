using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Stats")]
    [SerializeField] private int attackPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defencePower;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;

    [Header("Movement")]
    [SerializeField] private float followDistance;
    [SerializeField] private float moveSpeed;

    [Header("References")]
    [SerializeField] protected EnemyHealthSystem enemyHealth;
    [SerializeField] private CharacterStats enemyStats;
    [SerializeField] private Transform player;

    private SpriteRenderer ghostSprite;

    private float lastAttackTime;

    private void Start()
    {
        attackPower = enemyStats.attackPower;
        maxHealth = enemyStats.maxHealth;
        defencePower = enemyStats.defensePower;

        ghostSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player != null && !enemyHealth.isDying)
        {
            FollowPlayer();
            AttackPlayer();
        }

    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        if (player.position.x < transform.position.x)
        {
            // Player is to the left, so sprite should face left
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Player is to the right, so sprite should face right
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void AttackPlayer()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        }
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
}
