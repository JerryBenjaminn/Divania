using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private int currentPhase = 0;
    [SerializeField] private float timeBetweenAttacks = 3.0f;
    private float timeSinceLastAttack = 0.0f;
    private bool isAttacking = false;
    private bool isCasting = false;

    private Transform playerTransform;
    private Rigidbody2D bossRigidbody;

    [SerializeField] private int phase2HealthThreshold = 75;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attack1Range = 2.0f;
    [SerializeField] private float attack2Range = 3f;

    [SerializeField] private GameObject magicAttackPrefab;
    [SerializeField] private float magicAttackRange = 10f;

    [Header("Boss Attack Options")]
    [SerializeField] private int phase0Attack1Damage = 10;
    [SerializeField] private int phase0Attack2Damage = 10;
    [SerializeField] private int magicAttackBurstCount = 3;
    [SerializeField] private float timeBetweenMagicAttacks = 0.5f;
    [SerializeField] private float detectionRange = 5f;

    [SerializeField] private Transform attackPoint;

    private PlayerHealthSystem playerHealthSystem;
    private CharacterHealthSystem bossHealthSystem;
    private HealthSystem healthSystem;

    private Animator animator;
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int AttackType = Animator.StringToHash("AttackType");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsCasting = Animator.StringToHash("IsCasting");


    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealthSystem = playerTransform.GetComponent<PlayerHealthSystem>();
        bossRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bossHealthSystem = GetComponent<BossHealthSystem>();
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer <= detectionRange)
        {
            if (playerTransform != null)
            {
                if (playerTransform.position.x + 3 <= transform.position.x && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (playerTransform.position.x - 3 >= transform.position.x && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }

                if (!isAttacking && !isCasting && Time.time >= timeSinceLastAttack + timeBetweenAttacks)
                {
                    StartCoroutine(PerformAttack());
                }
                if (!isCasting && !isAttacking)
                {
                    switch (currentPhase)
                    {
                        case 0:
                            // Move logic for phase 0
                            MoveBossPhase0();
                            break;
                        case 1:
                            // Move logic for phase 1
                            MoveBossPhase1();
                            break;
                        case 2:
                            // Move logic for phase 2
                            MoveBossPhase2();
                            break;
                    }
                }
            }

            CheckBossDeath();
        }


    }

    private void CheckBossDeath()
    {
        if (healthSystem.GetCurrentHealth() == 0)
        {
            Debug.Log("Triggering death animation");
            StartCoroutine(BossDeathSequence());
        }
    }
    private IEnumerator BossDeathSequence()
    {
        // Trigger the death animation
        animator.SetTrigger("Die");

        // Wait for the animation to finish before disabling the boss's components
        // Replace AnimationLength with the actual length of the death animation
        float animationLength = 2f;
        yield return new WaitForSeconds(animationLength);

        // Disable the boss's collider and AI scripts to prevent further interactions
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Optional: Destroy the boss's GameObject after a short delay
         yield return new WaitForSeconds(2f);
         Destroy(gameObject);
    }

    private IEnumerator PerformAttack()
    {
        float originalMoveSpeed = moveSpeed;
        moveSpeed = 0;
        isAttacking = true;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        // Choose an attack based on the current phase

        switch (currentPhase)
        {
            case 0:
                // Execute attack 1 or attack 2 for phase 0
                
                if (distanceToPlayer <= attack1Range)
                {
                    StartCoroutine(Phase0Attack1());
                }
                else if (distanceToPlayer <= magicAttackRange)
                {
                    StartCoroutine(Phase0Attack2());
                }
                break;
            case 1:

                if(currentPhase == 1)
                {
                    // Execute attack 1 or attack 2 for phase 1
                    if (distanceToPlayer <= attack2Range)
                    {
                        StartCoroutine(Phase1Attack1());
                    }                      
                    else if (distanceToPlayer <= magicAttackRange)
                    {
                        StartCoroutine(Phase1Attack2());
                    }
                        
                }

                break;
            case 2:
                if (currentPhase == 2)
                {
                    // Check the distance between the boss and the player
                    if (distanceToPlayer > attack2Range) // If the player is far away, use Phase2Attack1
                    {
                        StartCoroutine(Phase2Attack1());
                    }
                    else // If the player is close, use Phase2Attack2
                    {
                        StartCoroutine(Phase2Attack2());
                    }
                }
                break;
        }

        yield return new WaitForSeconds(1.0f); // Wait for attack animation to finish

        // Set IsAttacking to false after the attack animation has finished
        animator.SetBool(IsAttacking, false);

        timeSinceLastAttack = Time.time;
        moveSpeed = originalMoveSpeed;
        isAttacking = false;
    }

    private void MoveBoss(Vector2 direction)
    {
        if (isAttacking || isCasting)
        {
            bossRigidbody.velocity = Vector2.zero;
            animator.SetBool(IsWalking, false);
            return;
        }

        bossRigidbody.velocity = direction * moveSpeed;

        // Update the Animator parameter
        animator.SetBool(IsWalking, direction != Vector2.zero);
    }


    private void MoveBossPhase0()
    {
        // Implement the movement logic for phase 0 here
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        MoveBoss(directionToPlayer);
    }

    private void MoveBossPhase1()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        MoveBoss(directionToPlayer);
    }

    private void MoveBossPhase2()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        MoveBoss(directionToPlayer);
    }


    private IEnumerator Phase0Attack1()
    {
        // Execute the melee attack animation here
        animator.SetBool(IsAttacking, true);
        animator.SetInteger(AttackType, 1);

        // Add a delay before dealing damage
        float damageDelay = 1; // Adjust this value based on your animation
        yield return new WaitForSeconds(damageDelay);

        // Execute attack 1 for phase 0
        float distanceToPlayer = Vector2.Distance(attackPoint.position, playerTransform.position);

        if (distanceToPlayer <= attack1Range)
        {
            // Deal damage to the player
            playerHealthSystem.TakeDamage(phase0Attack1Damage, attackPoint.position);
        }
    }

    private IEnumerator Phase0Attack2()
    {
        // Set IsCasting to true to start the Cast animation
        animator.SetBool(IsCasting, true);
        isCasting = true;

        // Wait for the Cast animation to finish
        yield return new WaitForSeconds(1.0f); // Adjust the waiting time according to your Cast animation length

        // Spawn the magic attack
        StartCoroutine(SpawnMagicAttackAfterDelay(playerTransform.position, 0.5f));

        // Set IsCasting back to false
        animator.SetBool(IsCasting, false);
        isCasting = false;
    }

    private IEnumerator Phase1Attack1()
    {
        // Execute attack 1 for phase 0
        float distanceToPlayer = Vector2.Distance(attackPoint.position, playerTransform.position);

        if (distanceToPlayer <= attack2Range)
        {
            // Execute the melee attack animation here
            animator.SetBool(IsAttacking, true);
            animator.SetInteger(AttackType, 2);

            // Add a delay before dealing damage
            float damageDelay = 1; // Adjust this value based on your animation
            yield return new WaitForSeconds(damageDelay);

            // Deal damage to the player
            playerHealthSystem.TakeDamage(phase0Attack2Damage, attackPoint.position);
        }
    }

    private IEnumerator Phase1Attack2()
    {
        for (int i = 0; i < magicAttackBurstCount; i++)
        {
            animator.SetBool(IsCasting, true);
            isCasting = true;
            yield return new WaitForSeconds(1.0f); // Ota huomioon Cast-animaation kesto
            StartCoroutine(SpawnMagicAttackAfterDelay(playerTransform.position, 0.5f));
            animator.SetBool(IsCasting, false);
            isCasting = false;
            yield return new WaitForSeconds(timeBetweenMagicAttacks);
        }
    }


    private IEnumerator Phase2Attack1()
    {
        Debug.Log("Starting Phase2Attack1");
        animator.SetBool("IsWalking", false);

        // Add the teleport out animation
        animator.SetBool("IsTeleporting", true);
        yield return new WaitForSeconds(1f); // Adjust the time to match the duration of your teleport out animation
        animator.SetBool("IsTeleporting", false);

        yield return new WaitForSeconds(0.5f); // Add a short delay between teleport out and teleport in animations

        float currentY = transform.position.y;
        float teleportOffsetX = 1.5f;
        float teleportY = currentY;

        // Determine which side of the player the boss should teleport to
        float targetX = playerTransform.position.x - teleportOffsetX * Mathf.Sign(transform.localScale.x);
        if (Mathf.Sign(targetX - playerTransform.position.x) == Mathf.Sign(transform.position.x - playerTransform.position.x))
        {
            targetX = playerTransform.position.x + teleportOffsetX * Mathf.Sign(transform.localScale.x);
        }

        Vector2 targetPosition = new Vector2(targetX, teleportY);
        transform.position = targetPosition;

        // Add the teleport in animation
        animator.SetBool("IsTeleporting", true);
        yield return new WaitForSeconds(1f); // Adjust the time to match the duration of your teleport in animation
        animator.SetBool("IsTeleporting", false);

        float delay = 1f;
        yield return new WaitForSeconds(delay);

        Debug.Log("Performing attack animation");
        float distanceToPlayer = Vector2.Distance(attackPoint.position, playerTransform.position);

        if (distanceToPlayer <= attack2Range)
        {
            animator.SetBool(IsAttacking, true);
            animator.SetInteger(AttackType, 2);

            float damageDelay = 1;
            yield return new WaitForSeconds(damageDelay);

            Debug.Log("Dealing damage");
            playerHealthSystem.TakeDamage(phase0Attack2Damage, attackPoint.position);
        }

        animator.SetBool("IsWalking", true);
    }

    private IEnumerator Phase2Attack2()
    {
        Debug.Log("Starting Phase2Attack2");
        animator.SetBool("IsWalking", false);

        // Add the teleport out animation
        animator.SetBool("IsTeleporting", true);
        yield return new WaitForSeconds(1f); // Adjust the time to match the duration of your teleport out animation
        animator.SetBool("IsTeleporting", false);

        yield return new WaitForSeconds(0.5f); // Add a short delay between teleport out and teleport in animations

        float currentY = transform.position.y;
        float teleportOffsetX = 5.0f; // Adjust the teleport distance as needed
        float teleportY = currentY;

        // Determine which side of the player the boss should teleport to
        float targetX = playerTransform.position.x - teleportOffsetX * Mathf.Sign(transform.localScale.x);
        if (Mathf.Sign(targetX - playerTransform.position.x) == Mathf.Sign(transform.position.x - playerTransform.position.x))
        {
            targetX = playerTransform.position.x + teleportOffsetX * Mathf.Sign(transform.localScale.x);
        }

        Vector2 targetPosition = new Vector2(targetX, teleportY);
        transform.position = targetPosition;

        // Add the teleport in animation
        animator.SetBool("IsTeleporting", true);
        yield return new WaitForSeconds(1f); // Adjust the time to match the duration of your teleport in animation
        animator.SetBool("IsTeleporting", false);

        // Phase1Attack2-like magic attack
        for (int i = 0; i < magicAttackBurstCount; i++)
        {
            animator.SetBool(IsCasting, true);
            isCasting = true;
            yield return new WaitForSeconds(1.0f); // Ota huomioon Cast-animaation kesto
            StartCoroutine(SpawnMagicAttackAfterDelay(playerTransform.position, 0.5f));
            animator.SetBool(IsCasting, false);
            isCasting = false;
            yield return new WaitForSeconds(timeBetweenMagicAttacks);
        }

        animator.SetBool("IsWalking", true);
    }


    // Call this function to change the boss's phase
    public void ChangePhase(int newPhase)
    {
        currentPhase = newPhase;
    }
    public void TakeDamage(int damage)
    {
        if (currentPhase == 0 && healthSystem.GetCurrentHealth() <= phase2HealthThreshold)
        {
            Debug.Log("Entering the Phase 2");
            ChangePhase(1);
        }
        else if (currentPhase == 1 && healthSystem.GetCurrentHealth() <= 35)
        {
            Debug.Log("Entering the Phase 3");
            ChangePhase(2);
        }
    }

    private void SpawnMagicAttack(Vector2 position)
    {
        Vector2 spawnPosition = position + new Vector2(0, 5); // Adjust the Y offset value as needed
        Instantiate(magicAttackPrefab, spawnPosition, Quaternion.identity);
    }
    private IEnumerator SpawnMagicAttackAfterDelay(Vector2 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnMagicAttack(position);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a circle for the attack1 range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attack1Range);
        Gizmos.DrawWireSphere(transform.position, magicAttackRange);
    }

}

