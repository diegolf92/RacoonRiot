using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractState : EnemyBaseState
{
    private float objectTriggerPosition;
    private EnemyStateMachine fsm;
    FieldOfView fovEnemy;
    float speed = 4;
    Color whiteColor = new Color(1, 1, 1, 0.3f);
    Color yellowColor = new Color(1, 1, 0, 0.3f);
    float waitTime = 3f;

    public DistractState(EnemyStateMachine enemyStateMachine, float posX, FieldOfView fov)
    {
        objectTriggerPosition = posX;
        fsm = enemyStateMachine;
        fovEnemy = fov;
    }

    public override void Enter()
    {
        fsm.anim.SetTrigger("isWalking");
        fovEnemy.gameObject.SetActive(true);
        fovEnemy.transform.parent.GetComponent<SpriteRenderer>().color = yellowColor;
        if (objectTriggerPosition < fsm.transform.position.x && fovEnemy.isFacingRight || objectTriggerPosition > fsm.transform.position.x && !fovEnemy.isFacingRight)
        {
           fovEnemy.Flip();
        }
    }

    public override void Update()
    {
        //store player pos
        Vector3 target = new Vector3(objectTriggerPosition, fsm.gameObject.transform.position.y, fsm.gameObject.transform.position.z);
        fsm.gameObject.transform.position = Vector3.MoveTowards(fsm.gameObject.transform.position, target, speed * Time.deltaTime);

        //calcular distancia hacia jugador
        float distanceToPos = Vector3.Distance(fsm.gameObject.transform.position, target);
        if (distanceToPos < 0.5f) fsm.anim.SetTrigger("isIdle");

        if (distanceToPos == 0)
        {
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
        waitTime = 3f;
        fovEnemy.transform.parent.GetComponent<SpriteRenderer>().color = whiteColor;
    }
}
