using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : EnemyBaseState
{
    private EnemyStateMachine fsm;
    private Transform enemyTransform;
    FieldOfView fovEnemy;
    Transform pointA, pointB;
    GameObject player;
    bool playerLeft;
    bool noMove;
    Color redColor = new Color(1, 0, 0, 0.2f);
    float waitTime = 3f;

    public RangeAttackState(GameObject player, EnemyStateMachine enemyStateMachine, Transform transform, FieldOfView fov)
    {
        this.player = player;
        enemyTransform = transform;
        fovEnemy = fov;
        fsm = enemyStateMachine;
    }

    public override void Enter()
    {
        fsm.anim.SetTrigger("specialAttack");
        fsm.Instantiator();
        fovEnemy.gameObject.SetActive(false);
        fovEnemy.transform.parent.GetComponent<SpriteRenderer>().color = redColor;
    }

    public override void Update()
    {
        //boomerang locks on player transform
        Vector3 target = new Vector3(player.transform.position.x, fsm.transform.position.y, fsm.transform.position.z);
        int teethDirection = fovEnemy.isFacingRight ? 1 : 0;
        if (teethDirection == 1) //Teeth going right->
        {
            //looks left
        }
        else if (teethDirection == 1) //Teeth going right->
        {
            //looksright
        }

        waitTime -= Time.deltaTime;
        if (waitTime <= 0f)
        {
            fsm.Alert();
        }
    }

    public override void Exit()
    {
        waitTime = 3f;
    }
}
