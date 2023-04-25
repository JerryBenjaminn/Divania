using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    [SerializeField] private int meleeDamage;
    [SerializeField] private float meleeAttackDuration;
    [SerializeField] private WeaponHitArea weaponHitArea;

    private PlayerController playerController;
    private PlayerAnimatorController animatorController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        animatorController = GetComponent<PlayerAnimatorController>();
    }

    public void PerformMeleeAttack()
    {
        StartCoroutine(MeleeAttack());
    }

    private IEnumerator MeleeAttack()
    {
        animatorController.SetAttackAnimation(true);
        weaponHitArea.EnableHitDetection();

        yield return new WaitForSeconds(meleeAttackDuration);

        weaponHitArea.DisableHitDetection();
        animatorController.SetAttackAnimation(false);
    }

}
