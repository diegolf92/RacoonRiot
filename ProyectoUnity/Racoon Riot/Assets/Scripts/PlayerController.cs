using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed;
    public float jumpForce;
    [SerializeField] private Rigidbody2D rb;

    [Header("Deteccion de suelo")]
    [SerializeField] private bool isGrounded; 
    public LayerMask groundLayer;
    public float checkRadius = 0.03f;
    public Transform pivotPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        DetectGround();
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private bool DetectGround()
    {
        isGrounded = Physics2D.OverlapCircle(pivotPos.position, checkRadius, groundLayer);
        return isGrounded;
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
