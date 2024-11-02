using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : EnemyBaseState
{
    private Transform enemyTransform;
    private float flipInterval = 2f;  // Time in seconds between flips
    private float flipTimer;
    private bool isFacingRight = true;

    SpriteRenderer FOV;
    Color whiteColor = new Color(1, 1, 1, 0.3f);
    Color yellowColor = new Color(1, 1, 0, 0.3f);
    Color redColor = new Color(1, 0, 0, 0.3f);
    float visionRange = 10f;
    float visionAngle = 60f;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;


    public GuardState(Transform transform)
    {
        enemyTransform = transform;
    }

    public override void Enter()
    {
        Debug.Log("Entering GUARD state.");
        // Add initialization for GUARD behavior here
        flipTimer = flipInterval;  // Reset the timer when entering the state
        obstacleLayer = 3;
        playerLayer = 7;
    }

    public override void Update()
    {
        //FLIP mechanic
        flipTimer -= Time.deltaTime;
        // Check if it's time to flip
        if (flipTimer <= 0f)
        {
            Flip();
            flipTimer = flipInterval;  // Reset the timer
        }

        //Check for player within FOV
        DetectPlayerInFOV();
    }

    public override void Exit()
    {
        Debug.Log("Exiting GUARD state.");
        // Cleanup if necessary
    }

    private void Flip()
    {
        // Toggle the facing direction
        isFacingRight = !isFacingRight;

        // Flip the enemy's scale on the X-axis
        Vector3 scale = enemyTransform.localScale;
        scale.x *= -1;
        enemyTransform.localScale = scale;
    }

    private void DetectPlayerInFOV()
    {
        // Calculate the direction the enemy is facing
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        // Cast rays within the FOV
        for (float angle = -visionAngle / 2; angle < visionAngle / 2; angle += 5f)
        {
            // Calculate the direction of each ray within the FOV
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Perform the raycast
            RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, rayDirection, visionRange, obstacleLayer | playerLayer);
            if (hit.collider != null)
            {
                if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
                {
                    Debug.Log("Player detected within field of view!");
                    // Trigger a state transition to chase or alert, etc.
                    // e.g., enemyStateMachine.Chase();
                    break;
                }
                else if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
                {
                    Debug.Log("Wall detected within field of view, blocking vision.");
                }
            }

            // Visualize the ray in the editor (for debugging)
            Debug.DrawRay(enemyTransform.position, rayDirection * visionRange, Color.red);
        }
    }
}
