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
    Vector3 boundaryTest;
    float chaseSpeed = 5;
    bool playerLeft;

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
        boundaryTest = CheckIfEnemyWithinBoundaries(player.transform);
        fovEnemy.gameObject.SetActive(false);
    }

    public override void Update()
    {
        //get player position
        Vector3 target = new Vector3(boundaryTest.x, fsm.gameObject.transform.position.y, fsm.gameObject.transform.position.z);
        fsm.gameObject.transform.position = Vector3.MoveTowards(fsm.gameObject.transform.position, target, chaseSpeed * Time.deltaTime);

        if (player.transform.position.x < fsm.gameObject.transform.position.x)
        {
            playerLeft = true;
        }
        //calcular distancia hacia jugador
        Vector2 direction = playerLeft ? Vector2.right : Vector2.left;
        float distanceToPlayer = Vector3.Distance(fsm.gameObject.transform.position, player.transform.position);

        if (distanceToPlayer < 1)
        {
            Debug.Log("CAPTURE");
        }
        else if (distanceToPlayer > 5)
        {
            //send to alert so it recharges player position coordinates
            Debug.Log("Enemy scape");
        } else 
        {
            Debug.Log("Chasing");
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting state.");
        // Cleanup if necessary
    }

    Vector3 CheckIfEnemyWithinBoundaries(Transform positionToTest)
    {
        if (positionToTest.position.x < pointA.position.x)
        {
            //this position is outside of boundaries
            return pointA.position;
        }
        else if (positionToTest.position.x > pointB.position.x)
        {
            return pointB.position;
        }
        else
        {
            //this position is inside of boundaries
            return positionToTest.position;
        }
    }
}

