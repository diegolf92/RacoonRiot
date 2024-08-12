using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool hasFallen = false;
    private Vector2 initialPosition;
    private SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    private Sprite originalSprite;
    public EnemyAi enemyScript;
    public float speedSensitivity = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = rb.position;
    }

    void Update()
    {
        if (!hasFallen && rb.velocity.y < speedSensitivity)
        {
            hasFallen = true;
        }

        if (hasFallen && Mathf.Approximately(rb.velocity.y, 0))
        {
            rb.bodyType = RigidbodyType2D.Static;
            if (enemyScript.currentState == EnemyAi.EnemyState.VIGILANDO) enemyScript.DistractEnemy(transform);
            hasFallen = false;
            
            ChangeSprite();
        }
    }

    // Reset the object (optional, for testing or level reset purposes)
    public void ResetObject()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.position = initialPosition;
        hasFallen = false;
        if (spriteRenderer != null && newSprite != null)
        {
            // Assuming you have a reference to the original sprite to reset it
            spriteRenderer.sprite = originalSprite;
        }
    }

    private void ChangeSprite()
    {
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }
}