using UnityEngine;

namespace ENEMYAI.FSM
{
    public abstract class EnemyBaseState 
    {
        public enum ENEMY_STATE
        {
            GUARD, PATROL, ALERT, CHASE, CAPTURE
        }

        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        /*
        #region Variables

        public ENEMY_STATE state;
        protected EVENT stage; //solo accesible dentro de este script
        protected GameObject enemy;
        protected Animator anim;
        protected Transform player;
        protected EnemyBaseState nextState;

        [Header("Enemy View")]
        public bool playerOnSight;
        bool isFacingRight;
        bool flipCountingDown = false;
        float flipTimeRemaining;
        float flipDuration;
        public SpriteRenderer FOV;
        Color whiteColor = new Color(1, 1, 1, 0.3f);
        Color yellowColor = new Color(1, 1, 0, 0.3f);
        Color redColor = new Color(1, 0, 0, 0.3f);
        public float visionRange = 10f; //view distance
        public float visionAngle = 60f; //view angle
        public int rayCount = 10; //ray density. sugested 10 or less
        public float losePlayer; //distance to lose sight
        public LayerMask playerLayer;
        public LayerMask groundLayer;

        #endregion
        

        public EnemyBaseState(GameObject _enemy, Transform _player, Animator _anim)
        {
            stage = EVENT.ENTER;
            enemy = _enemy;
            anim = _anim;
            player = _player;
        }

        public virtual void Enter() { stage = EVENT.UPDATE; } //se puede hacer override en una clase derivada de este script
        public virtual void Update() { stage = EVENT.UPDATE; }
        public virtual void Exit() { stage = EVENT.EXIT; }

        public EnemyBaseState Process()
        {
            
            if (stage == EVENT.ENTER) Enter();
            if (stage == EVENT.UPDATE) Update();
            if (stage == EVENT.EXIT) Exit();
            {
                Exit();
                return nextState;
            }
        }
    }

    public class Guard : EnemyBaseState
    {
        public Guard(GameObject _enemy, Transform _player, Animator _anim) : base(_enemy, _player, _anim)
        {
            //speed = 0;
            state = ENEMY_STATE.GUARD;
        }

        public override void Enter()
        {
            Debug.Log("Entering GUARD state.");
            // Add initialization for GUARD behavior here

            // anim.SetTrigger("isIdle");
            // base.Enter();
        }

        public override void Update()
        {
            Debug.Log("Updating GUARD state.");
            // Add GUARD logic here: nextState = new Patrol(enemy, player, anim);
            //stage = EVENT.EXIT;

        }

        public override void Exit()
        {
            //anim.ResetTrigger("isIdle");
            base.Exit();
        }*/
    }
}
