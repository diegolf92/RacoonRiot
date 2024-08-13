using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    EnemyScript enemy;
    public Transform pointA; // The first patrol point
    public Transform pointB; // The second patrol point
    public float walkSpeed = 1.0f; // Patrol speed
    private bool movingToPointB = true; // Direction flag
    private Vector2 targetPosition;
    public Transform target;
    public Transform player;
    bool isChasing;
    public float lostPlayerGuardTime = 3.0f; // Time to guard after losing the player
    public LayerMask stopEnemyLayer;
    public LayerMask groundLayer;
    private Vector3 lastSeenPosition;

    public float visionRange = 10f;
    public float visionAngle = 60f;
    public int rayCount = 10;
    private Vector3 originalPosition;
    bool isFacingRight = true;
    public float flipInterval;
    public LayerMask playerLayer;
    //
    public float speed = 200f;
    public float newWaypointDistance = 3f;

    bool reachedEndOfPath;
    public Rigidbody2D rb;

    public enum EnemyState
    {
        VIGILANDO,
        PERSIGUIENDO,
        CAPTURANDO,
        LostPlayerGuarding
    }

    public EnemyState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.VIGILANDO;
        originalPosition = transform.position;
        targetPosition = pointB.position;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.VIGILANDO:
                Vigilar();
                Patrol();
                break;
            case EnemyState.PERSIGUIENDO:
                IrTrasJugador();
                break;
            case EnemyState.CAPTURANDO:
                CapturePlayer();
                break;
            case EnemyState.LostPlayerGuarding:
                GuardAfterLosingPlayer();
                break;
        }
    }

    private void Vigilar()
    {
        float angleStep = visionAngle / (rayCount - 1);
        float startingAngle = isFacingRight ? -visionAngle / 2 : -visionAngle / 2;
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startingAngle + (angleStep * i);
            Vector2 direction = DirectionFromAngle(currentAngle);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, playerLayer | stopEnemyLayer | groundLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    player = hit.collider.transform;
                    currentState = EnemyState.PERSIGUIENDO;
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("StopEnemy") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
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
        //is returning null reference
        //Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        /*
        RaycastHit2D hit = Physics2D.Raycast(target.transform.position, direction, visionRange, playerLayer);
        if(hit.collider != null)
        {
            player = hit.collider.transform;
            currentState = EnemyState.PERSIGUIENDO;
            StopCoroutine(FlipCoroutine());
        } else if(transform.position != originalPosition)
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            rb.MovePosition(targetPosition);
        }*/
    }

    Vector2 DirectionFromAngle(float angle)
    {
        angle = isFacingRight ? angle : 180 - angle;
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
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
        if (player != null)
        {
            if (transform.position.x > pointA.position.x && transform.position.x < pointB.position.x)
            {
                Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                rb.MovePosition(targetPosition);
                Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
                RaycastHit2D wallHit = Physics2D.Raycast(target.transform.position, direction, visionRange / 16, stopEnemyLayer);
                if (wallHit.collider != null)
                {
                    //lastSeenPosition = transform.position;
                    currentState = EnemyState.LostPlayerGuarding;
                    StartCoroutine(LostPlayerGuardRoutine());
                    return;
                }
            }
        }
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
            if (rb != null)
            {
                playerRb.velocity = Vector2.zero;
                playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    void GuardAfterLosingPlayer()
    {
        // Guard at the last seen position
        // Vector2 targetPosition = Vector2.MoveTowards(transform.position, lastSeenPosition, speed * Time.deltaTime);
        // rb.MovePosition(targetPosition);

        // Check if the player is detected again
        Vigilar();
    }

    IEnumerator LostPlayerGuardRoutine()
    {
        yield return new WaitForSeconds(lostPlayerGuardTime);

        // If the player is not detected, return to initial position and resume guarding
        if (currentState == EnemyState.LostPlayerGuarding)
        {
            currentState = EnemyState.VIGILANDO;
            StartCoroutine(ReturnToInitialPosition());
        }
    }

    IEnumerator ReturnToInitialPosition()
    {
        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            rb.MovePosition(targetPosition);
            yield return null;
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

    private void Patrol()
    {
        Vector2 currentPosition = rb.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, walkSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            if (movingToPointB)
            {
                targetPosition = pointA.position;
            }
            else
            {
                targetPosition = pointB.position;
            }

            movingToPointB = !movingToPointB;
            Flip();
        }
    }
}
