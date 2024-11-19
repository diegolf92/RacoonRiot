using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    private Sprite originalSprite;
    public EnemyStateMachine enemyScript;
    public float fallingSpeedThreshold = -10f;  // Speed at which to call Distract
    private bool isFalling = false;         // Track if Distract has been called

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = rb.position;
    }

    void Update()
    {
        // Check if the falling speed exceeds the threshold and Distract hasn't been called yet
        if (rb.velocity.y <= fallingSpeedThreshold && !isFalling)
        {
            isFalling = true;  // Ensure Distract is only called once
        }
    }

    void StopMovement()
    {
        Debug.Log("sadasd");
        isFalling = false;
        // Stop the object's movement by setting velocity to zero
        spriteRenderer.sprite = newSprite;
        enemyScript.Distract(transform.position.x);
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;  // Make the object stop interacting with physics
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pushable") && isFalling)
        {
            StopMovement();
        }
    }
}