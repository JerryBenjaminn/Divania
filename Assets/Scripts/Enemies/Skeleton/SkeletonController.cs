using UnityEngine;


public class SkeletonController : EnemyController
{
    [SerializeField] private Animator animator; // Reference to the animator component
    protected bool hasRisen = false;
    protected override void Update()
    {
        base.Update();

        // Check if the player is within the detection range and the rise animation has not been triggered
        if (isPlayerDetected && !hasRisen)
        {
            // Trigger the rise animation
            animator.SetTrigger("Rise");
        }
    }

    // This function is called from the animator at the end of the rise animation
    protected override bool CanMove()
    {
        return hasRisen;
    }

    //This is an animation event
    private void RiseComplete()
    {
        hasRisen = true;
    }
}

