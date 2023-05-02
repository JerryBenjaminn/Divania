using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int attackPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defencePower;

    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeAttackDuration;
    [SerializeField] private WeaponHitArea weaponHitArea;
    public int MeleeDamage => attackPower;

    private bool isJumpAttacking;

    private PlayerController playerController;
    private PlayerAnimatorController animatorController;

    [SerializeField] private CharacterStats playerStats;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        animatorController = GetComponent<PlayerAnimatorController>();

        attackPower = playerStats.attackPower;
        defencePower = playerStats.defensePower;
        maxHealth = playerStats.maxHealth;
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

        animatorController.SetAttackAnimation(true);
        weaponHitArea.EnableHitDetection();
        
        yield return new WaitForSeconds(meleeAttackDuration);


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
