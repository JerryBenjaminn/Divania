using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterHealthSystem : MonoBehaviour
{
    //Reference to the health system
    [SerializeField] private HealthSystem healthSystem;
    //Reference to the animator
    [SerializeField] private Animator animator;
    //Reference to the rigidbody2d
    private Rigidbody2D rb2D;
    //Reference to the enemy
    private EnemyController enemy;

    [Header("Hurt settings")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float flashDuration = 0.1f;
    [SerializeField] protected int numberOfFlashes = 3;
    [SerializeField] protected float knockbackForce = 5;
    [SerializeField] protected float knockbackDuration = 0.5f;
    [SerializeField] protected float knockbackAngle = 45f;

    public bool isDying = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyController>();
        Debug.Log(enemy);
    }
    public virtual void TakeDamage(int damage, Vector2 damageDealer)
    {
        //Check that the health system is active, otherwise message an error and return
        if(healthSystem == null)
        {
            Debug.Log("HealthSystem is null");
            return;
        }
        Debug.Log("Player position: " + transform.position);
        Debug.Log("Damage dealer position: " + damageDealer);
        //Substract the damage from the hp
        healthSystem.TakeDamage(damage);
        Debug.Log(damage);

        if (!isDying)
        {
            animator.SetTrigger("Hurt");
            StartCoroutine(FlashSprite());
        }

        //If the health reached zero, the enemy is destroyed
        if (healthSystem.IsDead())
        {
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()
    {
        DisableColliderAndRigidbody();
        isDying = true;
        // Play the die animation
        animator.SetTrigger("Die");

        // Wait for the length of the animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destroy the game object
        Destroy(gameObject);
    }
    public virtual IEnumerator FlashSprite()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.material.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material.color = Color.clear;
            yield return new WaitForSeconds(flashDuration);
        }
        spriteRenderer.material.color = Color.white; // reset color to normal
    }
    private void DisableColliderAndRigidbody()
    {
        Collider2D collider = GetComponent<Collider2D>();
        
        if (collider != null)
        {
            collider.enabled = false;            
        }
        if(rb2D != null)
        {
            rb2D.simulated = false;
        }
    }

}
