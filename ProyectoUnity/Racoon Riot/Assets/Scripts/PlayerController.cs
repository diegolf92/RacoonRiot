using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerSprite;
    Animator anim;

    [Header("Movimiento")]
    private float moveX;
    private float moveY;
    public float speed;
    public float jumpForce;
    [SerializeField] private Rigidbody2D rb;
    bool isFacingRight = true;

    [Header("Deteccion de contacto")]
    //ground
    [SerializeField] private LayerMask groundLayer;
    public float checkRadius = 0.03f;
    public Transform pivotPos;
    [SerializeField] private bool isGrounded;

    //ceiling
    [SerializeField] private LayerMask ceilingLayer;
    public Transform ceilingPos;
    [SerializeField] private bool isCeiling;

    //slide
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    public float wallSlideSpeed = 2f;
    [SerializeField] private bool isWallSliding;

    //wall jump
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration;
    [SerializeField] private Vector2 wallJumpPower = new Vector2(8f, 16f);

    //crouch
    BoxCollider2D playerCollider;
    bool isCrouching;
    Vector2 boxColNormalSize;
    public Vector2 boxColCrouchSize;
    float offsetCollider = -0.4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = playerSprite.GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        boxColNormalSize = playerCollider.size;
    }

    void Update()
    {
        if (!isWallJumping) { Move(); }
        if (!isWallJumping) { Flip(); }
        DetectGround();
        DetectCeiling();
        WallSlide();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        WallJump();

        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            isCrouching = true;
            anim.SetBool("crouch", true);
            Crouch(offsetCollider);
            playerCollider.size = boxColCrouchSize;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && !isCeiling)
        {
            isCrouching = false;
            anim.SetBool("crouch", false);
            Crouch(-offsetCollider);
            playerCollider.size = boxColNormalSize;
        }
    }

    private bool DetectGround()
    {
        isGrounded = Physics2D.OverlapCircle(pivotPos.position, checkRadius, groundLayer);
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
        if (DetectWall() && !DetectGround() && moveX != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        if (!isCrouching)
        {
            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveX * speed / 3, rb.velocity.y);
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
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
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

            Invoke(nameof(StopWallJump), wallJumpDuration);
        }
    }

    private void StopWallJump()
    {
        isWallJumping = false;
    }

    private void Crouch(float amount)
    {
        playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y + amount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "elevator")
        {
            transform.parent = collision.gameObject.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "elevator")
        {
            transform.parent = null;
        }
    }
}