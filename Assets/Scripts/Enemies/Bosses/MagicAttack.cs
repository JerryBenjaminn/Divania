using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Set the damage value for the magic attack
    [SerializeField] private float lifetime = 3f; // Set the lifetime of the magic attack
    [SerializeField] private float animationDuration = 1f; // Set the duration of the magic attack animation

    private bool hasDamagedPlayer = false;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the magic attack after its lifetime
    }

    private void Update()
    {
        if (lifetime <= animationDuration)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            lifetime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the magic attack hits the player
        if (collision.CompareTag("Player") && !hasDamagedPlayer)
        {
            // Get the PlayerHealthSystem component
            PlayerHealthSystem playerHealthSystem = collision.GetComponent<PlayerHealthSystem>();

            // Apply damage to the player
            playerHealthSystem.TakeDamage(damage, transform.position);

            // Set hasDamagedPlayer to true to prevent multiple damage application
            hasDamagedPlayer = true;

            // Start a coroutine to destroy the magic attack after the animation duration
            StartCoroutine(DestroyAfterAnimation(animationDuration));
        }
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
