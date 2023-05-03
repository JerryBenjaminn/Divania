using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthSystem : MonoBehaviour
{
    //Reference to the health system
    [SerializeField] private HealthSystem healthSystem;

    [SerializeField] private Animator animator;

    public bool isDying = false;

    public void TakeDamage(int damage)
    {
        //Check that the health system is active, otherwise message an error and return
        if(healthSystem == null)
        {
            Debug.Log("HealthSystem is null");
            return;
        }

        //Substract the damage from the hp
        healthSystem.TakeDamage(damage);
        Debug.Log(damage);

        //If the health reached zero, the enemy is destroyed
        if (healthSystem.IsDead())
        {
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()
    {
        isDying = true;
        // Play the die animation
        animator.SetTrigger("Die");

        // Wait for the length of the animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destroy the game object
        Destroy(gameObject);
    }

}
