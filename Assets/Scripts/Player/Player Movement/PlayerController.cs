using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float variableJumpHeightMultiplier;

    //Jump settings
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown = 0.2f;

    //Variables for jumping
    private float jumpTimer;
    private bool jumpRequest;
    private bool wasGrounded;

    //Variables for checking if the player is on the ground
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;

    //Variables for the Coyote time (player can jump for a short moment after leaving a ledge or a platform)
    [Header("Coyote Time Settings")]
    [SerializeField] private float coyoteTimeDuration = 0.2f;
    private float coyoteTime;

    private Rigidbody2D rb;
    private float moveInput;

    private PlayerAnimatorController animatorController;
    private PlayerCombatController combatController;

    private PlayerController instance;

    //Groundcheck boolean
    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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
        //Assigning the check to the variable
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //If player is grounded reset the coyote time
        if (isGrounded)
        {
            coyoteTime = coyoteTimeDuration;
        }

        //If player is in the air start reducing the coyote time
        else if (wasGrounded)
        {
            coyoteTime -= Time.deltaTime;
        }

        //Start to increase the jump timer until it reaches the value of the cooldown
        if(jumpTimer < jumpCooldown)
        {
            jumpTimer += Time.deltaTime;
        }

        //Check if the jump timer is same or higher that the cooldown when pressing the jump button
        if (Input.GetButtonDown("Jump") && jumpTimer >= jumpCooldown)
        {
            jumpRequest = true;
            jumpTimer = 0;
        }

        //Saving the ground check for the next frame
        wasGrounded = isGrounded;

        //Call the methods to get input and flip the player sprite
        GetInput();
        FlipPlayer(moveInput);
        
        //Set the values for the animator
        animatorController.SetMoveAnimation(moveInput);
        animatorController.SetJumpAnimation(rb.velocity.y);
        animatorController.SetGroundedAnimation(isGrounded);

        //If the left mouse button is presses, perform a melee attack
        if (Input.GetMouseButtonDown(0))
        {
            combatController.PerformMeleeAttack();
        }
    }
    private void FixedUpdate()
    {
        //Call the move and jump methods. Since using rigidbody we are calling from the fixed update
        Move();
        Jump();
    }

    private void GetInput()
    {
        //Get the player input for the movement
        moveInput = Input.GetAxis("Horizontal");
    }
    private void Move()
    {   
        //Assign the vector for the movement
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        //If the request for the jump has been made and coyote time is active, perform jump
        if (jumpRequest && coyoteTime > 0)
        {
            rb.AddForce(Vector2.up.normalized * jumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
        }

        //If the player is in the air and the jump button is released and player is moving upwards, adjust the jump height
        if (!isGrounded && Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void FlipPlayer(float direction)
    {
        //Flips the player in the right direction when moving
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
        //Draws a gizmo to see the ground check
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
