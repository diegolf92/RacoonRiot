using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureState : EnemyBaseState
{
    private Transform enemyTransform;
    EnemyStateMachine fsm;

    public CaptureState(Transform transform, EnemyStateMachine fsm)
    {
        enemyTransform = transform;
        this.fsm = fsm;
    }

    public override void Enter()
    {
        fsm.anim.SetTrigger("isAttack");
    }

    public override void Update()
    {
        fsm.anim.SetBool("isCaptured", true);
    }

    public override void Exit()
    {
        fsm.anim.SetBool("isCaptured", false);
    }
}

