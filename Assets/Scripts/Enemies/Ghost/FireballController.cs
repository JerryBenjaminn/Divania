using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] private CharacterStats fireBallDamage;
    [SerializeField] private int projectileDamage;

    [SerializeField] private float destroyTime;

    private void Start()
    {
        projectileDamage = fireBallDamage.attackPower;

        Destroy(gameObject, 3f);
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
            playerHealth.TakeDamage(fireBallDamage.attackPower, contactPoint);

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }
    }
}
