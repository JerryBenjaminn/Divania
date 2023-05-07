using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Ghost Options")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackAnimationDuration;

    //Because wizard is really similiar with Ghost, we use this script and add a wizard spesific action
    [Header("Wizard Options")]
    [SerializeField] private bool isWizard;
    [SerializeField] private float combustAttackRange;
    [SerializeField] private float combustCooldown;
    [SerializeField] private int combustDamage;
    [SerializeField] private float combustDamageInterval = 0.5f;
    [SerializeField] private float deathDuration = 0.2f;


    private bool isCombustActive = false;
    private float lastCombustTime;

    private SpriteRenderer ghostSprite;
    private Animator animator;
    private HealthSystem healthSystem;

    private bool isAttacking = false;

    private float lastAttackTime;

    private void Start()
    {
        attackPower = enemyStats.attackPower;
        maxHealth = enemyStats.maxHealth;
        defencePower = enemyStats.defensePower;

        ghostSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player != null && !enemyHealth.isDying && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            FollowPlayer();
            AttackPlayer();
        }
        if (isWizard)
        {
            CombustAttack();
        }

        // Check if the enemy is dead
        if (enemyHealth.healthSystem.IsDead() && !enemyHealth.isDying)
        {
            StartCoroutine(Die());
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
        if (Time.time > lastAttackTime + attackCooldown && !isCombustActive)
        {
            if (!isAttacking)
            {
                StartCoroutine(PerformAttack());
            }
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Start attack animation
        StartAttackAnimation();

        // Wait for the animation to finish (adjust the duration based on your animation length)
        yield return new WaitForSeconds(attackAnimationDuration);

        if(player != null)
        {
            // Shoot the projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        }
        
        // Stop attack animation
        StopAttackAnimation();

        lastAttackTime = Time.time;
        isAttacking = false;
    }

    private IEnumerator Die()
    {
        enemyHealth.isDying = true;

        // Play the die animation
        animator.SetTrigger("Die");

        // Wait for the length of the animation
        yield return new WaitForSeconds(deathDuration);

        // Destroy the game object
        Destroy(gameObject);
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

    private void CombustAttack()
    {
        if (player != null && Time.time > lastCombustTime + combustCooldown && Vector2.Distance(transform.position, player.position) <= combustAttackRange)
        {
            lastCombustTime = Time.time;

            //Set isCombustActive to true
            isCombustActive = true;

            StartCoroutine(DealCombustDamage());
        }
    }

    private IEnumerator DealCombustDamage()
    {
        //Trigger animation for the combust attack
        animator.SetTrigger("Combust");

        float delayBeforeDamage = 1.1f;
        yield return new WaitForSeconds(delayBeforeDamage);

        if (player != null)
        {
            float startTime = Time.time;

            while (player != null && Time.time < startTime + attackAnimationDuration)
            {
                PlayerHealthSystem playerHealth = player.GetComponent<PlayerHealthSystem>();
                if (playerHealth != null && Vector2.Distance(transform.position, player.position) <= combustAttackRange)
                {
                    playerHealth.TakeDamage(combustDamage, player.position);
                }

                yield return new WaitForSeconds(combustDamageInterval);
            }

            isCombustActive = false;
        }
    }


    public void CombustAttackFinished()
    {
        // Set isCombustActive to false
        isCombustActive = false;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, combustAttackRange);
    }

    private void StartAttackAnimation()
    {
        animator.SetBool("IsAttacking", true);
    }
    private void StopAttackAnimation()
    {
        animator.SetBool("IsAttacking", false);
    }
}
