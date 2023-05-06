using UnityEngine;

public class HellgatorController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int attackPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defencePower;

    [Header("References")]
    [SerializeField] protected EnemyHealthSystem enemyHealth;
    [SerializeField] private CharacterStats enemyStats;

    [SerializeField] private float movementSpeed;

    [Header("Spawn and Destroy")]
    [SerializeField] private float timeToDestroy = 5f;

    private SpriteRenderer hellgatorSprite;

    private void Start()
    {
        attackPower = enemyStats.attackPower;
        maxHealth = enemyStats.maxHealth;
        defencePower = enemyStats.defensePower;

        hellgatorSprite = GetComponent<SpriteRenderer>();

        Invoke("DestroyHellgator", timeToDestroy);
    }
    private void Update()
    {
        transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);
        if(movementSpeed <= 0)
        {
            hellgatorSprite.flipX = true;
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

    private void DestroyHellgator()
    {
        Destroy(gameObject);
    }
}
