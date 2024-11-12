using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : EnemyBaseState
{
    private Transform enemyTransform;
    private EnemyStateMachine fsm;
    float startXPos;
    FieldOfView fovEnemy;
    //float speed = 4;
    Color whiteColor = new Color(1, 1, 1, 0.3f);
    Color yellowColor = new Color(1, 1, 0, 0.3f);
    float waitTime = 3f;

    public AlertState(EnemyStateMachine enemyStateMachine, Transform transform, FieldOfView fov, float ogPos)
    {
        enemyTransform = transform;
        startXPos = ogPos;
        fsm = enemyStateMachine;
        fovEnemy = fov;
    }

    public override void Enter()
    {
        fsm.anim.SetTrigger("isIdle");
        fovEnemy.gameObject.SetActive(true);
        fovEnemy.transform.parent.GetComponent<SpriteRenderer>().color = yellowColor;
    }

    public override void Update()
    {
        //wait few seconds
        waitTime -= Time.deltaTime;
        if (waitTime <= 0f)
        {
            if(!fsm.canPatrol)fsm.Guard();
            else fsm.Patrol();
        }
    }

    public override void Exit()
    {
        waitTime = 3f;
        fovEnemy.transform.parent.GetComponent<SpriteRenderer>().color = whiteColor;
    }

}

