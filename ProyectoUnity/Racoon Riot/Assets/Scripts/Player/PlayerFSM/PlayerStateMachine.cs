using ENEMYAI.FSM;
using PLAYERAI.PlayerFSM;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerBaseState currentState;
    Rigidbody2D rb;
    Animator anim;

    // States
    private NormalState normalState;
    private CapturedState capturedState;
    private RecoveringState recoveringState;

    [Header("Public Variables")]
    public Transform feetPosFront;
    public Transform feetPosBack;
    public Transform ceilingPos;
    public Transform wallPos;
    public PhysicsMaterial2D[] raccoonMaterial;

    void Start()
    {
        //Initialize variables
        rb = GetComponent<Rigidbody2D>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        // Initialize the normalState and other states with this object's transform
        normalState = new NormalState(this, rb, anim);
        capturedState = new CapturedState();
        recoveringState = new RecoveringState();

        TransitionToState(normalState);
    }

    void Update()
    {
        // Call the current state's Update method
        currentState?.Update();
    }

    public void TransitionToState(PlayerBaseState newState)
    {
        // Exit the current state
        currentState?.Exit();

        // Set the new state and call Enter
        currentState = newState;
        currentState.Enter();
    }

    // Example methods to trigger state transitions
    public void Normal()
    {
        TransitionToState(normalState);
    }
}
