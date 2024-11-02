using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyBaseState currentState;

    // States
    private GuardState guardState;
    private PatrolState patrolState;
    private AlertState alertState;
    private ChaseState chaseState;
    private CaptureState captureState;

    void Start()
    {
        // Initialize the guardState and other states with this object's transform
        guardState = new GuardState(transform);
        patrolState = new PatrolState(transform);
        alertState = new AlertState(transform);
        chaseState = new ChaseState(transform);
        captureState = new CaptureState(transform);

        // Start in the GUARD state
        TransitionToState(guardState);
    }

    void Update()
    {
        // Call the current state's Update method
        currentState?.Update();
    }

    public void TransitionToState(EnemyBaseState newState)
    {
        // Exit the current state
        currentState?.Exit();

        // Set the new state and call Enter
        currentState = newState;
        currentState.Enter();
    }

    // Example methods to trigger state transitions
    public void Alert()
    {
        TransitionToState(alertState);
    }

    public void Patrol()
    {
        TransitionToState(patrolState);
    }

    public void Chase()
    {
        TransitionToState(chaseState);
    }

    public void Capture()
    {
        TransitionToState(captureState);
    }
}
