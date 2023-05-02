using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthSystem : MonoBehaviour
{
    //Reference to the health system
    [SerializeField] private HealthSystem healthSystem;

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
            Destroy(gameObject);
        }
    }

}
