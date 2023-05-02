using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    [SerializeField] private int meleeDamage;
    [SerializeField] private float meleeAttackDuration;
    [SerializeField] private WeaponHitArea weaponHitArea;
    public int MeleeDamage => meleeDamage;

    private bool isJumpAttacking;
    private bool isAttacking;

    private PlayerController playerController;
    private PlayerAnimatorController animatorController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        animatorController = GetComponent<PlayerAnimatorController>();
    }
    private void Update()
    {
        if (isJumpAttacking && playerController.IsGrounded)
        {
            animatorController.SetJumpAttackAnimation(false);
            isJumpAttacking = false;
        }
    }
    public void PerformMeleeAttack()
    {

        if (playerController.IsGrounded)
        {
            StartCoroutine(MeleeAttack());
        }
        else
        {
            StartCoroutine(JumpAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        animatorController.SetAttackAnimation(true);
        weaponHitArea.EnableHitDetection();
        
        yield return new WaitForSeconds(meleeAttackDuration);

        isAttacking = false;
        weaponHitArea.DisableHitDetection();
        animatorController.SetAttackAnimation(false);
    }

    private IEnumerator JumpAttack()
    {
        isJumpAttacking = true;
        animatorController.SetJumpAttackAnimation(true);
        weaponHitArea.EnableHitDetection();

        yield return new WaitForSeconds(meleeAttackDuration);

        isJumpAttacking = false;
        weaponHitArea.DisableHitDetection();
        animatorController.SetJumpAttackAnimation(false);
    }
}
