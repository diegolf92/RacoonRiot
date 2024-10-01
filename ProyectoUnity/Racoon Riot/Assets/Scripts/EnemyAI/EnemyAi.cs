using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    public bool isFacingRight;
    public int flipDuration = 60;
    int flipTimeRemaining;
    public bool flipCountingDown = false;
    public bool canParry = false;
    public bool coroutineStopper;
    public bool isCapturing;

    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask objectLayer;
    public LayerMask groundLayer;

    [Header("Field Of View")]
    public SpriteRenderer FOV;
    Color whiteColor = new Color(1,1,1,0.3f);
    Color yellowColor = new Color(1, 1, 0, 0.3f);
    Color redColor = new Color(1, 0, 0, 0.3f);
    public float visionRange = 10f;
    public float visionAngle = 60f;
    public int rayCount = 10;
    public float losePlayer;

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
                FOV.color = whiteColor;
                if (!canMove) Vigilar();
                else StartCoroutine(Patrullar()); RayosDePatrullaje();
                break;
            case EnemyState.REVISANDO:
                FOV.color = yellowColor;
                StartCoroutine(MoverseHaciaObjeto());
                break;
            case EnemyState.PERSIGUIENDO:
                FOV.color = redColor;
                StartCoroutine(PerseguirAlJugador());
                break;
            case EnemyState.CAPTURANDO:
                FOV.color = Color.clear;
                anim.SetBool("playerCaptured", true);
                //StartCoroutine(EnemigoCapturado());
                break;
            case EnemyState.JUGADOR_ESCAPO:
                FOV.color = yellowColor;
                anim.SetBool("playerCaptured", false);
                isCapturing = false;
                StartCoroutine(CheckObjectThenGoBack());
                break;
            case EnemyState.REGRESANDO:
                FOV.color = yellowColor;
                coroutineStopper = true;
                break;
        }
    }

    Vector3 CheckIfEnemyWithinBoundaries(Transform positionToTest)
    {
        if (positionToTest.position.x < limitPoints[0].position.x)
        {
            //this position is outside of boundaries
            return limitPoints[0].position;
        } else if (positionToTest.position.x > limitPoints[1].position.x)
        {
            return limitPoints[1].position;
        }
        else {
            //this position is inside of boundaries
            return positionToTest.position;
        }
    }

    Vector3 ConvertAngleToVector3(float angle)
    {
        //angle = 0 -> 360
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
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
        else if (state == 2)
        {
            currentState = EnemyState.REVISANDO;
            coroutineStopper = false;
        }
        else if (state == 3)
        {
            currentState = EnemyState.REGRESANDO;
            coroutineStopper = false;
        }
        else if (state == 4)
        {
            currentState = EnemyState.VIGILANDO;
            coroutineStopper = false;
        }
    }

    public void ChangeEnemyState(int state, GameObject posXToMove)
    {
        if (state == 2)
        {
            objectOnSight = posXToMove;
            currentState = EnemyState.REVISANDO;
            coroutineStopper = true;
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
            //creando cono de vision
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
        if(objectPos.position.x < transform.position.x && isFacingRight || objectPos.position.x > transform.position.x && !isFacingRight) FlipWithoutCoroutine();
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
        coroutineStopper = false;
    }

    private IEnumerator Patrullar()
    {
        //moverse hacia punto de patrullaje dependiendo del bool isFacingRight
        if (isFacingRight)
        {
            Vector3 targetPos = new Vector3(limitPoints[1].position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            //chequear distancia hasta objeto
            float distanceToObject = Vector3.Distance(transform.position, targetPos);
            Debug.DrawLine(transform.position, targetPos, Color.blue);

            if (distanceToObject < 1f)
            { //esto se reproduce cuando alcanza el distractor
                StartCoroutine(StareThenContinuePatrol());
                yield return null;
            } else
            { //mientras va hacia el distractor
                //objectOnSight = null;
                anim.SetBool("isWalking", true);
            }

        }
        else
        {
            Vector3 targetPos = new Vector3(limitPoints[0].position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            //chequear distancia hasta objeto
            float distanceToObject = Vector3.Distance(transform.position, targetPos);
            Debug.DrawLine(transform.position, targetPos, Color.blue);

            if (distanceToObject < 1f)
            { //esto se reproduce cuando alcanza el distractor
                StartCoroutine(StareThenContinuePatrol());
                yield return null;
            }else
            { //mientras va hacia el distractor
                //objectOnSight = null;
                anim.SetBool("isWalking", true);
            }
        }
    }

    private void RayosDePatrullaje()
    {
        //genera rayos en distintos angulos
        float angleStep = visionAngle / (rayCount - 1);
        float startingAngle = isFacingRight ? -visionAngle / 2 : -visionAngle / 2;
        //informacion de cada rayo 
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startingAngle + (angleStep * i);
            Vector2 direction = DirectionFromAngle(currentAngle);

            //si el rayo toca uno de estos layers activa un nuevo enemystate
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, playerLayer | objectLayer | groundLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    objectOnSight = hit.transform.gameObject;
                    //coroutineStopper = true;
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
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Stop the ray at the obstacle
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                }
            }
            else
            {
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange, Color.green);
            }
        }
    }

    IEnumerator StareThenContinuePatrol()
    {
        if (coroutineStopper) yield break;
            coroutineStopper = true;
        anim.SetBool("isWalking", false);
        yield return new WaitForSeconds(4f);
        FlipWithoutCoroutine();
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
            Vector3 boundaryTest = CheckIfEnemyWithinBoundaries(objectOnSight.transform); //chequea si objeto a seguir no se sale de los limites de movimiento del enemigo
            //moverse hacia objeto
            Vector3 target = new Vector3(boundaryTest.x, transform.position.y, transform.position.z);
            
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            
            //chequear distancia hasta objeto
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            float distanceToObject = Vector3.Distance(transform.position, target);
            Debug.DrawLine(transform.position, objectOnSight.transform.position, Color.blue);

            while (distanceToObject < 1.5f)
            { //esto se reproduce cuando alcanza el distractor
                
                if(objectOnSight.GetComponent<CircleCollider2D>() !=null) objectOnSight.GetComponent<CircleCollider2D>().isTrigger = true; 
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
            Vector3 boundaryTest = CheckIfEnemyWithinBoundaries(objectOnSight.transform); //chequea si jugador no se sale de los limites de movimiento del enemigo
            //moverse hacia jugador
            Vector3 target = new Vector3(boundaryTest.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);

            //calcular distancia hacia jugador
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            float distanceToObject = Vector3.Distance(transform.position, target);
            float distanceToPlayer = Vector3.Distance(transform.position, objectOnSight.transform.position);
            if (distanceToPlayer > losePlayer) { ChangeEnemyState(1); } else { Debug.Log("onsight"); }
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
        if (objectOnSight.layer != 7) objectOnSight.gameObject.layer = 0;
        else if (objectOnSight == null) Debug.Log("Nothing On Sight");
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
