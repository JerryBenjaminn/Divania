using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitArea : MonoBehaviour
{
    [SerializeField] private PlayerCombatController playerCombatController;
    [SerializeField] private LayerMask enemyLayers;

    private bool canHit;

    private List<Collider2D> collidersInHitArea = new List<Collider2D>();
    private List<Collider2D> recentlyHitColliders = new List<Collider2D>();

    private void FixedUpdate()
    {
        // Tarkistetaan kaikki collidereiden osumat, kun hyökkäys on aktiivinen
        if (canHit)
        {
            for (int i = collidersInHitArea.Count - 1; i >= 0; i--)
            {
                Collider2D collider = collidersInHitArea[i];
                // Tarkistetaan, onko collider jo osunut viimeisen hyökkäyksen aikana
                if (!recentlyHitColliders.Contains(collider))
                {
                    HandleCollision(collider);
                    // Pidetään kirjaa hiljattain osuneista collidereista
                    recentlyHitColliders.Add(collider);
                }
            }
        }
    }


    //If a collider comes in range of the attack, add it to a list to keep track if it can be attacked
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidersInHitArea.Add(collision);
    }

    //If a collider leaves the range, remove it from the list
    private void OnTriggerExit2D(Collider2D collision)
    {
        collidersInHitArea.Remove(collision);
        recentlyHitColliders.Remove(collision);
    }

    //If collision happens with enemy, handle the damage
    private void HandleCollision(Collider2D collision)
    {
        if ((enemyLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Enemy hit");
            // Vahingon aiheutus siirretään tänne
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(playerCombatController.MeleeDamage);
            }
        }
    }

    //Activates the hit detection during the attack
    public void EnableHitDetection()
    {
        canHit = true;
        Debug.Log("Hit detection enabled");
    }

    //Disables the hit detection and removes the colliders from the list which have been hit
    public void DisableHitDetection()
    {
        canHit = false;
        recentlyHitColliders.Clear();
        Debug.Log("Hit detection disabled");
    }

    //Makes sure that the PlayerCombatController is active
    private void Awake()
    {
        if (playerCombatController == null)
        {
            playerCombatController = GetComponentInParent<PlayerCombatController>();
        }
    }
}
