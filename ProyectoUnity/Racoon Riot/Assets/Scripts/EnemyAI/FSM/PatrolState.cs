using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    private Transform enemyTransform;
    private EnemyStateMachine fsm;
    float startXPos;
    FieldOfView fovEnemy;
    float speed = 4;
    Color whiteColor = new Color(1, 1, 1, 0.3f);
    Color yellowColor = new Color(1, 1, 0, 0.3f);
    float waitTime = 3f;
    Transform pointA, pointB;
    bool patrolRight = true;

    public PatrolState(EnemyStateMachine enemyStateMachine, Transform transform, FieldOfView fov, Transform pointA, Transform pointB)
    {
        enemyTransform = transform;
        fsm = enemyStateMachine;
        fovEnemy = fov;
        this.pointA = pointA;
        this.pointB = pointB;
    }

    public override void Enter()
    {
        fovEnemy.gameObject.SetActive(true);
        fovEnemy.transform.parent.GetComponent<SpriteRenderer>().color = whiteColor;
    }

    public override void Update()
    {
        if (patrolRight)
        {
            //store player pos
            Vector3 target = new Vector3(pointB.transform.position.x, fsm.gameObject.transform.position.y, fsm.gameObject.transform.position.z);
            fsm.gameObject.transform.position = Vector3.MoveTowards(fsm.gameObject.transform.position, target, speed * Time.deltaTime);

            //calcular distancia hacia jugador
            float distanceToPos = Vector3.Distance(fsm.gameObject.transform.position, target);
            if (distanceToPos < 0.5f) 
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
            if (distanceToPos < 0.5f)
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

    public override void Exit()
    {
        Debug.Log("Exiting state.");
        // Cleanup if necessary
    }
}
