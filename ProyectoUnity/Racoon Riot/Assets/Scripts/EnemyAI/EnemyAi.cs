using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D rb;
    public Animator anim;
    public GameObject objectOnSight;
    public Vector3 target;
    [SerializeField] Transform[] limitPoints;
    private Vector3 originalPosition;
    private Vector3 lastSeenPosition;

    [Header("Variables")]
    public float speed;
    public float chaseSpeed;
    public bool canMove = false;
    public Vector3 velocity;
    private Vector3 previousPosition;
    bool isFacingRight;
    public int flipDuration = 60;
    int flipTimeRemaining;
    public bool flipCountingDown = false;
    public bool canParry = false;
    public float visionRange = 10f;
    public float visionAngle = 60f;
    public int rayCount = 10;
    bool coroutineStopper;
    public bool isCapturing;

    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask objectLayer;

    public enum EnemyState
    {
        VIGILANDO,
        REVISANDO,
        PERSIGUIENDO,
        CAPTURANDO,
        JUGADOR_ESCAPO,
        REGRESANDO
    }

    public EnemyState currentState;

    void Start()
    {
        currentState = EnemyState.VIGILANDO;
        originalPosition = transform.position;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.VIGILANDO:
                if (!canMove) Vigilar();
                else Patrullar();
                break;
            case EnemyState.REVISANDO:
                StartCoroutine(MoverseHaciaObjeto());
                break;
            case EnemyState.PERSIGUIENDO:
                StartCoroutine(PerseguirAlJugador());
                break;
            case EnemyState.CAPTURANDO:
                anim.SetBool("playerCaptured", true);
                //StartCoroutine(EnemigoCapturado());
                break;
            case EnemyState.JUGADOR_ESCAPO:
                anim.SetBool("playerCaptured", false);
                isCapturing = false;
                StartCoroutine(CheckObjectThenGoBack());
                break;
            case EnemyState.REGRESANDO:
                coroutineStopper = true;
                break;
        }
    }

    public void ChangeEnemyState(int state)
    {
        if (state == 0)
        {
            //anim.SetBool("isWalking", false);
            coroutineStopper = true;
            anim.SetTrigger("attack");
            currentState = EnemyState.CAPTURANDO;
        }
        else if(state == 1)
        {
            currentState = EnemyState.JUGADOR_ESCAPO;
            coroutineStopper = false;
        }
    }


    private void Vigilar()
    {
        anim.SetBool("isWalking", false);
        if (!flipCountingDown)
        {
            flipCountingDown = true;
            flipTimeRemaining = flipDuration;
            StartCoroutine(Flip());
            //Invoke("Flip", 1f);
        }

        //genera rayos en distintos angulos
        float angleStep = visionAngle / (rayCount - 1);
        float startingAngle = isFacingRight ? -visionAngle / 2 : -visionAngle / 2;
        //informacion de cada rayo 
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startingAngle + (angleStep * i);
            Vector2 direction = DirectionFromAngle(currentAngle);

            //si el rayo toca uno de estos layers activa un nuevo enemystate
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, playerLayer | objectLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    objectOnSight = hit.transform.gameObject;
                    coroutineStopper = true;
                    //objectOnSight.GetComponent<PlayerController>().BeingChasedBy(gameObject);
                    anim.SetBool("isWalking", true);
                    currentState = EnemyState.PERSIGUIENDO;
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Object"))
                {
                    objectOnSight = hit.transform.gameObject;
                    coroutineStopper = true;
                    //go check it out
                    currentState = EnemyState.REVISANDO;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange, Color.green);
            }
        }
    }

    public void DistractEnemy(Transform objectPos)
    {
        coroutineStopper = true;
        if(objectPos.position.x < transform.position.x && isFacingRight) FlipWithoutCoroutine();
    }

    Vector2 DirectionFromAngle(float angle)
    {
        angle = isFacingRight ? angle : 180 - angle;
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    IEnumerator Flip()
    {
        while(flipTimeRemaining > 0)
        {
            flipTimeRemaining--;
            if (coroutineStopper)
            {
                coroutineStopper = false;
                yield break;
            }
            yield return null;
        }{
            flipCountingDown = false;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            isFacingRight = !isFacingRight;
        }
    }

    void FlipWithoutCoroutine()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
    }

    private void Patrullar()
    {
        target = limitPoints[0].position;
        Movement();
    }

    void Movement()
    {
        velocity = ((transform.position - previousPosition)/ Time.deltaTime);
        previousPosition = transform.position;
        anim.SetBool("isWalking", true);

        if (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            if (target == limitPoints[0].position)
            {
                if (isFacingRight)
                {
                    isFacingRight = !isFacingRight;
                    StartCoroutine("SetTarget", limitPoints[1].position);
                }
            }
            else
            {
                if (!isFacingRight)
                {
                    isFacingRight = !isFacingRight;
                    StartCoroutine("SetTarget", limitPoints[0].position);
                }
            }
        }
    }

    IEnumerator SetTarget(Vector3 position)
    { 
        yield return new WaitForSeconds(5f);
        target = position;
        FaceTowards(position - transform.position);
    }

    void FaceTowards(Vector3 direction)
    {
        if (direction.x < 0f)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    IEnumerator MoverseHaciaObjeto()
    {
        if(coroutineStopper) yield break;
        if (objectOnSight != null)
        {
            //moverse hacia objeto
            Vector3 target = new Vector3(objectOnSight.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            
            //chequear distancia hasta objeto
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            float distanceToObject = Vector3.Distance(transform.position, target);
            Debug.DrawLine(transform.position, objectOnSight.transform.position, Color.blue);

            while (distanceToObject < 1.5f)
            { //esto se reproduce cuando alcanza el distractor
                
                objectOnSight.GetComponent<CircleCollider2D>().isTrigger = true; 
                //StartCoroutine(CheckObjectThenGoBack());
                yield return null;
            }
            { //mientras va hacia el distractor
                //objectOnSight = null;
                anim.SetBool("isWalking", true);
            }
        }
    }

    IEnumerator PerseguirAlJugador()
    {
        if (coroutineStopper) yield break;
        if (objectOnSight != null)
        {
            
            //moverse hacia jugador
            Vector3 target = new Vector3(objectOnSight.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);

            //calcular distancia hacia jugador
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            float distanceToObject = Vector3.Distance(transform.position, target);
            Debug.DrawLine(transform.position, objectOnSight.transform.position, Color.blue);
            yield return null;
        }
    }

    IEnumerator EnemigoCapturado()
    {
        //objectOnSight.GetComponent<PlayerController>().isCaptured = true;
        canParry = true;
        //if (distanceToObject < 3f)
        yield return new WaitForSeconds(2f);
        canParry = false;
        //if didnt parry lose game else go back to position
    }

    IEnumerator CheckObjectThenGoBack()
    {
        if(objectOnSight == null) yield break;
        anim.SetBool("isWalking", false);
        yield return new WaitForSeconds(4f);
        if(objectOnSight.layer != 7)objectOnSight.gameObject.layer = 0;
        objectOnSight = null;
        FlipWithoutCoroutine();
        StartCoroutine(ReturnToInitialPosition());
    }

    public IEnumerator ReturnToInitialPosition()
    {
        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            currentState = EnemyState.REGRESANDO;
            anim.SetBool("isWalking", true);

            transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);

            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            float distanceToObject = Vector3.Distance(transform.position, target);


            yield return null;
        }
        {
            flipCountingDown = false;
            coroutineStopper = false;
            currentState = EnemyState.VIGILANDO;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            anim.SetBool("isWalking", false);
            coroutineStopper = true;
            
            StartCoroutine(CheckObjectThenGoBack());
        }
    }
}
