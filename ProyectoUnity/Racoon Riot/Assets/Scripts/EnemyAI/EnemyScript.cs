using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Variables")]
    public float speed = 200f;
    public float newWaypointDistance = 3f;
    public float visionRange = 10f;
    public float visionAngle = 60f;
    float flipInterval;
    public int rayCount = 10;
    public float alertCooldown = 3.0f; 
    bool isFacingRight = true;
    bool isChasing;
    bool reachedEndOfPath;
    public bool canFlip = true;

    [Header("Components")]
    Rigidbody2D rb;
    public Animator anim;
    public GameObject objectOnSight;

    [Header("Position")]
    public Transform target;
    public Transform player;
    [SerializeField] Transform[] limitPoints;
    private Vector3 originalPosition;
    private Vector3 lastSeenPosition;

    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask wallLayer;
    public LayerMask objectLayer;

    public enum EnemyState
    {
        VIGILANDO,
        PERSIGUIENDO,
        CAPTURANDO,
        ALERTADO,
        DISTRAIDO,
        NORMALIZANDO
    }

    public EnemyState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.VIGILANDO;
        originalPosition = transform.position;
        
    }

    void Update()
    {
        
        switch(currentState)
        {
            case EnemyState.VIGILANDO:
                Vigilar();
                
                break;
            case EnemyState.PERSIGUIENDO:
                IrTrasJugador();
                break;
            case EnemyState.CAPTURANDO:
                CapturePlayer();
                break;
            case EnemyState.ALERTADO:
                StartCoroutine(LostPlayerGuardRoutine());
                break;
            case EnemyState.DISTRAIDO:
                StartCoroutine(GoCheckOut());
                break;
            case EnemyState.NORMALIZANDO:
                break;
        }
    }

    private void Vigilar()
    {
        anim.SetBool("isWalking", false);
        if (canFlip) StartCoroutine(FlipCoroutine());
        //genera rayos en distintos angulos
        float angleStep = visionAngle / (rayCount - 1);
        float startingAngle = isFacingRight ? -visionAngle/2 : -visionAngle/2;
        //informacion de cada rayo 
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startingAngle + (angleStep * i);
            Vector2 direction = DirectionFromAngle(currentAngle);

            //si el rayo toca uno de estos layers activa un nuevo enemystate
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, playerLayer | wallLayer | objectLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    player = hit.collider.transform;
                    currentState = EnemyState.PERSIGUIENDO;
                } else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Object"))
                {
                    objectOnSight = hit.transform.gameObject;
                    //go check it out
                    currentState = EnemyState.DISTRAIDO;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange, Color.green);
            }
        }
    }

    Vector2 DirectionFromAngle(float angle)
    {
        angle = isFacingRight ? angle : 180 - angle;
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    IEnumerator FlipCoroutine()
    {
        canFlip = false;
        yield return new WaitForSeconds(10);
        canFlip = true;
        Flip();
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
    }

    private void IrTrasJugador()
    {
        if(player != null)
        {
            anim.SetBool("isWalking", true);
            //captura la posicion del jugador y se dirige alli
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            rb.MovePosition(targetPosition);
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            if (limitPoints[0].position.x > transform.position.x || limitPoints[1].position.x < transform.position.x)
            {
                anim.SetBool("isWalking", false);
                currentState = EnemyState.ALERTADO;
                return;
            }
            /*RaycastHit2D wallHit = Physics2D.Raycast(target.transform.position, direction, visionRange/8, wallLayer);
            if (wallHit.collider != null)
            {
                //lastSeenPosition = transform.position;
                currentState = EnemyState.ALERTADO;
                StartCoroutine(LostPlayerGuardRoutine());
                return;
            }*/
        }
    }

    public void ChangeStateToDistract(GameObject objectPos)
    {
        //StopCoroutine(FlipCoroutine());
        if (objectPos.transform.position.x < transform.position.x && isFacingRight || objectPos.transform.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }

    IEnumerator GoCheckOut()
    {
        if (objectOnSight != null)
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            rb.MovePosition(targetPosition);
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            RaycastHit2D objectHit = Physics2D.Raycast(objectOnSight.transform.position, direction, visionRange / 8, objectLayer);
            float distanceToObject = Vector3.Distance(transform.position, objectHit.collider.transform.position);
            Debug.DrawLine(transform.position, objectHit.transform.position, Color.blue);

            while (distanceToObject < 1.5f)
            { //esto se activa cuando alcance el distractor
                anim.SetBool("isWalking", false);
                StartCoroutine(CheckObjectThenGoBack());
                yield return null;
            }
            { //esto se reproduce cuando es alertado y mientras va hacia el distractor
                objectOnSight = null;
                anim.SetBool("isWalking", true);
            }
        }
    }

    IEnumerator CheckObjectThenGoBack()
    {
        
        yield return new WaitForSeconds(alertCooldown);
        
        objectOnSight.gameObject.layer = 0;
        
        StartCoroutine(ReturnToInitialPosition());
    }

    public void CapturingPlayer()
    {
        currentState = EnemyState.CAPTURANDO;
    }

    void CapturePlayer()
    {
        if (player != null)
        {
            // Stop the player from moving
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
                playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        } 
    }

    IEnumerator LostPlayerGuardRoutine()
    {
        yield return new WaitForSeconds(alertCooldown);

        // If the player is not detected, return to initial position and resume guarding
        if (currentState == EnemyState.ALERTADO)
        {
            player = null;
            //currentState = EnemyState.VIGILANDO;
            if (transform.position.x < originalPosition.x)
            { Flip(); }
            StartCoroutine(ReturnToInitialPosition());
        }
    }

    IEnumerator ReturnToInitialPosition()
    {
        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            anim.SetBool("isWalking", true);
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            rb.MovePosition(targetPosition);
            
            yield return null;
        }
        {
            currentState = EnemyState.VIGILANDO;
            
        }   
    }

    // Optional: Visualize the vision range in the scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        float angleStep = visionAngle / (rayCount - 1);
        float startingAngle = isFacingRight ? -visionAngle / 2 : visionAngle / 2;

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startingAngle + (angleStep * i);
            Vector2 direction = DirectionFromAngle(currentAngle);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange);
        }

        //Gizmos.color = Color.red;
        //Vector3 direction = isFacingRight ? Vector3.right : Vector3.left;
        //Gizmos.DrawLine(transform.position, transform.position + direction * visionRange);
    }

    
}
