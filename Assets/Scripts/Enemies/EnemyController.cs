using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    public void TakeDamage(int damage)
    {
        if(healthSystem == null)
        {
            Debug.Log("HealthSystem is null");
            return;
        }

        healthSystem.TakeDamage(damage);
        Debug.Log(damage);

        if (healthSystem.IsDead())
        {
            Destroy(gameObject);
        }
    }

}
