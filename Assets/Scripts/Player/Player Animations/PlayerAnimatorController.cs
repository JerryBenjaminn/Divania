using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void SetMoveAnimation(float speed)
    {
        animator.SetFloat("Running", Mathf.Abs(speed));
    }
    public void SetJumpAnimation(float verticalVelocity)
    {
        animator.SetFloat("VerticalVelocity", verticalVelocity);

        if(verticalVelocity > 0)
        {
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
        }
        else if(verticalVelocity < 0)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
    }

    public void SetGroundedAnimation(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);

        if (isGrounded)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }
    }

    public void SetAttackAnimation(bool isAttacking)
    {
        animator.SetBool("isAttacking", isAttacking);
    }
}
