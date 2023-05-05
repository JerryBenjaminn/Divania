using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private int currentPhase = 0;
    [SerializeField] private float timeBetweenAttacks = 3.0f;
    private float timeSinceLastAttack = 0.0f;
    private bool isAttacking = false;

    private void Update()
    {
        if (!isAttacking && Time.time >= timeSinceLastAttack + timeBetweenAttacks)
        {
            StartCoroutine(PerformAttack());
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
                if (Random.Range(0, 2) == 0)
                    Phase0Attack1();
                else
                    Phase0Attack2();
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

        timeSinceLastAttack = Time.time;
        isAttacking = false;
    }

    // Call this function to change the boss's phase
    public void ChangePhase(int newPhase)
    {
        currentPhase = newPhase;
    }
    private void Phase0Attack1()
    {
        // Execute attack 1 for phase 0
    }

    private void Phase0Attack2()
    {
        // Execute attack 2 for phase 0
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

}

