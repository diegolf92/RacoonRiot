using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : EnemyBaseState
{
    private EnemyStateMachine fsm;
    private Transform enemyTransform;
    private float flipInterval = 2f;  // Time in seconds between flips
    private float flipTimer;
    private bool isFacingRight = true;

    SpriteRenderer FOV;
    FieldOfView fovEnemy;
    Color whiteColor = new Color(1, 1, 1, 0.3f);
    Color yellowColor = new Color(1, 1, 0, 0.3f);
    Color redColor = new Color(1, 0, 0, 0.3f);
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;

    public GuardState(EnemyStateMachine enemyStateMachine, Transform transform, FieldOfView fov)
    {
        enemyTransform = transform;
        fovEnemy = fov;
        fsm = enemyStateMachine;
    }

    public override void Enter()
    {
        // Add initialization for GUARD behavior here
        flipTimer = flipInterval;  // Reset the timer when entering the state
        obstacleLayer = 3;
        playerLayer = 7;
        fovEnemy.gameObject.SetActive(true);
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
        fovEnemy.DetectLayers(isFacingRight);
        if (fovEnemy.playerDetected == true)
        {
            fsm.Chase();
        }
    }

    public override void Exit()
    {
        fovEnemy.gameObject.SetActive(false);
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
}
