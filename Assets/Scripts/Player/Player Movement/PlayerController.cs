using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float variableJumpHeightMultiplier;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown = 0.2f;

    private float jumpTimer;
    private bool jumpRequest;
    private bool wasGrounded;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;

    [Header("Coyote Time Settings")]
    [SerializeField] private float coyoteTimeDuration = 0.2f;
    private float coyoteTime;

    private Rigidbody2D rb;
    private float moveInput;

    private PlayerAnimatorController animatorController;
    private PlayerCombatController combatController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<PlayerAnimatorController>();
        combatController = GetComponent<PlayerCombatController>();

        coyoteTime = coyoteTimeDuration;
        jumpTimer = jumpCooldown;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTime = coyoteTimeDuration;
        }
        else if (wasGrounded)
        {
            coyoteTime -= Time.deltaTime;
        }

        if(jumpTimer < jumpCooldown)
        {
            jumpTimer += Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && jumpTimer >= jumpCooldown)
        {
            jumpRequest = true;
            jumpTimer = 0;
        }

        wasGrounded = isGrounded;

        GetInput();
        FlipPlayer(moveInput);
        
        animatorController.SetMoveAnimation(moveInput);
        animatorController.SetJumpAnimation(rb.velocity.y);
        animatorController.SetGroundedAnimation(isGrounded);

        if (Input.GetMouseButtonDown(0))
        {
            combatController.PerformMeleeAttack();
        }
    }
    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void GetInput()
    {
        moveInput = Input.GetAxis("Horizontal");
    }
    private void Move()
    {      
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (jumpRequest && coyoteTime > 0)
        {
            rb.AddForce(Vector2.up.normalized * jumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
        }

        if (!isGrounded && Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void FlipPlayer(float direction)
    {
        if (direction > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
