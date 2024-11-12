using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackState : EnemyBaseState
{
    private EnemyStateMachine fsm;
    private Transform enemyTransform;
    FieldOfView fovEnemy;
    Transform playerPosition;
    bool playerLeft;
    bool noMove;
    float waitTime = 3f;
    float jumpForce = 10f;
    Rigidbody2D rb;
    float detectionRange = 1f; // Range within which the enemy can detect the player
    LayerMask groundLayer = 3;
    bool patrolRight = true;
    Transform pointA, pointB;
    float speed = 4;
    bool isJumpAttacking = false;

    public JumpAttackState(Transform playerPos, EnemyStateMachine enemyStateMachine, Transform transform, FieldOfView fov, Transform pointA, Transform pointB)
    {
        playerPosition = playerPos;
        enemyTransform = transform;
        fovEnemy = fov;
        fsm = enemyStateMachine;
        this.pointA = pointA;
        this.pointB = pointB;
    }

    public override void Enter()
    {
        rb = fsm.GetComponent<Rigidbody2D>();
        fsm.anim.SetTrigger("jumpAttack");
        fovEnemy.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (!isJumpAttacking)
        {
            //Check for player within FOV
            fovEnemy.DetectLayers();
            if (fovEnemy.playerDetected == true)
            {
                if (fsm.oldMan == true) fsm.RangeAttack();
                if (fsm.isDog == true) fsm.JumpAttack();
                else fsm.Chase();
            }

            if (patrolRight)
            {
                //store player pos
                Vector3 target = new Vector3(pointB.transform.position.x, fsm.gameObject.transform.position.y, fsm.gameObject.transform.position.z);
                fsm.gameObject.transform.position = Vector3.MoveTowards(fsm.gameObject.transform.position, target, speed * Time.deltaTime);

                //calcular distancia hacia jugador
                float distanceToPos = Vector3.Distance(fsm.gameObject.transform.position, target);
                if (distanceToPos < 0.3f)
                {
                    fsm.anim.SetTrigger("isIdle");
                    //wait few seconds
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0f)
                    {
                        fovEnemy.Flip();
                        waitTime = 3f;
                        patrolRight = false;
                    }
                }
                else fsm.anim.SetTrigger("isWalking");
            }
            else
            {
                //store player pos
                Vector3 target = new Vector3(pointA.transform.position.x, fsm.gameObject.transform.position.y, fsm.gameObject.transform.position.z);
                fsm.gameObject.transform.position = Vector3.MoveTowards(fsm.gameObject.transform.position, target, speed * Time.deltaTime);

                //calcular distancia hacia jugador
                float distanceToPos = Vector3.Distance(fsm.gameObject.transform.position, target);
                if (distanceToPos < 0.3f)
                {
                    fsm.anim.SetTrigger("isIdle");
                    //wait few seconds
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0f)
                    {
                        fovEnemy.Flip();
                        waitTime = 3f;
                        patrolRight = true;
                    }
                }
                else fsm.anim.SetTrigger("isWalking");
            }
        }

        // Check if the player is within detection range
        float distanceToPlayer = Vector2.Distance(fsm.transform.position, playerPosition.position);
        if (distanceToPlayer <= detectionRange)
        {
            // Check if the player is above the enemy
            if (playerPosition.position.y > fsm.transform.position.y)
            {
                isJumpAttacking = true;
                JumpAttack();
                /*
                // Check for a platform between the enemy and player
                Vector2 direction = (playerPosition.position - fsm.transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(fsm.transform.position, direction, detectionRange);

                if (hit.collider != null && hit.collider.CompareTag("Player") && IsGrounded())
                {
                    JumpAttack();
                }*/
            }
        }

        /*
        waitTime -= Time.deltaTime;
        if (waitTime <= 0f)
        {
            fsm.Alert();
        }*/
    }

    private bool IsGrounded()
    {
        // Perform a raycast downwards to check if the enemy is grounded
        RaycastHit2D hit = Physics2D.Raycast(fsm.transform.position, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    private void JumpAttack()
    {
        // Apply a vertical force for the jump attack
        //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        rb.AddForce(fsm.transform.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        waitTime = 3f;
    }
}
