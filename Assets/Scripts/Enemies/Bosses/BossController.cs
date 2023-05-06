using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private int currentPhase = 0;
    [SerializeField] private float timeBetweenAttacks = 3.0f;
    private float timeSinceLastAttack = 0.0f;
    private bool isAttacking = false;

    private Transform playerTransform;
    private Rigidbody2D bossRigidbody;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attack1Range = 2.0f;

    [SerializeField] private GameObject magicAttackPrefab;
    [SerializeField] private float magicAttackRange = 10f;

    private Animator animator;
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int AttackType = Animator.StringToHash("AttackType");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsCasting = Animator.StringToHash("IsCasting");


    private void Awake()
    {
        bossRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!isAttacking && Time.time >= timeSinceLastAttack + timeBetweenAttacks)
        {
            StartCoroutine(PerformAttack());
        }
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

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Choose an attack based on the current phase
        switch (currentPhase)
        {
            case 0:
                // Execute attack 1 or attack 2 for phase 0
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer <= attack1Range)
                {
                    Phase0Attack1();
                }
                else if (distanceToPlayer <= magicAttackRange)
                {
                    StartCoroutine(Phase0Attack2());
                }
                break;
            case 1:
                // Execute attack 1 or attack 2 for phase 1
                if (Random.Range(0, 2) == 0)
                    Phase1Attack1();
                else
                    Phase1Attack2();
                break;
            case 2:
                // Execute attack 1 or attack 2 for phase 2
                if (Random.Range(0, 2) == 0)
                    Phase2Attack1();
                else
                    Phase2Attack2();
                break;
        }

        yield return new WaitForSeconds(1.0f); // Wait for attack animation to finish

        // Set IsAttacking to false after the attack animation has finished
        animator.SetBool(IsAttacking, false);

        timeSinceLastAttack = Time.time;
        isAttacking = false;
    }

    private void MoveBoss(Vector2 direction)
    {
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
        // Implement the movement logic for phase 1 here
    }

    private void MoveBossPhase2()
    {
        // Implement the movement logic for phase 2 here
    }


    private void Phase0Attack1()
    {
        // Execute attack 1 for phase 0
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attack1Range)
        {
            // Execute the melee attack animation here
            animator.SetBool(IsAttacking, true);
            animator.SetInteger(AttackType, 1);
            // Deal damage to the player
        }
    }

    private IEnumerator Phase0Attack2()
    {
        // Set IsCasting to true to start the Cast animation
        animator.SetBool(IsCasting, true);

        // Wait for the Cast animation to finish
        yield return new WaitForSeconds(1.0f); // Adjust the waiting time according to your Cast animation length

        // Spawn the magic attack
        SpawnMagicAttack(playerTransform.position);

        // Set IsCasting back to false
        animator.SetBool(IsCasting, false);
    }

    private void Phase1Attack1()
    {
        // Execute attack 1 for phase 1
    }

    private void Phase1Attack2()
    {
        // Execute attack 2 for phase 1
    }

    private void Phase2Attack1()
    {
        // Execute attack 1 for phase 2
    }

    private void Phase2Attack2()
    {
        // Execute attack 2 for phase 2
    }

    // Call this function to change the boss's phase
    public void ChangePhase(int newPhase)
    {
        currentPhase = newPhase;
    }
    private void SpawnMagicAttack(Vector2 position)
    {
        Instantiate(magicAttackPrefab, position, Quaternion.identity);
    }
    private void OnDrawGizmosSelected()
    {
        // Draw a circle for the attack1 range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attack1Range);
        Gizmos.DrawWireSphere(transform.position, magicAttackRange);
    }

}

