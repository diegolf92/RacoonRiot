using PLAYERAI.PlayerFSM;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalState : PlayerBaseState
{
    private PlayerStateMachine fsm;
    Rigidbody2D rb;
    Animator anim;

    //variables
    bool isFacingRight = true;
    private float moveX;
    private float moveY;
    float speed = 5;
    private bool isGrounded;
    float checkRadius = 0.03f;
    private LayerMask groundLayer;
    private LayerMask ceilingLayer;
    private LayerMask wallLayer;
    bool isWallJumping, isWallSliding;
    bool isCrouching;
    bool canCrouch = true;
    bool isCeiling;
    BoxCollider2D playerCollider;
    Vector2 boxColNormalSize;
    Vector2 boxColCrouchSize = new Vector2(0.75f, 0.43f);
    Vector2 boxColSlideSize = new Vector2(0.4f, 0.54f);
    float wallSlideSpeed = 2f;
    float wallJumpDirection = 0.5f;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration;
    Vector2 wallJumpPower = new Vector2(3f, 10f);
    float jumpForce = 12f;

    public NormalState(PlayerStateMachine enemyStateMachine, Rigidbody2D rigid, Animator animator)
    {
        fsm = enemyStateMachine;
        rb = rigid;
        anim = animator;
    }

    public override void Enter()
    {
        groundLayer = 3;
        wallLayer = 6;
        ceilingLayer = 8;
        playerCollider = fsm.GetComponent<BoxCollider2D>();
        boxColNormalSize = playerCollider.size;
    }

    public override void Update()
    {
        DetectGround();
        DetectCeiling();
        Crouch();
        WallSlide();
        WallJump();
        if (!isWallJumping) { Move(); }
        if (!isWallJumping) { Flip(); }

        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
            isWallJumping = false;
            isWallSliding = false;
            anim.SetBool("isWallSliding", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && canCrouch)
        {
            isCrouching = !isCrouching;
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

    private void Jump()
    {
        anim.SetTrigger("jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        if (isFacingRight && moveX < 0f || !isFacingRight && moveX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = fsm.transform.localScale;
            localScale.x *= -1f;
            fsm.transform.localScale = localScale;
        }
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

        if (!isCrouching)
        {
            isCrouching = false;
            anim.SetBool("crouch", false);
            anim.SetBool("crouchMove", false);
            playerCollider.size = boxColNormalSize;
        }
    }

    private void WallSlide()
    {
        if (DetectWall() && !isGrounded && rb.velocity.y <= 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            playerCollider.size = boxColSlideSize;
            anim.GetComponent<SpriteRenderer>().flipX = true;
            playerCollider.sharedMaterial = fsm.raccoonMaterial[1];
            anim.SetBool("isWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            //playerCollider.size = boxColNormalSize;
            anim.GetComponent<SpriteRenderer>().flipX = false;
            playerCollider.sharedMaterial = fsm.raccoonMaterial[0];
            anim.SetBool("isWallSliding", false);
        }
    }

    private void WallJump()
    {
        if (rb.velocity.y <= 0 && isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -fsm.transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            //CancelInvoke(nameof(StopWallJumping));
            isWallJumping = false;
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

            if (fsm.transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = fsm.transform.localScale;
                localScale.x *= -1f;
                fsm.transform.localScale = localScale;
            }
            //Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private bool DetectGround()
    {
        isGrounded = Physics2D.OverlapCircle(fsm.feetPosFront.position, checkRadius, groundLayer);
        isGrounded = Physics2D.OverlapCircle(fsm.feetPosBack.position, checkRadius, groundLayer);
        anim.SetBool("grounded", isGrounded);
        return isGrounded;
    }

    private bool DetectWall()
    {
        return Physics2D.OverlapCircle(fsm.wallPos.position, checkRadius, wallLayer);
    }

    private bool DetectCeiling()
    {
        isCeiling = Physics2D.OverlapCircle(fsm.ceilingPos.position, checkRadius, ceilingLayer);
        return isCeiling;
    }

    public override void Exit()
    {
    }
}
