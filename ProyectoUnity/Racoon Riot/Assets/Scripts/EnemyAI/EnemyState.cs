using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public enum ENEMY_STATE
    {
        PATROL, ALERT, CHASE
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    #region Variables

    public ENEMY_STATE state;
    protected EVENT stage; //solo accesible dentro de este script
    protected Animator anim;
    protected Transform position;
    protected EnemyState nextState;

    [Header("Field Of View")]
    public SpriteRenderer FOV;
    Color whiteColor = new Color(1, 1, 1, 0.3f);
    Color yellowColor = new Color(1, 1, 0, 0.3f);
    Color redColor = new Color(1, 0, 0, 0.3f);
    public float visionRange = 10f; //view distance
    public float visionAngle = 60f; //view angle
    public int rayCount = 10; //ray density. sugested 10 or less
    public float losePlayer; //distance to lose sight

    #endregion


    public EnemyState(Transform _position, Animator _anim)
    {
        stage = EVENT.ENTER;
        anim = _anim;
        position = _position;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; } //se puede hacer override en una clase derivada de este script
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public EnemyState Process()
    { 
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT) Exit();
        {
            Exit();
            return nextState;
        }
        //return this;
    }
}
