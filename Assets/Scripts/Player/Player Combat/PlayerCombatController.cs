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
        //Reset jump attack if player is grounded
        if (isJumpAttacking && playerController.IsGrounded)
        {
            animatorController.SetJumpAttackAnimation(false);
            isJumpAttacking = false;
        }
    }
    public void PerformMeleeAttack()
    {
        //Attack depending if player is in the air or not
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
        //Start the attack animation
        animatorController.SetAttackAnimation(true);

        //Enable hit detection
        weaponHitArea.EnableHitDetection();
        
        //Wait for the duration of the attack
        yield return new WaitForSeconds(meleeAttackDuration);

        //Disable hit detection
        weaponHitArea.DisableHitDetection();

        //Stop the attack animation
        animatorController.SetAttackAnimation(false);
    }

    private IEnumerator JumpAttack()
    {
        //Start the jump attack
        isJumpAttacking = true;

        //Start the jump attack animation
        animatorController.SetJumpAttackAnimation(true);

        //Enable hit detection
        weaponHitArea.EnableHitDetection();

        //Wait for the duration of the attack
        yield return new WaitForSeconds(meleeAttackDuration);

        //Stop the jump attack
        isJumpAttacking = false;

        //Disable hit detection
        weaponHitArea.DisableHitDetection();

        //Stop the jump attack animation
        animatorController.SetJumpAttackAnimation(false);
    }
}
