using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerSprite;
    public bool coroutineStopper;
    EnemyAi enemyChasing;
    int parryCount;
    public Animator anim;
    public GameObject taco;

    [Header("Movimiento")]
    private float moveX;
    private float moveY;
    public float speed;
    public float jumpForce;
    [SerializeField] private Rigidbody2D rb;
    public bool isFacingRight = true;
    public bool isJumping;
    [SerializeField] PhysicsMaterial2D[] raccoonMaterial;

    [Header("Deteccion de contacto")]
    //ground
    [SerializeField] private LayerMask groundLayer;
    public float checkRadius = 0.03f;
    public Transform pivotPos;
    public Transform pivotPosTwo;
    [SerializeField] private bool isGrounded;
    bool coolDown;
    public bool isCaptured = false;

    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip jumpSound;


    //ceiling
    [SerializeField] private LayerMask ceilingLayer;
    public Transform ceilingPos;
    [SerializeField] private bool isCeiling;

    //slide
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    public float wallSlideSpeed = 2f;
    [SerializeField] private bool isWallSliding;

    [Header("Wall Jump")]
    private bool isWallJumping;
    public float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration;
    [SerializeField] private Vector2 wallJumpPower = new Vector2(8f, 16f);
    
    //crouch
    BoxCollider2D playerCollider;
    public bool isCrouching;
    Vector2 boxColNormalSize;
    public Vector2 boxColCrouchSize;
    public Vector2 boxColSlideSize;
    public PlayerLife damage;

    public enum PlayerState
    {
        NORMAL,
        CAPTURADO,
        MURIENDO
    }

    public PlayerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = playerSprite.GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        boxColNormalSize = playerCollider.size;
        currentState = PlayerState.NORMAL;
    }

    void Update()
    {
        switch (currentState)
        {
            case PlayerState.NORMAL:
                
                if (isGrounded)
                {
                    isJumping = false;
                    anim.SetBool("isJumping", false);
                    isWallJumping = false;
                    isWallSliding = false;
                    anim.SetBool("isWallSliding", false);
                }

                if (!isWallJumping) { Move(); }
                if (!isWallJumping) { Flip(); }

                Crouch();
                DetectGround();
                DetectCeiling();
                WallSlide();
                WallJump();

                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    Jump();
                }

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    isCrouching = !isCrouching;
                }
                break;


            case PlayerState.CAPTURADO:
                gameObject.layer = 0;
                StartCoroutine(ParryCoroutine());
                break;

            case PlayerState.MURIENDO:
                Die();
                break;
        }
    }

    IEnumerator ParryCoroutine()
    {
        isCaptured = true;
        if (coroutineStopper)
        {
            yield break; // Exit the coroutine if it has already run
        }

        playerSprite.GetComponent<SpriteRenderer>().enabled = false;
        
        //if you get caught and parry 3 times while enemy canParry is true you escape, else you lose
        if (Input.GetKeyDown(KeyCode.Space))
        {
            parryCount++;
        }

        if (parryCount > 5)
        {
            coroutineStopper = true;
            enemyChasing.ChangeEnemyState(1);
            StartCoroutine(EscapeTime());
            yield break;
        }

        yield return new WaitForSeconds(3f);

        if (isCaptured)
        {
            damage.EnemyDamage();
            currentState = PlayerState.NORMAL;
            gameObject.layer = 7;
            playerSprite.GetComponent<SpriteRenderer>().enabled = true;
            enemyChasing.ChangeEnemyState(1);
            StartCoroutine(EscapeTime());
            enemyChasing = null;
            isCaptured = false;
            yield break;
        }
    }

    IEnumerator EscapeTime()
    {
        coolDown = true;
        currentState = PlayerState.NORMAL;
        if (isFacingRight)
        {
            Vector3 offset = new Vector3(2f, 0, 0);
            transform.position = enemyChasing.transform.position + offset;
        }
        else {
            Vector3 offset = new Vector3(-2f, 0, 0);
            transform.position = enemyChasing.transform.position + offset;
        }
        playerSprite.GetComponent<SpriteRenderer>().enabled = true;
        playerSprite.GetComponent<SpriteRenderer>().color = Color.gray;
        yield return new WaitForSeconds(3f);
        gameObject.layer = 7;
        coroutineStopper = false;
        playerSprite.GetComponent<SpriteRenderer>().color = Color.white;
        enemyChasing = null;
        parryCount = 0;
        coolDown = false;
    }

    void Crouch()
    {
        if (isCrouching || isCeiling)
        {
            playerCollider.size = boxColCrouchSize;
            if (moveX != 0)
            {
                anim.SetBool("crouch", false);
                anim.SetBool("crouchMove", true);
            }
            else
            {
                anim.SetBool("crouch", true);
                anim.SetBool("crouchMove", false);
            }
        }

        if(!isCrouching)
        {
            isCrouching = false;
            anim.SetBool("crouch", false);
            anim.SetBool("crouchMove", false);
            playerCollider.size = boxColNormalSize;
        }
    }

    private bool DetectGround()
    {
        isGrounded = Physics2D.OverlapCircle(pivotPos.position, checkRadius, groundLayer);
        isGrounded = Physics2D.OverlapCircle(pivotPosTwo.position, checkRadius, groundLayer);
        anim.SetBool("grounded", isGrounded);
        return isGrounded;
    }

    private bool DetectCeiling()
    {
        isCeiling = Physics2D.OverlapCircle(ceilingPos.position, checkRadius, ceilingLayer);
        return isCeiling;
    }

    private bool DetectWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);
    }

    private void WallSlide()
    {
        if (DetectWall() && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            playerCollider.size = boxColSlideSize;
            playerSprite.GetComponent<SpriteRenderer>().flipX = true;
            playerCollider.sharedMaterial = raccoonMaterial[1];
            anim.SetBool("isWallSliding", true); 
        }
        else
        {
            isWallSliding = false;
            //playerCollider.size = boxColNormalSize;
            playerSprite.GetComponent<SpriteRenderer>().flipX = false;
            playerCollider.sharedMaterial = raccoonMaterial[0];
            anim.SetBool("isWallSliding", false);
        }
    }

    private void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        anim.SetFloat("speedX", Mathf.Abs(moveX));
        anim.SetFloat("speedY", rb.velocity.y);

        if (!isCrouching)
        {
            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
            if (moveX != 0 && isGrounded)
            {
                anim.SetBool("run", true);
            }
            else
            {
                anim.SetBool("run", false);
            }
        }
        else
        {
            rb.velocity = new Vector2(moveX * speed / 3, rb.velocity.y);
            anim.SetBool("run", false);
        }

    }

    private void Flip()
    {
        if (isFacingRight && moveX < 0f || !isFacingRight && moveX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Jump()
    {
        isJumping = true;
        anim.SetTrigger("jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            anim.SetTrigger("wallJump");
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public void Die()
    {
        anim.SetBool("muerteHambre", true);
        anim.SetBool("isJumping", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "elevator")
        {
            transform.parent = collision.gameObject.transform;
        }

        if (collision.gameObject.tag == ("KillZone"))
        {
            damage.ApplyDamage();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "elevator")
        {
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SecretWall"))
        {
            //smooth out fade
            collision.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        }

        if (collision.CompareTag("Enemy") && !coolDown)
        {
            enemyChasing = collision.GetComponent<EnemyAi>();
            enemyChasing.ChangeEnemyState(0);
            currentState = PlayerState.CAPTURADO;
        }
        else if (collision.CompareTag("Enemy") && coolDown)
        {
            enemyChasing = collision.GetComponent<EnemyAi>();
            enemyChasing.ChangeEnemyState(1);
        }

        if (collision.CompareTag("Food"))
        {
            damage.TimeSliderAdd();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Goal"))
        {
            anim.SetBool("washing", true);
            taco.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SecretWall"))
        {
            //smooth out fade
            collision.GetComponent<SpriteRenderer>().color = new Color(0.26f, 0.3f, 0.33f, 1);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pivotPos.transform.position, checkRadius);
        Gizmos.DrawSphere(wallCheck.transform.position, checkRadius);
        Gizmos.DrawSphere(ceilingPos.transform.position, checkRadius);
    }
}