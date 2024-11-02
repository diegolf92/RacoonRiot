using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureState : EnemyBaseState
{
    private Transform enemyTransform;

    public CaptureState(Transform transform)
    {
        enemyTransform = transform;
    }

    public override void Enter()
    {
        Debug.Log("Entering state.");
        // Add initialization for GUARD behavior here
    }

    public override void Update()
    {
        Debug.Log("Updating state.");
        // Add GUARD logic here
    }

    public override void Exit()
    {
        Debug.Log("Exiting state.");
        // Cleanup if necessary
    }
}

