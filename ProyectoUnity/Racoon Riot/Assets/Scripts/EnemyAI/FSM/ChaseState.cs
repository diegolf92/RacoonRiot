using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyBaseState
{
    private EnemyStateMachine fsm;
    private Transform enemyTransform;
    FieldOfView fovEnemy;
    Transform pointA, pointB;
    GameObject player;
    float chaseSpeed = 5;
    bool playerLeft;
    bool noMove;
    Color redColor = new Color(1, 0, 0, 0.3f);
    float waitTime = 3f;
    float flipTime = 1f;

    public ChaseState(GameObject player, EnemyStateMachine enemyStateMachine,Transform transform, Transform pointA, Transform pointB, FieldOfView fov)
    {
        this.player = player;
        enemyTransform = transform;
        fovEnemy = fov;
        fsm = enemyStateMachine;
        this.pointA = pointA;
        this.pointB = pointB;
    }

    public override void Enter()
    {
        fsm.anim.SetTrigger("isWalking");
        fovEnemy.gameObject.SetActive(false);
        fovEnemy.transform.parent.GetComponent<SpriteRenderer>().color = redColor;
    }

    public override void Update()
    {
        //store player pos
        Vector3 boundaryTest = CheckIfEnemyWithinBoundaries(player.transform);
        Vector3 target = new Vector3(boundaryTest.x, fsm.gameObject.transform.position.y, fsm.gameObject.transform.position.z);
        fsm.gameObject.transform.position = Vector3.MoveTowards(fsm.gameObject.transform.position, target, chaseSpeed * Time.deltaTime);

        if (player.transform.position.x < fsm.gameObject.transform.position.x) playerLeft = true;
        else playerLeft = false;

        if (playerLeft && fovEnemy.isFacingRight && !noMove || !playerLeft && !fovEnemy.isFacingRight && !noMove)
        {
            noMove = true;
            fovEnemy.Flip();
        }

        if (noMove == true)
        {
            chaseSpeed = 0.2f;
            //wait few seconds
            flipTime -= Time.deltaTime;
            if (flipTime <= 0f)
            {
                fsm.Alert();
            }
        }
        
        //calcular distancia hacia jugador
        float distanceToPlayerPos = Vector3.Distance(fsm.gameObject.transform.position, target);
        
        if (distanceToPlayerPos < 1.5f)
        {
            //fsm.Alert();
        }
        else if (distanceToPlayerPos > 10)
        {
            //send to alert so it recharges player position coordinates
            //fsm.Alert();
        } else 
        {
            //Debug.Log("Chasing");
        }

        //check if hits limits point
        if (playerLeft && fsm.transform.position.x <= pointA.position.x || !playerLeft && fsm.transform.position.x >= pointB.position.x)
        {
            fsm.anim.SetTrigger("isIdle");
            //wait few seconds
            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                fsm.Alert();
            }
        }
    }

    public override void Exit()
    {
        fsm.anim.SetTrigger("isIdle");
        chaseSpeed = 5f;
        noMove = false;
        flipTime = 1f;
        waitTime = 3f;
        fovEnemy.gameObject.SetActive(true);
        // Cleanup if necessary
    }

    Vector3 CheckIfEnemyWithinBoundaries(Transform positionToTest)
    {
        if (positionToTest.position.x < pointA.position.x)
        {
            //this position is outside of boundaries on the left
            return pointA.position;
        }
        else if (positionToTest.position.x > pointB.position.x)
        {
            //this position is outside of boundaries on the right
            return pointB.position;
        }
        else
        {
            //this position is inside of boundaries
            return positionToTest.position;
        }
    }
}

