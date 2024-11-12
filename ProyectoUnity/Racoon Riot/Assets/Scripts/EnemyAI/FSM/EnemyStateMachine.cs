using ENEMYAI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static PlayerController;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyBaseState currentState;
    public Animator anim;
    public GameObject player;
    public bool canPatrol;
    public bool oldMan;
    public bool isDog;
    float originalPos;
    [SerializeField] GameObject oldManTeeth;
    float objectPos;

    // States
    private GuardState guardState;
    private PatrolState patrolState;
    private AlertState alertState;
    private ChaseState chaseState;
    private CaptureState captureState;
    private RangeAttackState rangeAttackState;
    private DistractState distractState;
    private JumpAttackState jumpAttackState;

    //variables
    public FieldOfView fov;
    [SerializeField] Transform[] limitPoints;

    void Start()
    {
        originalPos = transform.position.x;

        // Initialize the guardState and other states with this object's transform
        guardState = new GuardState(this,transform, fov);
        patrolState = new PatrolState(this,transform,fov, limitPoints[0], limitPoints[1]);
        alertState = new AlertState(this, transform, fov, originalPos);
        chaseState = new ChaseState(player, this, transform, limitPoints[0], limitPoints[1], fov);
        rangeAttackState = new RangeAttackState(player, this, transform, fov);
        jumpAttackState = new JumpAttackState(player.transform, this, transform, fov, limitPoints[0], limitPoints[1]);
        captureState = new CaptureState(transform, this);
        distractState = new DistractState(this, objectPos, fov);


        // Start in the GUARD state
        if (!canPatrol)TransitionToState(guardState);
        //else if (isDog && canPatrol) TransitionToState(jumpAttackState);
        else TransitionToState(patrolState);
        
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
    public void Guard()
    {
        TransitionToState(guardState);
    }

    public void Alert()
    {
        TransitionToState(alertState);
    }

    public void Distract(float objXPos)
    {
        objectPos = objXPos;
        distractState = new DistractState(this, objectPos, fov);
        TransitionToState(distractState);
    }

    public void Patrol()
    {
        TransitionToState(patrolState);
    }

    public void Chase()
    {
        TransitionToState(chaseState);
    }

    public void RangeAttack()
    {
        TransitionToState(rangeAttackState);
    }

    public void JumpAttack()
    {
        TransitionToState(jumpAttackState);
    }

    public void Capture()
    {
        TransitionToState(captureState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player" && !oldMan && !isDog)
        {
            //ACTIVATE PLAYER DAMAGE
            player.GetComponent<PlayerController>().GotCaptured(this);
            Capture();
        } else if (collision.transform.tag == "Player" && oldMan || collision.transform.tag == "Player" && isDog)
        {
            //ACTIVATE PLAYER DAMAGE
            anim.SetTrigger("isAttack");
            player.GetComponent<PlayerController>().GotDamaged();
        }
    }

    public void Instantiator()
    {
        Instantiate(oldManTeeth, fov.transform.position, Quaternion.identity);
    }
}
